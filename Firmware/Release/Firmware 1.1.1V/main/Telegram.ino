#ifdef ENABLE_TELEGRAM //include la libreria telegram se è abilitato
void telegram_init() {
  telegram_init(true);
}
void telegram_init(bool make_help_board) {
  
  myBot.setUpdateTime(1500);
  myBot.setTelegramToken(TELEGRAM_TOKEN);
  myBot.begin();
  //Creo la keyboaard per l'helpboard
  if (make_help_board)  create_telegram_helpboard();
}

void telegram_loop() {
  static bool ad_ses_lg = false;
  static bool ad_ss_us_ban = false;
  static int64_t ad_ss_id = 0;
  //Definisco le macro per questo void , permette di gestire correttamente la variabili create per il login
#define TG_ADMIN_SESSION_RESET       ad_ss_id=0; ad_ses_lg=false;ad_ss_id=false;ad_ss_us_ban=false;TG_ABORT_TIMEOUT_ADMIN //Reseeto la sessione di amministratore
#define TG_ADMIN_SESSION_FREE        (ad_ss_id==0) //La sessione admin è disponibile
#define TG_ADMIN_SESSION_START(id)   ad_ss_id = id //avvia la sessione amminsitratore
#define TG_ADMIN_LOGGED              ad_ses_lg     //Identifica se l'utente è loggato oppure no
#define TG_ADMIN_LOGOUT              !ad_ses_lg   //Identifica se l'utente ha mandato il messaggio di logout admin session
#define TG_ADMIN_USERID              ad_ss_id    //Ritorno l'id telegram dell'amministratore
#define TG_ADMIN_ENABLED(id)         !telegram_admin_user_is_banned(&id) // controlla se l'utente è stato bannato nelle ultime 24h
#define TG_ADMIN_BANN_NOW            ad_ss_us_ban  //Indica se deve bannare l'utente

#define TG_ADD_TO_BAN_LSIT(id)       tg_banlist[tg_banlist_index][0]= id;START_TIMER(tg_banlist[tg_banlist_index][1]);CIRC_BUFFER_ONESCROLL(tg_banlist_index, BANLIST_TG_SIZE)

  //----


  //----Controllo perenne del timeout Admin Session

  //controllo il timeout della sessione utente anche se non ci sono messaggi disponibili
  if (TG_EXPIRED_TIME_ADMIN_SESSION && TG_ALIVE_TIMEOUT_ADMIN && !TG_ADMIN_SESSION_FREE) { // controlla il timeout della richeista Admin Session
    TG_ABORT_TIMEOUT_ADMIN;
    myBot.sendTo(TG_ADMIN_USERID, MSG_TG_ADMIN_SESSION_ALLERT_EXPIRIED_TIME);
    TG_ADMIN_SESSION_RESET;
    Serial.println("TIME OUT");
  }

  //------

  static TBMessage msg;
  if (myBot.getNewMessage(msg)) {
    //Controlla se riceve i dati dal gruppo giusto
    if (msg.chatId == TELEGRAM_GROUP_ID ) {
      if (msg.text.equalsIgnoreCase(MSG_TG_HELP_COMMAND)) {
        myBot.sendMessage(msg, "Helpboard attivata", myHelpBoard);
      } else if ((msg.text.equalsIgnoreCase(BUTTON_TG_TURN_ON_OUTPUT1)) && (OUTPUT1_AS_EVENT == OUTPUT_AS_TELEGRAM)) {
        trg_output1 = true;
        new_st_output1 = true; // modifica lo stato dell'uscita
      } else if ((msg.text.equalsIgnoreCase(BUTTON_TG_TURN_ON_OUTPUT2)) && (OUTPUT2_AS_EVENT == OUTPUT_AS_TELEGRAM)) {
        trg_output2 = true;
        new_st_output2 = true; // modifica lo stato dell'uscita
      } else if ((msg.text.equalsIgnoreCase(BUTTON_TG_TURN_OFF_OUTPUT1)) && (OUTPUT1_AS_EVENT == OUTPUT_AS_TELEGRAM)) {
        trg_output1 = true;
        new_st_output1 = false; // modifica lo stato dell'uscita
      } else if ((msg.text.equalsIgnoreCase(BUTTON_TG_TURN_OFF_OUTPUT2)) && (OUTPUT2_AS_EVENT == OUTPUT_AS_TELEGRAM)) {
        trg_output2 = true;
        new_st_output2 = false; // modifica lo stato dell'uscita
      } else if (msg.text.equalsIgnoreCase(BUTTON_TG_READ_INPUT1)) {
        String str = (ISSENSORESCLUSO(stato_input1)) ? MSG_TG_INPUT1_OFF : MSG_TG_INPUT1_ON;
        myBot.sendMessage(msg,  str);
      } else if (msg.text.equalsIgnoreCase(BUTTON_TG_READ_INPUT2)) {
        String str = (ISSENSORESCLUSO(stato_input2)) ? MSG_TG_INPUT2_OFF : MSG_TG_INPUT2_ON;
        myBot.sendMessage(msg,  str);
      }  else if (msg.text.equalsIgnoreCase(BUTTON_TG_STATUS_TAMPER)) {
        String str = (ISSENSORRIPOSO(stato_tamper)) ? MSG_TG_RIPOSO_TAMPER : MSG_TG_ALLARME_TAMPER;
        str = (ISSENSORESCLUSO(stato_tamper)) ? (str + " - " +  MSG_TG_ESCLUSIONE_TAMPER) : str;
        myBot.sendMessage(msg, str);
      } else if (msg.text.equalsIgnoreCase(BUTTON_TG_STATUS_ZONE)) {
        String output = "";
        String str = (ISSENSORRIPOSO(stato_zona1)) ? MSG_TG_RIPOSO_ZONA1 : MSG_TG_ALLARME_ZONA1;
        str = (ISSENSORESCLUSO(stato_zona1)) ? (str + " - " + MSG_TG_ESCLUSIONE_ZONA1) : str;
        output += str + "\n";

        str = (ISSENSORRIPOSO(stato_zona2)) ? MSG_TG_RIPOSO_ZONA2 : MSG_TG_ALLARME_ZONA2;
        str = (ISSENSORESCLUSO(stato_zona2)) ? (str +  " - " + MSG_TG_ESCLUSIONE_ZONA2) : str;

        output += str + "\n";

        str = (ISSENSORRIPOSO(stato_zona3)) ? MSG_TG_RIPOSO_ZONA3 : MSG_TG_ALLARME_ZONA3;
        str = (ISSENSORESCLUSO(stato_zona3)) ? (str + " - " + MSG_TG_ESCLUSIONE_ZONA3) : str;
        output += str + "\n";

        str = (ISSENSORRIPOSO(stato_zona4)) ? MSG_TG_RIPOSO_ZONA4 : MSG_TG_ALLARME_ZONA4;
        str = (ISSENSORESCLUSO(stato_zona4)) ? (str +  " - " + MSG_TG_ESCLUSIONE_ZONA4) : str;
        output += str + "\n";
        myBot.sendMessage(msg, output);

      } else if (msg.text.equalsIgnoreCase(BUTTON_TG_ST_ALlARME)) {
        String str = (stato_key_allarme  == ST_DISATTIVA_ALLARME) ? MSG_TG_ALLARME_DISINSERITA : MSG_TG_ALLARME_INSERITA;
        myBot.sendMessage(msg, str);
      } else if (msg.text.equalsIgnoreCase(BUTTON_TG_TURN_ON_ALLARME)) {
        trg_change_key = true;
        new_st_key = ST_ATTIVA_ALLARME;
        while (stato_key_allarme == ST_DISATTIVA_ALLARME) { // attende l'attivazione dell'allarme
          TASKDELAY(1);
        }
        myBot.sendMessage(msg, MSG_TG_INSERIMENTO_ALLARME);
      } else if (msg.text.equalsIgnoreCase(BUTTON_TG_TURN_OFF_ALLARME)) {
        trg_change_key = true;
        new_st_key = ST_DISATTIVA_ALLARME;
        while (stato_key_allarme == ST_ATTIVA_ALLARME) { // attende la disattivazione dell'allarme
          TASKDELAY(1);
        }
        myBot.sendMessage(msg, MSG_TG_DISINSERIMENTO_ALLARME);
      } else if (msg.text.equalsIgnoreCase(BUTTON_TG_READ_POWERLINE)) { // verifica la presenza di rete
        String str = (power_state == ST_YES_POWER) ? (MSG_TG_PRESENZA_POWERLINE) : (MSG_TG_MANCANZA_POWERLINE);
        myBot.sendMessage(msg, str);
      } else if (msg.text.equalsIgnoreCase(BUTTON_TG_FW_VERS)) { // verifica la presenza di rete
        String str = FW_VERSION;
        myBot.sendMessage(msg, "Vers. FW: " + str);
      } else if (msg.text.equalsIgnoreCase(BUTTON_TG_ADMIN_LOGIN)) { // Si assicura che il jumper di programmazione remota sia abbilitato


        if (!digitalRead(PRG_JUMPER)) {
          myBot.sendMessage(msg, MSG_TG_ADMIN_SESSION_ALLERT_DISABLED_PRG_JMPR);

        } else {//Se il PRG JMPR è attivo allora avvia la sessione
          if (!TG_ADMIN_ENABLED(msg.sender.id)) {
            //mando un messaggio fittizzio
            myBot.sendTo(msg.sender.id, MSG_TG_ADMIN_SESSION_ALLERT_IS_FULL);
          } else {//se l'utente non è bannato allora controlla se può essere loggato
            if (TG_ADMIN_SESSION_FREE) {
              TG_ADMIN_SESSION_START(msg.sender.id);
              TG_START_TIMEOUT_ADMIN;
            } else {
              (myBot.sendTo(msg.sender.id, MSG_TG_ADMIN_SESSION_ALLERT_IS_FULL));
            }//End controllo sessione libera
          }//end controllo utente bannato
        }//end controllo ricezione MSG di admin session
      }//end controllo PRG JMPR attivo
      
    }//end ricezione messaggi dal gruppo

    //Eseguo il codice che gestisce la ricezione dei messaggi dalla sessione amministratore
    if (!TG_ADMIN_SESSION_FREE && (TG_ADMIN_USERID == msg.sender.id)) { //Se qualcuno ha fatto la richiesta di avvio della sessione
      if (TG_ADMIN_LOGGED) {
        telegram_admin_session(msg, TG_ADMIN_LOGGED); //gestisce i messaggi di updating firmware e il logout
        //Se ha ricevuto il messaggio di logout allora resetta la sessione
        if (TG_ADMIN_LOGOUT) { //TG_ADMIN_LOGGED fa riferimento alla variabile usata da TG_ADMIN_LOGOUT per identificare la fine della sessione
          myBot.sendTo(TG_ADMIN_USERID, MSG_TG_ADMIN_SESSION_LOGOUT_MESSAGE);
          TG_ADMIN_SESSION_RESET;
        }
      } else { //Chiede di eseguire il login
        telegram_admin_loop(msg, &TG_ADMIN_USERID, &TG_ADMIN_LOGGED, &TG_ADMIN_BANN_NOW);
        if (TG_ADMIN_BANN_NOW) {//Fase in cui banna l'utente
          TG_ADD_TO_BAN_LSIT(TG_ADMIN_USERID); // banno l'utente
          TG_ADMIN_SESSION_RESET;
          TG_ABORT_TIMEOUT_ADMIN;
        } else if (TG_ADMIN_LOGGED) {
          myBot.sendTo(TG_ADMIN_USERID, MSG_TG_ADMIN_SESSION_INFO_IDLE_TIME);
        }
      }
    }//Fine controllo admin session
  }


  //----

}

