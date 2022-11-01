void InternetLoop() {
  static unsigned long internet_timer0 = 0;
  static unsigned long internet_timer1 = 0;
  static unsigned long internet_timer2 = 0;


  static byte fault_Internet_connection = 0;

  static unsigned int email_index_buffer_sended = 0;
  static String temp_string_builder = "";
  //Flag che seve per inviare il messaggio di Startup system solo la prima volta che si collega al WiFi
  static bool system_started = false;

  switch (internet_st) {
    case INTERNET_ETHERNET_FATAL_FAIL:
      //Nel caso in cui l'ethernet non funziona o non viene rilevato va in errore
      //in questo stato non fa nulla
      break;
    case INTERNET_SYSTEM_STARTUP:
      system_started = true; // Serve a triggerare l'evento di invio messaggio dopo che ha acquisito l'orario delle tramite WiFi
      internet_st = INTERNET_CONNECTION1;
      break;
    case INTERNET_CONNECTION1://Configura l'interfaccia di rete

#if INTERNET_MODE == INTERNET_WiFi
      WiFi.mode(WIFI_STA);
#elif INTERNET_MODE  == INTERNET_Eth
      //Compatibile solo con il chip ethernet W5500
      Ethernet.init(ETH_SPI_PORT); // configura la porta SPI su cui è inserito il modulo
      Ethernet.begin(mac);
#endif
      internet_connected = false;
      START_TIMER(internet_timer0);
      internet_st = INTERNET_CONNECTION2;
      break;
    case INTERNET_CONNECTION2:
#if INTERNET_MODE == INTERNET_WiFi
      if (IS_DELAYED(internet_timer0, INTERNET_CONNECTION_TRIES_DELAY)) { //aspetta un pò di tempo e poi prova a connettersi all'access point nel caso del WiFi
        WiFi.disconnect();
        WiFi.begin(WiFi_SSID, WiFi_PASS);
        START_TIMER(internet_timer0);
        internet_st = INTERNET_CONNECTION3;
      }
#elif INTERNET_MODE  == INTERNET_Eth
      START_TIMER(internet_timer0);
      internet_st = INTERNET_CONNECTION3;
      if (Ethernet.hardwareStatus() == EthernetNoHardware) internet_st = INTERNET_ETHERNET_FATAL_FAIL; // se l'hardware ethernet non viene rilevato allora interrompe la macchina a stati del WiFi/Ethernet
#endif
      break;
    case INTERNET_CONNECTION3: // rimane in attesa di connessione
#if INTERNET_MODE == INTERNET_WiFi
      switch (WiFi.status()) {
        case WL_CONNECTED://assigned when connected to a WiFi network
          internet_connected = true;//Notifica che la connessione al WiFi è andata a buon fine
          fault_Internet_connection = 0;
          send_notify(MSG_TG_WiFi_CONNESSO);
          internet_st = INTERNET_SET_TELEGRAM;
          break;
        case WL_DISCONNECTED://assigned when disconnected from a network
        case WL_IDLE_STATUS: //it is a temporary status assigned when WiFi.begin() is called and remains active until the number of attempts expires (resulting in WL_CONNECT_FAILED) or a connection is established (resulting in WL_CONNECTED)
        case WL_NO_SSID_AVAIL://assigned when no SSID are available
        case WL_CONNECT_FAILED://assigned when the connection fails for all the attempts
          if (IS_DELAYED(internet_timer0, INTERNET_CONNECTION_TRIES_DELAY)) { //Se l'attesa va in timeout si sposta nello stato precedente
            internet_st = INTERNET_CONNECTION2;
            fault_Internet_connection++;//Aggiunge un numero agli errori di connessione
          }
          break;

      }
#elif INTERNET_MODE  == INTERNET_Eth
      switch (Ethernet.linkStatus()) {
        case LinkON:
          internet_connected = true;//Notifica che la connessione al Ethernet è andata a buon fine
          fault_Internet_connection = 0;
          send_notify(MSG_TG_ETHERNET_CONNESSO);
          internet_st = INTERNET_SET_TELEGRAM;
          break;
        case Unknown:
        case LinkOFF:
          if (IS_DELAYED(internet_timer0, INTERNET_CONNECTION_TRIES_DELAY)) { //Se l'attesa va in timeout si sposta nello stato precedente
            internet_st = INTERNET_CONNECTION2;
            fault_Internet_connection++;//Aggiunge un numero agli errori di connessione
          }
          break;
      }
#endif
      if (fault_Internet_connection >= INTERNET_CONNECTION_ERROR_AFTER_TRIES) {
        internet_connected = false; // in questa fase Internet non è connesso
      }
      break;
    case INTERNET_SET_TELEGRAM:
#ifdef ENABLE_TELEGRAM
      telegram_init();
      START_TIMER(internet_timer0);//Resetto il timer per temporizzare la lettura dei messaggi telegram
#endif
      internet_st = INTERNET_SET_EMAIL;
      break;
    case INTERNET_SET_EMAIL:
#ifdef ENABLE_EMAIL
      //Configurazione della sessione SMTP
      session.server.host_name = SMTP_HOST;
      session.server.port = SMTP_PORT;
      session.login.email = SMTP_AUTHOR_EMAIL;
      session.login.password = SMTP_AUTHOR_PASSWORD;
      session.login.user_domain = SMTP_USER_DOMAIN;

      smtp.debug(0);
      //smtp.callback(smtpCallback);
      START_TIMER(internet_timer2);
#endif
      internet_st = INTERNET_SET_NTP;

      break;
    case INTERNET_SET_NTP:
#if INTERNET_MODE  == INTERNET_Eth
      //nel caso in cui si usa l'ethernet inizializza la libreria NTP
      timeClient.begin();
#endif
      configTime(GMT_ADJUSTMENT, GMT_ADJUSTMENT, NTP_SERVER);
#ifdef AUTO_ADJUSTMENT
      //configura l'auto adjustment del tempo
      setenv("TZ", GMT_AUTOADJUSTMENT, 1);
#endif
      tzset();
      START_TIMER(internet_timer1);
      while (getLocalTime(&RTCTime) && (!IS_DELAYED(internet_timer1, 15 * SECONDS))) { // Sincronizza l'orologgio per la prima volta - va in timeout se entro 15s non riesce a sincronizzarsi con il server
        //Attenda che il l'orario sia veramente sincronizzato
        TASKDELAY(100); //Attende 10ms tra una richiesta e l'altra
      }
      if (system_started) { // invia il messaggio di accensione del sistema anti intrusione
        send_notify(MSG_TG_WAKEUP_MESSAGE);
        system_started = false;
      }
      if (!IS_DELAYED(internet_timer1, 15 * SECONDS)) send_notify(MSG_TG_NTP_NO_CONNECTION);
      internet_st = INTERNET_CHECK;
      break;
    case INTERNET_CHECK:
      internet_st = INTERNET_LOOP_TELEGRAM;
#if INTERNET_MODE == INTERNET_WiFi
      if (WiFi.status() != WL_CONNECTED)internet_st = INTERNET_RESET_WIFI;
#elif INTERNET_MODE  == INTERNET_Eth
      if (Ethernet.linkStatus() != LinkON) internet_st = INTERNET_RESET_WIFI;
#endif
      break;
    case INTERNET_LOOP_TELEGRAM:
      internet_st = INTERNET_LOOP_EMAIL;
#ifdef ENABLE_TELEGRAM
      if (IS_DELAYED(internet_timer0, TELEGRAM_LOOP_TIME)) { // Riutilizzo il timer0 per temporizzare il polling di telegram ogni 300ms
        telegram_loop();
        START_TIMER(internet_timer0);
      }
      if (need_tx_telegram_message()) internet_st = INTERNET_TELEGRAM_SEND; // controlla se ci sono messaggi da inviare su telegram
#endif
      break;

#ifdef ENABLE_TELEGRAM // Se telegram è disabilitato ignora questo stato
    case INTERNET_TELEGRAM_SEND:
      //esce dal loop solo quando end_tx_telegram_message conferma di aver inviato tutti i messaggi nel buffer
      if (end_tx_telegram_message())  internet_st = INTERNET_LOOP_EMAIL; // finisce il loop di invio messaggi telegram
      break;
#endif

    case INTERNET_LOOP_EMAIL:
      internet_st = INTERNET_CHECK; // in ogni caso manda avanti la macchina a stati
#ifdef ENABLE_EMAIL
      if (email_index_buffer_sended != email_index_buffer) { // controlla se ci sono messaggi da inviare su telegram
        //Esegue il controllo dell'orario ogni minuto
        if (!IS_DELAYED(internet_timer2, 1 * MINUTES)) break;
        START_TIMER(internet_timer2);//Resetta il timer

        if (getHoursToString() != ORARIO_INVIO_REPORT) { // Se non è ancora l'orario in cui deve inviare il report allora esce dal case
          break ;
        }

        //invia l'email
        if (!smtp.connect(&session)) break;// si collega alla sessione SMTP , se fallisce esce dallo stato e riprova la volta dopo - N.B POTREBBE saltare l'invio del report
        temp_string_builder = ""; //Resetta il buffer
        internet_st = INTERNET_EMAIL_BODY;
      }
#endif
      break;
#ifdef ENABLE_EMAIL // Se l'email è disabilitata ignora questi stati
    case INTERNET_EMAIL_BODY:
      //Crea un unico testo per inviare tutto via email
      temp_string_builder += Email_Buffer_Text[email_index_buffer_sended] + "\n\n";
      CIRC_BUFFER_ONESCROLL(email_index_buffer_sended, BUFSIZE_EMAIL_SEND);
      //Condizione di uscita dal loop di creazione body
      if (email_index_buffer_sended == email_index_buffer)  internet_st = INTERNET_EMAIL_SEND; // se ha finito di creare il body dell'email la invia
      break;
    case INTERNET_EMAIL_SEND:
      //Imposta L'header del messaggio
      message.sender.name = SMTP_SENDER_NAME;
      message.sender.email = SMTP_AUTHOR_EMAIL;
      message.subject = "Report Giornaliero";
      message.addRecipient("Report", RECIPIENT_EMAIL);
      //inserisco il body del messaggio
      message.text.content = temp_string_builder.c_str();
      message.text.charSet = "us-ascii";
      message.text.transfer_encoding = Content_Transfer_Encoding::enc_7bit;
      //invia l'email
      MailClient.sendMail(&smtp, &message);

      temp_string_builder = ""; //Resetta il buffer

      internet_st = INTERNET_CHECK; // fa ricominciare il loop di internet
      break;
#endif

    case INTERNET_RESET_WIFI:

      internet_st = INTERNET_CONNECTION2;//Riparta dal tentativo di connessione wifi , non ha bisogno di riconfigurare la periferica
      fault_Internet_connection = 0; //Resetto la variabile dei tetnativi

#if INTERNET_MODE  == INTERNET_Eth
      send_notify(MSG_TG_ETHERNET_DISCONNESSO);
#elif INTERNET_MODE  == INTERNET_WiFi
      send_notify(MSG_TG_WiFi_DISCONNESSO);
#endif

      break;
  }
}

