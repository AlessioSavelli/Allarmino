void WiFiLoop() {
  static unsigned long wifi_timer0 = 0;
  static unsigned long wifi_timer1 = 0;
  static unsigned long wifi_timer2 = 0;


  static byte fault_WiFi_connection = 0;
  static unsigned int telegram_index_buffer_sended = 0;
  static unsigned int email_index_buffer_sended = 0;
  static String temp_string_builder = "";
  //Flag che seve per inviare il messaggio di Startup system solo la prima volta che si collega al WiFi
  static bool system_started = false;

  switch (wifi_st) {
    case WIFI_SYSTEM_STARTUP:
      system_started = true; // Serve a triggerare l'evento di invio messaggio dopo che ha acquisito l'orario delle tramite WiFi
      wifi_st = WIFI_CONNECTION1;
      break;
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
      switch (WiFi.status()) {
        case WL_CONNECTED://assigned when connected to a WiFi network
          wifi_connected = true;//Notifica che la connessione al WiFi è andata a buon fine
          fault_WiFi_connection = 0;
          send_telegram_message("Connesso al WiFi");
          send_email_message("Connesso al WiFi");
          wifi_st = WIFI_SET_TELEGRAM;
          break;
        case WL_DISCONNECTED://assigned when disconnected from a network
        case WL_IDLE_STATUS: //it is a temporary status assigned when WiFi.begin() is called and remains active until the number of attempts expires (resulting in WL_CONNECT_FAILED) or a connection is established (resulting in WL_CONNECTED)
        case WL_NO_SSID_AVAIL://assigned when no SSID are available
        case WL_CONNECT_FAILED://assigned when the connection fails for all the attempts
          if (IS_DELAYED(wifi_timer0, WiFi_CONNECTION_TRIES_DELAY)) { //Se l'attesa va in timeout si sposta nello stato precedente
            wifi_st = WIFI_CONNECTION2;
            fault_WiFi_connection++;//Aggiunge un numero agli errori di connessione
          }
          break;

      }
      if (fault_WiFi_connection >= WiFi_CONNECTION_ERROR_AFTER_TRIES) {
        wifi_connected = false; // in questa fase il WiFi non è connesso
      }
      break;
    case WIFI_SET_TELEGRAM:
#ifdef ENABLE_TELEGRAM
      myBot.setTelegramToken(TELEGRAM_TOKEN);
      START_TIMER(wifi_timer0);//Resetto il timer per temporizzare la lettura dei messaggi telegram
#endif
      wifi_st = WIFI_SET_EMAIL;
      break;
    case WIFI_SET_EMAIL:
#ifdef ENABLE_EMAIL
      //Configurazione della sessione SMTP
      session.server.host_name = SMTP_HOST;
      session.server.port = SMTP_PORT;
      session.login.email = SMTP_AUTHOR_EMAIL;
      session.login.password = SMTP_AUTHOR_PASSWORD;
      session.login.user_domain = SMTP_USER_DOMAIN;

      smtp.debug(0);
      smtp.callback(smtpCallback);
      START_TIMER(wifi_timer2);
#endif
      wifi_st = WIFI_SET_NTP;

      break;
    case WIFI_SET_NTP:
      configTime(GMT_ADJUSTMENT, GMT_ADJUSTMENT, NTP_SERVER);
#ifdef AUTO_ADJUSTMENT
      //configura l'auto adjustment del tempo
      setenv("TZ", GMT_AUTOADJUSTMENT, 1);
#endif
      getLocalTime(&RTCTime); // Sincronizza l'orologgio per la prima volta

      if (system_started) { // invia il messaggio di accensione del sistema anti intrusione
        send_telegram_message("-Il Sistema anti intrusione si è acceso-");
        send_email_message("Il Sistema anti intrusione si è acceso");
        system_started = false;
      }

      wifi_st = WIFI_CHECK_WIFI;
      break;

    case WIFI_CHECK_WIFI:
      wifi_st = WIFI_LOOP_TELEGRAM;
      if (WiFi.status() != WL_CONNECTED)wifi_st = WIFI_RESET_WIFI;
      break;
    case WIFI_LOOP_TELEGRAM:
      wifi_st = WIFI_LOOP_EMAIL;
#ifdef ENABLE_TELEGRAM
      if (IS_DELAYED(wifi_timer0, TELEGRAM_LOOP_TIME)) { // Riutilizzo il timer0 per temporizzare il polling di telegram ogni 300ms
        telegram_loop();
        START_TIMER(wifi_timer0);
      }
      if (telegram_index_buffer_sended != telegram_index_buffer) { // controlla se ci sono messaggi da inviare su telegram
        wifi_st = WIFI_TELEGRAM_SEND; // Invia il messaggio
      }
#endif

      break;
#ifdef ENABLE_TELEGRAM // Se telegram è disabilitato ignora questo stato
    case WIFI_TELEGRAM_SEND:
      myBot.sendMessage(TELEGRAM_GROUP_ID, Telegram_Buffer_Text[telegram_index_buffer_sended]);
      CIRC_BUFFER_ONESCROLL(telegram_index_buffer_sended, BUFSIZE_TELEGRAM_SEND);
      //Condizione di uscita del loop invio messaggi
      if (telegram_index_buffer_sended == telegram_index_buffer)  wifi_st = WIFI_LOOP_EMAIL; // finisce il loop di invio messaggi telegram
      break;
#endif
    case WIFI_LOOP_EMAIL:
      wifi_st = WIFI_CHECK_WIFI; // in ogni caso manda avanti la macchina a stati
#ifdef ENABLE_EMAIL
      if (email_index_buffer_sended != email_index_buffer) { // controlla se ci sono messaggi da inviare su telegram
        //Esegue il controllo dell'orario ogni minuto
        if (!IS_DELAYED(wifi_timer2, 1 * MINUTES)) break;
        START_TIMER(wifi_timer2);//Resetta il timer

        if (getHoursToString() != ORARIO_INVIO_REPORT) { // Se è l'orario in cui deve inviare il report inizia l'invio
          break ;
        }
        if (!smtp.connect(&session)) break;// si collega alla sessione SMTP , se fallisce esce dallo stato e riprova la volta dopo - N.B Potrebbe saltare l'invio del report
        temp_string_builder = ""; //Resetta il buffer
        wifi_st = WIFI_EMAIL_BODY;
      }
#endif
      break;
#ifdef ENABLE_EMAIL // Se l'email è disabilitata ignora questi stati
    case WIFI_EMAIL_BODY:
      //Crea un unico testo per inviare tutto via email
      temp_string_builder += Email_Buffer_Text[email_index_buffer_sended] + "\n\n";
      CIRC_BUFFER_ONESCROLL(email_index_buffer_sended, BUFSIZE_EMAIL_SEND);
      //Condizione di uscita dal loop di creazione body
      if (email_index_buffer_sended == email_index_buffer)  wifi_st = WIFI_EMAIL_SEND; // se ha finito di creare il body dell'email la invia
      break;
    case WIFI_EMAIL_SEND:
      wifi_st = WIFI_CHECK_WIFI; // fa ricominciare il loop

      //Imposta L'header del messaggio
      message.sender.name = SMTP_SENDER_NAME;
      message.sender.email = SMTP_AUTHOR_EMAIL;
      message.subject = "Report Giornaliero";
      message.addRecipient("Report", RECIPIENT_EMAIL);
      //Creo il body del messaggio
      message.text.content = temp_string_builder.c_str();
      message.text.charSet = "us-ascii";
      message.text.transfer_encoding = Content_Transfer_Encoding::enc_7bit;
      //invia l'email
      MailClient.sendMail(&smtp, &message);

      temp_string_builder = ""; //Resetta il buffer
      break;
#endif

    case WIFI_RESET_WIFI:
      wifi_st = WIFI_CONNECTION2;//Riparta dal tentativo di connessione wifi , non ha bisogno di riconfigurare la periferica
      fault_WiFi_connection = 0; //Resetto la variabile dei tetnativi
      send_telegram_message("Disconnessione WiFi");
      send_email_message("Disconnessione WiFi");
      break;
  }
}
#ifdef ENABLE_TELEGRAM
//sotto funzioni per telegram
void telegram_loop() {
  static bool hold_value = false;
  //Legge  i messaggi da telegram
  //if (myBot.testConnection()) {
  if (myBot.getNewMessage(msg)) {
    //Controlla se riceve i dati dal gruppo giusto
    if (msg.group.id == TELEGRAM_GROUP_ID && msg.group.title == TELEGRAM_GROUP_NAME) { // Accetta solo i messaggi che rispettano questi parametri
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
        hold_value = digitalRead(OUTPUT1);
        trg_output1 = true;
        new_st_output1 = !hold_value; // modifica lo stato dell'uscita
        //la risposta verrà inviata dal loop che gestisce gli output
      } else if (msg.text.equalsIgnoreCase("/toggleoutput2") && (OUTPUT2_AS_EVENT == OUTPUT_AS_TELEGRAM)) {
        hold_value = digitalRead(OUTPUT2);
        trg_output2 = true;
        new_st_output2 = !hold_value; // modifica lo stato dell'uscita
        //la risposta verrà inviata dal loop che gestisce gli output
        myBot.sendMessage(msg.group.id, "Attendi...");
      } else if (msg.text.equalsIgnoreCase("/readinput1")) {
        String str = (ISSENSORESCLUSO(stato_input1)) ? "Riposo" : "Allarme";
        myBot.sendMessage(msg.group.id, "Input1 :" + str);
      } else if (msg.text.equalsIgnoreCase("/readinput2")) {
        String str = (ISSENSORESCLUSO(stato_input2)) ? "Riposo" : "Allarme";
        myBot.sendMessage(msg.group.id, "Input2 :" + str);
      } else if (msg.text.equalsIgnoreCase("/readtamper")) {
        String str = (ISSENSORRIPOSO(stato_tamper)) ? "Regolare" : "Manomesso";
        str = (ISSENSORESCLUSO(stato_tamper)) ? (str + "-Escluso") : str;
        myBot.sendMessage(msg.group.id, "Anti manomissione :" + str);

      } else if (msg.text.equalsIgnoreCase("/readzone")) {
        String output = "";
        String str = (ISSENSORRIPOSO(stato_zona1)) ? "Riposo" : "Allarme";
        str = (ISSENSORESCLUSO(stato_zona1)) ? (str + "-Escluso") : str;
        output += "Zona1 :" + str + "\n";

        str = (ISSENSORRIPOSO(stato_zona2)) ? "Riposo" : "Allarme";
        str = (ISSENSORESCLUSO(stato_zona2)) ? (str + "-Escluso") : str;

        output += "Zona2 :" + str + "\n";

        str = (ISSENSORRIPOSO(stato_zona3)) ? "Riposo" : "Allarme";
        str = (ISSENSORESCLUSO(stato_zona3)) ? (str + "-Escluso") : str;
        output += "Zona3 :" + str + "\n";

        str = (ISSENSORRIPOSO(stato_zona4)) ? "Riposo" : "Allarme";
        str = (ISSENSORESCLUSO(stato_zona4)) ? (str + "-Escluso") : str;
        output += "Zona4 :" + str + "\n";
        myBot.sendMessage(msg.group.id, output);

      } else if (msg.text.equalsIgnoreCase("/stallarm")) {
        String str = (stato_key_allarme  == ST_DISATTIVA_ALLARME) ? "Non Attiva" : "Attiva";
        myBot.sendMessage(msg.group.id, "Stato Corrente dell'allarme :" + str);
      } else if (msg.text.equalsIgnoreCase("/allarmOn")) {
        trg_change_key = true;
        new_st_key = ST_ATTIVA_ALLARME;
        while (stato_key_allarme == ST_DISATTIVA_ALLARME) { // attende l'attivazione dell'allarme
          TASKDELAY(1);
        }
        myBot.sendMessage(msg.group.id, "Stato allarme : In Attivazione");
      } else if (msg.text.equalsIgnoreCase("/allarmOff")) {
        trg_change_key = true;
        new_st_key = ST_DISATTIVA_ALLARME;
        while (stato_key_allarme == ST_ATTIVA_ALLARME) { // attende la disattivazione dell'allarme
          TASKDELAY(1);
        }
        myBot.sendMessage(msg.group.id, "Stato allarme :In Disattivazione");
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
#endif
//-----

void send_telegram_message(String _message) {
  String strtime = getTimeToString();

  _message = "[ " +  strtime + " ]\n" + _message;
  Telegram_Buffer_Text[telegram_index_buffer] = _message;
  CIRC_BUFFER_ONESCROLL(telegram_index_buffer, BUFSIZE_TELEGRAM_SEND);
  //myBot.sendMessage(msg.group.id, _message);
}
void send_email_message(String _message) {
  String strtime = getTimeToString();
  _message = "[ " +  strtime + " ]\n" + _message;
  Email_Buffer_Text[email_index_buffer] = _message + "\n\n";
  CIRC_BUFFER_ONESCROLL(email_index_buffer, BUFSIZE_EMAIL_SEND);
}
#ifdef ENABLE_EMAIL
void smtpCallback(SMTP_Status status) {
  //Per ora il Callback non viene usato
  /*
    if (status.success()) {
    Serial.println("----------------");
    ESP_MAIL_PRINTF("Message sent success: %d\n", status.completedCount());
    ESP_MAIL_PRINTF("Message sent failled: %d\n", status.failedCount());
    Serial.println("----------------\n");
    struct tm dt;

    for (size_t i = 0; i < smtp.sendingResult.size(); i++) {

      SMTP_Result result = smtp.sendingResult.getItem(i);
      time_t ts = (time_t)result.timestamp;
      localtime_r(&ts, &dt);

      ESP_MAIL_PRINTF("Message No: %d\n", i + 1);
      ESP_MAIL_PRINTF("Status: %s\n", result.completed ? "success" : "failed");
      ESP_MAIL_PRINTF("Date/Time: %d/%d/%d %d:%d:%d\n", dt.tm_year + 1900, dt.tm_mon + 1, dt.tm_mday, dt.tm_hour, dt.tm_min, dt.tm_sec);
      ESP_MAIL_PRINTF("Recipient: %s\n", result.recipients);
      ESP_MAIL_PRINTF("Subject: %s\n", result.subject);
    }
    Serial.println("----------------\n");
    }
  */
}
#endif
String getTimeToString() {
  if (getLocalTime(&RTCTime)) {
    char timeStringBuff[34];
    strftime(timeStringBuff, sizeof(timeStringBuff), "%d/%B/%Y %H:%M:%S", &RTCTime);
    return convertToString(timeStringBuff, sizeof(timeStringBuff) / sizeof(char));
  }
  return "Non Disponibile";
}
String getHoursToString() {
  if (getLocalTime(&RTCTime)) {
    char timeStringBuff[7];
    strftime(timeStringBuff, sizeof(timeStringBuff), "%H:%M", &RTCTime);
    return convertToString(timeStringBuff, sizeof(timeStringBuff) / sizeof(char));
  }
  return "Non Disponibile";
}
String convertToString(char* a, int size)
{
  int i;
  String s = "";
  for (i = 0; i < size; i++) {
    if (a[i] == '\0')break;
    s = s + a[i];
  }
  return s;
}