void telegram_admin_session(TBMessage &msg, bool &session_login) {
  static String fw_path;    //Il percorso remoto del firmware passato dal server telegram
  static bool updateRequest = false;  //variabile per gestire i messaggi inerenti al firmware update
  switch (msg.messageType) {
    default:
      if (msg.text.equalsIgnoreCase("/logout")) { // controlla se c'è la richista di logout
        session_login = false; // dice che la sessione deve essere chiusa
      }
      break;
    case MessageDocument:  {

        // Store in memory link to the firmware file
        fw_path = msg.document.file_path;
        if (msg.document.file_exists) {
          // Check file extension of received document (firmware must be .bin)
          if ( msg.document.file_path.endsWith(".bin")) {
            // Force reply don't work with web version of Telegram Client (use Telegram Desktop or mobile app)
            msg.force_reply = true;
            myBot.sendMessage(msg, MSG_TG_ADMIN_SESSION_ACCEPT_FW_UPDATE, "");
            updateRequest = true;
          }
        }
        else {
          myBot.sendMessage(msg, MSG_TG_ADMIN_SESSION_INVALID_SIZE_FW_UPDATE);
        }
        break;
      }
    case MessageReply: {
        // User has confirmed flash start
        if (updateRequest) {

          // se non viene confermato il messagio
          if (!msg.text.equalsIgnoreCase("yes")) {
            myBot.sendMessage(msg, MSG_TG_ADMIN_SESSION_INFO_ABORT_UPDATE);
            fw_path = "";
            break;
          }
          // Check if previous message has a valid firmware remote path
          if (fw_path.length() != 0) {
            updateRequest = false;
            myBot.sendMessage(msg, MSG_TG_ADMIN_SESSION_INFO_CONFIRM_UPDATE);
            myBot.sendMessage(msg,  MSG_TG_ADMIN_SESSION_START_FW_UPDATE); // invia il mesaggio di updating in privato
            // Inform user and query for flash confirmation with password
            myBot.sendTo(TELEGRAM_GROUP_ID, "Firmware update\nFile name: " + msg.document.file_name + "\nFile size: " + msg.document.file_size + "\nAdmin Name: " + msg.sender.firstName + " " + msg.sender.lastName + "\nAdmin UserName: @" + msg.sender.username );
          } else
            break;

          TASKDELAY(5 * SECONDS); //Attendo 5s cosi invia tutti i messagi TG

          //Avvio il download del firmware di cui bisogna fare il flashing
          switch (fw_https_download(client_fw_Update, TELEGRAM_HOST, fw_path, msg.document.file_size)) {
            case HTTP_UPDATE_CLIENT_ERROR:
            case HTTP_UPDATE_FLUSH_TIMEOUT:
            case HTTP_UPDATE_READING_TIMEOUT:
            case HTTP_UPDATE_NOHEADER_TIMEOUT:
            case HTTP_UPDATE_FAILED:
              TASKDELAY(1 * SECONDS);
              myBot.sendMessage(msg, MSG_TG_ADMIN_SESSION_INFO_UPDATE_FAIL);
              myBot.sendTo(TELEGRAM_GROUP_ID, MSG_TG_ADMIN_SESSION_INFO_UPDATE_FAIL);
              break;
            case HTTP_UPDATE_NO_SPACE:
              TASKDELAY(1 * SECONDS);
              myBot.sendMessage(msg, MSG_TG_ADMIN_SESSION_INFO_UPDATE_NO_SPACE);
              myBot.sendTo(TELEGRAM_GROUP_ID, MSG_TG_ADMIN_SESSION_INFO_UPDATE_NO_SPACE);
              break;
            case HTTP_UPDATE_OK_NO_FLUSH:
            case HTTP_UPDATE_OK:
              TASKDELAY(1 * SECONDS);
              //myBot.sendMessage(msg, MSG_TG_ADMIN_SESSION_INFO_UPDATE_DONE);
              //myBot.sendTo(TELEGRAM_GROUP_ID, MSG_TG_ADMIN_SESSION_INFO_UPDATE_DONE);
              TASKDELAY(5 * SECONDS);
              fw_apply_update();
              break;

          } //End messaggi sull'updating del firmware
          break;
        }//end update request
      }//End gestione messaggi per la sessione di aggiornamento fw
  }//End switch
}