//-----

#ifdef ENABLE_EMAIL
void send_email_message(String _message) {
  Email_Buffer_Text[email_index_buffer] = _message + "\n\n";
  CIRC_BUFFER_ONESCROLL(email_index_buffer, BUFSIZE_EMAIL_SEND);
}
#endif
void send_notify(String _message) {
  String strtime = getTimeToString();
  _message = "[ " +  strtime + " ]\n" + _message;
#ifdef ENABLE_TELEGRAM
  send_telegram_message(_message);
#endif
#ifdef ENABLE_EMAIL
  send_email_message(_message);
#endif
}
void send_notify(String _message, bool type) { // Invia il messaggio solo su telegram o solo via email predisponendo i buffer adeguati
  String strtime = getTimeToString();
  _message = "[ " +  strtime + " ]\n" + _message;
#ifdef ENABLE_TELEGRAM
  if (type)send_telegram_message(_message);
#endif
#ifdef ENABLE_EMAIL
  if (!type)send_email_message(_message);
#endif
}
String getTimeToString() {
#if INTERNET_MODE  == INTERNET_Eth
  //nel caso dell'ethernet usa la libreria NTPClient prende l'epoctime e lo imposta nel rtc interno dell'esp
  timeClient.update();
  struct timeval tv;
  tv.tv_sec = timeClient.getEpochTime();  // enter UTC UNIX time (get it from https://www.unixtimestamp.com )
  settimeofday(&tv, NULL);
#endif
  if (getLocalTime(&RTCTime)) {
    char timeStringBuff[34];
    strftime(timeStringBuff, sizeof(timeStringBuff), "%d/%B/%Y %H:%M:%S", &RTCTime);
    return convertToString(timeStringBuff, sizeof(timeStringBuff) / sizeof(char));
  }
  return "Non Disponibile";
}
String getHoursToString() {
#if INTERNET_MODE  == INTERNET_Eth
  //nel caso dell'ethernet usa la libreria NTPClient prende l'epoctime e lo imposta nel rtc interno dell'esp
  timeClient.update();
  struct timeval tv;
  tv.tv_sec = timeClient.getEpochTime();  // enter UTC UNIX time (get it from https://www.unixtimestamp.com )
  settimeofday(&tv, NULL);
#endif
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
