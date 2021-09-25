void WiFiLoop() {
  static unsigned long wifi_timer0 = 0;
  static unsigned long wifi_timer1 = 0;

  static byte fault_WiFi_connection = 0;
  static unsigned int telegram_index_buffer_sended = 0;
  static unsigned int email_index_buffer_sended = 0;
  static String builder_email_string = "";

  
  switch (wifi_st) {
    case WIFI_CONNECTION1://Configura l'interfaccia di rete

#if WiFi_MODE > 0
      WiFi.mode(WIFI_AP_STA);
#else
      WiFi.mode(WIFI_STA);
#endif
      wifi_connected = false;
      START_TIMER(wifi_timer0);
      wifi_st = WIFI_CONNECTION2;
      break;
    case WIFI_CONNECTION2:
      if (IS_DELAYED(wifi_timer0, WiFi_CONNECTION_TRIES_DELAY)) { //aspetta un pò di tempo e poi prova a connettersi all'access point
        WiFi.disconnect();
        WiFi.begin(WiFi_SSID, WiFi_PASS);

        START_TIMER(wifi_timer0);
        wifi_st = WIFI_CONNECTION3;
      }
      break;
    case WIFI_CONNECTION3: // rimane in attesa di connessione
      if ((WiFi.status() == WL_CONNECTED) && IS_DELAYED(wifi_timer0, WiFi_CONNECTION_TRIES_DELAY)) {
        wifi_connected = true;
        fault_WiFi_connection = 0;
        START_TIMER(wifi_timer0);
#ifdef ENABLE_TELEGRAM //Se telegram è attivo si sposta a configurare telegram altrimenti passa ala configurazione email
        wifi_st = WIFI_SET_TELEGRAM;
#else
        wifi_st = WIFI_SET_EMAIL;
#endif
        fault_WiFi_connection++;
        if (fault_WiFi_connection == 255)fault_WiFi_connection = WiFi_CONNECTION_ERROR_AFTER_TRYES; //evita l'overflow della variabile
      } else {
        if (fault_WiFi_connection >= WiFi_CONNECTION_ERROR_AFTER_TRYES)  wifi_connected = false; //Segnala che dopo un certo numero di tentativi la connessione ha fallito
      }
      break;
    case WIFI_SET_TELEGRAM:
      myBot.setTelegramToken(TELEGRAM_TOKEN);
      while (myBot.getNewMessage(msg)) { //pulisce i messaggi arrivati prima del suo riavvio
      }
      myBot.sendMessage(TELEGRAM_GROUP_ID, "-il Sistema anti intrusione si è appena acceso-");
      START_TIMER(wifi_timer0);//Resetto il timer per temporizzare la lettura dei messaggi telegram
      wifi_st = WIFI_SET_EMAIL;
      break;
    case WIFI_SET_EMAIL:
      wifi_st = WIFI_SET_NTP;
      //Da Fare
      break;
    case WIFI_SET_NTP:
      WiFiRTC.begin();//inizializza il server NTP
      if (!WiFiRTC.update()) { //aggiorna i dati dell'rtc
        WiFiRTC.forceUpdate();
      }
      wifi_st = WIFI_CHECK_WIFI;
      break;
    case WIFI_CHECK_WIFI:
#ifdef ENABLE_TELEGRAM //Se telegram è attivo si sposta nel loop telegram altrimenti passa al loop email
      wifi_st = WIFI_LOOP_TELEGRAM;
#else
      wifi_st = WIFI_LOOP_EMAIL;
#endif
      if (WiFi.status() != WL_CONNECTED)wifi_st = WIFI_RESET_WIFI;
      break;
    case WIFI_LOOP_TELEGRAM:

      if (IS_DELAYED(wifi_timer0, TELEGRAM_LOOP_TIME)) { // Riuso il timer0 per temporizzare il polling di telegram ogni 300ms
        telegram_loop();
        START_TIMER(wifi_timer0);
        
      }
      if (telegram_index_buffer_sended != telegram_index_buffer) { // controlla se ci sono messaggi da inviare su telegram
        while (telegram_index_buffer_sended != telegram_index_buffer) {//invia tutto il buffer dei messaggi su telegram
          myBot.sendMessage(msg.group.id, Telegram_Buffer_Text[telegram_index_buffer_sended]);
          CIRC_BUFFER_ONESCROLL(telegram_index_buffer_sended, BUFSIZE_TELEGRAM_SEND);
        }
      }
      wifi_st = WIFI_LOOP_EMAIL;
      break;
    case WIFI_LOOP_EMAIL:
      if (email_index_buffer_sended != email_index_buffer) { // controlla se ci sono messaggi da inviare su telegram
        while (email_index_buffer_sended != email_index_buffer) {//Crea un unico testo per inviare tutto via email
          builder_email_string += Email_Buffer_Text[email_index_buffer_sended] + "\n\n";
          CIRC_BUFFER_ONESCROLL(email_index_buffer_sended, BUFSIZE_EMAIL_SEND);
        }
        //Invio email
        builder_email_string = ""; //Reset buffer

      }
      wifi_st = WIFI_LOOP_RTC;
      break;
    case WIFI_LOOP_RTC:
      WiFiRTC.update();
      wifi_st = WIFI_CHECK_WIFI;
      break;
    case WIFI_RESET_WIFI:
      // WiFi.disconnect(); // Fa la procedura di disconnessione
      fault_WiFi_connection = 0; //Resetto la variabile dei tetnativi
      wifi_st = WIFI_CONNECTION2;//Riparta dal tentativo di connessione wifi , non ha bisogno di riconfigurare la periferica
      break;
  }
}
//sotto funzioni per telegram
void telegram_loop() {
  //Legge  i messaggi da telegram
  //if (myBot.testConnection()) {
  if (myBot.getNewMessage(msg)) {
    //Controlla se riceve i dati dal gruppo giusto
    if (msg.group.id == TELEGRAM_GROUP_ID && msg.group.title == TELEGRAM_GROUP_NAME) { // Accetta solo i messaggi che rispettano questi parametri
      //myBot.sendMessage(msg.group.id,"RICEVUTO CAPO ALL");
      if (msg.text.equalsIgnoreCase("/help")) {
        String str = "";
        //Da il comando di pilotaggio output solo se viene abilitato in fase di setup
        if (OUTPUT1_AS_EVENT == OUTPUT_AS_TELEGRAM) str += "/toggleoutput1 - Modifica lo stato dell'output\n";
        if (OUTPUT2_AS_EVENT == OUTPUT_AS_TELEGRAM) str +=  "/toggleoutput2 - Modifica lo stato dell'output\n";

        str += "/readinput1 - Legge lo stato dell'input\n";
        str += "/readinput2 - Legge lo stato dell'input\n";

        str += "/readtamper - Legge lo stato dell'antimanomissione\n";
        str += "/readzone   - Legge lo stato attuale delle zone\n";

        str += "/stallarm   - Legge lo stato dell'allarme\n";
        //Aggiunge questo comando solo se uno dei due input è impostato come presenza rete
        if (power_state != ST_UNSET_POWER)str += "/stpower   - Rileva la presenza tensione di rete\n";

        str +=  "/allarmOn  - Attiva il sistema di allarme\n";
        str += "/allarmOff - Disattiva il sistema di allarme\n";
        myBot.sendMessage(msg.group.id, str);

      } else if (msg.text.equalsIgnoreCase("/toggleoutput1") && (OUTPUT1_AS_EVENT == OUTPUT_AS_TELEGRAM)) {
        // digitalWrite(OUTPUT1, !digitalRead(OUTPUT1));
        trg_output1 = true;
        new_st_output1 = !digitalRead(OUTPUT1);
        String str = (new_st_output1) ? "ON" : "OFF";
        myBot.sendMessage(msg.group.id, "OUTPUT1 :" + str);
      } else if (msg.text.equalsIgnoreCase("/toggleoutput2") && (OUTPUT2_AS_EVENT == OUTPUT_AS_TELEGRAM)) {
        trg_output2 = true;
        new_st_output2 = !digitalRead(OUTPUT2);
        String str = (!digitalRead(OUTPUT2)) ? "ON" : "OFF";
        myBot.sendMessage(msg.group.id, "OUTPUT2 :" + str);
      } else if (msg.text.equalsIgnoreCase("/readinput1")) {
        String str = (stato_input1 == RIPOSO) ? "Riposo" : "Allarme";
        myBot.sendMessage(msg.group.id, "Input1 :" + str);
      } else if (msg.text.equalsIgnoreCase("/readinput2")) {
        String str = (stato_input2 == RIPOSO) ? "Riposo" : "Allarme";
        myBot.sendMessage(msg.group.id, "Input2 :" + str);
      } else if (msg.text.equalsIgnoreCase("/readtamper")) {
        String str = (stato_tamper  == RIPOSO) ? "Regolare" : "Manomesso";
        str = (stato_tamper   == AUTOESCLUSIONE) ? (str + "-Escluso") : str;
        myBot.sendMessage(msg.group.id, "Anti manomissione :" + str);

      } else if (msg.text.equalsIgnoreCase("/readzone")) {
        String output = "";
        String str = (stato_zona1   == RIPOSO) ? "Riposo" : "Allarme";
        str = (stato_zona1   == AUTOESCLUSIONE) ? (str + "-Escluso") : str;
        output += "Zona1 :" + str + "\n";

        str = (stato_zona2   == RIPOSO) ? "Riposo" : "Allarme";
        str = (stato_zona2   == AUTOESCLUSIONE) ? (str + "-Escluso") : str;

        output += "Zona2 :" + str + "\n";

        str = (stato_zona3   == RIPOSO) ? "Riposo" : "Allarme";
        str = (stato_zona3   == AUTOESCLUSIONE) ? (str + "-Escluso") : str;
        output += "Zona3 :" + str + "\n";

        str = (stato_zona4   == RIPOSO) ? "Riposo" : "Allarme";
        str = (stato_zona4   == AUTOESCLUSIONE) ? (str + "-Escluso") : str;
        output += "Zona4 :" + str + "\n";
        myBot.sendMessage(msg.group.id, output);

      } else if (msg.text.equalsIgnoreCase("/stallarm")) {
        String str = (stato_key_allarme  == ST_DISATTIVA_ALLARME) ? "Non Attiva" : "Attiva";
        myBot.sendMessage(msg.group.id, "Stato Corrente dell'allarme :" + str);
      } else if (msg.text.equalsIgnoreCase("/allarmOn")) {
        trg_change_key = true;
        new_st_key = ST_ATTIVA_ALLARME;
        while (stato_key_allarme == ST_DISATTIVA_ALLARME) { // attende l'attivazione dell'allarme
          delay(1);
        }
        myBot.sendMessage(msg.group.id, "Stato allarme : In Attivazione");
      } else if (msg.text.equalsIgnoreCase("/allarmOff")) {
        trg_change_key = true;
        new_st_key = ST_DISATTIVA_ALLARME;
        while (stato_key_allarme == ST_ATTIVA_ALLARME) { // attende la disattivazione dell'allarme
          delay(1);
        }
        myBot.sendMessage(msg.group.id, "Stato allarme :In Disattivata");
      } else if (msg.text.equalsIgnoreCase("/stpower")) { // verifica la presenza di rete
        String str = (power_state == ST_YES_POWER) ? ("SI") : ("NO");
        myBot.sendMessage(msg.group.id, "Presenza rete elettrica :" + str);
      } else {
        myBot.sendMessage(msg.group.id, "usa il comando /help se non sai cosa scrivere.");
      }
    }//end controllo valenza mex
  }//end if new mex

  // }
}

//-----

void send_telegram_message(String _message) {
  Telegram_Buffer_Text[telegram_index_buffer] = _message;
  CIRC_BUFFER_ONESCROLL(telegram_index_buffer, BUFSIZE_TELEGRAM_SEND);
  //myBot.sendMessage(msg.group.id, _message);
}
void send_email_message(String _message) {
  Email_Buffer_Text[email_index_buffer] = _message + "\n\n";
  CIRC_BUFFER_ONESCROLL(email_index_buffer, BUFSIZE_EMAIL_SEND);
}