// perform the actual update from a given stream

bool telegram_admin_user_is_banned(int64_t *req_admin_session_user_id) {
#define FIND_USER_IN_BANLIST(index,id) (tg_banlist[index][0]==*id)
#define USER_STILL_BANNED(index)       (!IS_DELAYED(tg_banlist[index][1], 24 * 60 * MINUTES))
#define REMOVE_FROM_BANLIST(index)     (tg_banlist[index][0]=0)
  //cerca l'utente se è bannato
  for (int i = 0; i < BANLIST_TG_SIZE; i++) {
    //controlla se l'utente è bannato
    if (FIND_USER_IN_BANLIST(i, req_admin_session_user_id)) {
      bool ret = USER_STILL_BANNED(i); //se l'utente è ancora bannato per 24h ritorna true altrimenti ritorna false
      if (ret)REMOVE_FROM_BANLIST(i); //rimuove l'utente dalla banlist se sono passate 24h dal suo ban
      return ret;
    }
  }
  return false; //utente non bannato
}
void telegram_admin_loop(TBMessage &_msg, int64_t *req_admin_session_user_id, bool *_telegram_admin_login_status, bool *telegram_admin_sessiong_login_banned) {

  /*
     Gestisce lo start e stop del timeout
  */

  static byte login_tryes = 0;
  switch (st_tg_admin) {
    case TG_ADMIN_LOGIN: // Richista della password
      login_tryes = 0;
      myBot.sendTo(*req_admin_session_user_id, MSG_TG_ADMIN_SESSION_WELCOM_FROM);
      myBot.sendTo(*req_admin_session_user_id, "Password :");
      st_tg_admin = TG_ADMIN_READ_PSW;
      break;
    case TG_ADMIN_READ_PSW: //Attesa e convalid inserimento password
      //Controlla se riceve i dati dall'utente che ha fatto la richiesta di login
      //if (_msg.chatId == *req_admin_session_user_id ) {
      if (telegram_admin_check_password(_msg.text)) { // convalida la password
        *_telegram_admin_login_status = true;
        myBot.sendTo(*req_admin_session_user_id, MSG_TG_ADMIN_SESSION_CORRECT_PSW);
        st_tg_admin = TG_ADMIN_LOGIN; // resetta la macchina a stati
      } else { // in caso di password errata
        *_telegram_admin_login_status = false;
        myBot.sendTo(*req_admin_session_user_id, MSG_TG_ADMIN_SESSION_INCORRECT_PSW);
        login_tryes++;
        if (login_tryes >= 3) { // se ha sbagliato 3 volte la password
          *telegram_admin_sessiong_login_banned = true; // banna l'utente
          myBot.sendTo(*req_admin_session_user_id, MSG_TG_ADMIN_SESSION_ALLERT_LOGIN_BLOCKED);
          st_tg_admin = TG_ADMIN_LOGIN; // resetta la macchina a stati
        }
      }//end controllo password

      break;

  }//End switch

}

bool telegram_admin_check_password(String password) {
  String inst_pass = TELEGRAM_PASSWORD_INSTALLATORE;
  if (inst_pass.length() != password.length()) return false;
  for (int i = 0; i < inst_pass.length(); i++) {
    if (inst_pass.charAt(i) != password.charAt(i)) return false;

  }
  return true;
}

void create_telegram_helpboard() {
  //Creo i pulsanti di TURN ON/OFF e visualizzazione stato di allarme
  myHelpBoard.addButton(BUTTON_TG_TURN_ON_ALLARME);
  myHelpBoard.addButton(BUTTON_TG_TURN_OFF_ALLARME);
  myHelpBoard.addButton(BUTTON_TG_ST_ALlARME);
  myHelpBoard.addRow();
  myHelpBoard.addButton(BUTTON_TG_STATUS_TAMPER);
  myHelpBoard.addButton(BUTTON_TG_STATUS_ZONE);
  if (OUTPUT1_AS_EVENT == OUTPUT_AS_TELEGRAM) {
    myHelpBoard.addRow();
    myHelpBoard.addButton(BUTTON_TG_TURN_ON_OUTPUT1);
    myHelpBoard.addButton(BUTTON_TG_TURN_OFF_OUTPUT1);
  }
  if (OUTPUT2_AS_EVENT == OUTPUT_AS_TELEGRAM) {
    myHelpBoard.addRow();
    myHelpBoard.addButton(BUTTON_TG_TURN_ON_OUTPUT2);
    myHelpBoard.addButton(BUTTON_TG_TURN_OFF_OUTPUT2);
  }
  myHelpBoard.addRow();
  myHelpBoard.addButton(BUTTON_TG_READ_INPUT1);
  myHelpBoard.addButton(BUTTON_TG_READ_INPUT2);
  if (power_state != ST_UNSET_POWER) myHelpBoard.addButton(BUTTON_TG_READ_POWERLINE);
  myHelpBoard.addRow();
  myHelpBoard.addButton(BUTTON_TG_FW_VERS);
  if (digitalRead(PRG_JUMPER))myHelpBoard.addButton(BUTTON_TG_ADMIN_LOGIN); // visualizza il pulsante di programmazione remota solo se il jumper è attivato
}
bool need_tx_telegram_message() {

  return (telegram_index_buffer_sended != telegram_index_buffer) ;
}
bool end_tx_telegram_message() {

  myBot.sendTo(TELEGRAM_GROUP_ID, Telegram_Buffer_Text[telegram_index_buffer_sended]);
  CIRC_BUFFER_ONESCROLL(telegram_index_buffer_sended, BUFSIZE_TELEGRAM_SEND);
  return (telegram_index_buffer_sended == telegram_index_buffer) ;
}
void send_telegram_message(String _message) {
  Telegram_Buffer_Text[telegram_index_buffer] = _message;
  CIRC_BUFFER_ONESCROLL(telegram_index_buffer, BUFSIZE_TELEGRAM_SEND);
}
#endif
