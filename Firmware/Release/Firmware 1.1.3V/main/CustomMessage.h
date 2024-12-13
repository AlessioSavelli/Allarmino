/*
   Configurazione dei messaggi Telegram
*/
//--Questi messaggi sono gli stessi che riceverei tramite E-Mail e Telegram per le notifiche degli eventi
#define MSG_TG_WAKEUP_MESSAGE "-Il sistema anti intrusione si è acceso-"

#define MSG_TG_ALLARME_ZONA1    "Zona 1 in allarme"
#define MSG_TG_ALLARME_ZONA2    "Zona 2 in allarme"
#define MSG_TG_ALLARME_ZONA3    "Zona 3 in allarme"
#define MSG_TG_ALLARME_ZONA4    "Zona 4 in allarme"
#define MSG_TG_ALLARME_TAMPER   "Anti manomissione in allarme"

#define MSG_TG_RIPOSO_ZONA1   "Zona 1 a riposo"
#define MSG_TG_RIPOSO_ZONA2   "Zona 2 a riposo"
#define MSG_TG_RIPOSO_ZONA3   "Zona 3 a riposo"
#define MSG_TG_RIPOSO_ZONA4   "Zona 4 a riposo"
#define MSG_TG_RIPOSO_TAMPER  "Tamper a riposo"

#define MSG_TG_ESCLUSIONE_ZONA1   "Zona 1 a esclusa"
#define MSG_TG_ESCLUSIONE_ZONA2   "Zona 2 a esclusa"
#define MSG_TG_ESCLUSIONE_ZONA3   "Zona 3 a esclusa"
#define MSG_TG_ESCLUSIONE_ZONA4   "Zona 4 a esclusa"
#define MSG_TG_ESCLUSIONE_TAMPER  "Tamper a escluso"

#define MSG_TG_ALLERT_ZONE_ESCLUSE              "!!ATTENZIONE alcune zone sono escluse!!"
#define MSG_TG_ALLERT_TAMPER_ESCLUSO_ALLARME_ON "!! Tamper Escluso !!.\nALLARME ATTIVO"
#define MSG_TG_ALLERT_INTRUSIONE_RILEVATA       "!! INTRUSIONE RILEVATA !!"

#define MSG_TG_INPUT1_ON "Input 1 HIGH"
#define MSG_TG_INPUT2_ON "Input 2 HIGH"

#define MSG_TG_INPUT1_OFF "Input 1 LOW"
#define MSG_TG_INPUT2_OFF "Input 2 LOW"

#define MSG_TG_OUTPUT1_ON "Output 1 ON"
#define MSG_TG_OUTPUT2_ON "Output 2 ON"

#define MSG_TG_OUTPUT1_OFF "Output 1 OFF"
#define MSG_TG_OUTPUT2_OFF "Output 2 OFF"

#define MSG_TG_INSERIMENTO_ALLARME    "Inserimento allarme"
#define MSG_TG_DISINSERIMENTO_ALLARME "Disinserimento allarme"

#define MSG_TG_ALLARME_INSERITA       "Allarme inserita correttamente"
#define MSG_TG_ALLARME_DISINSERITA    "Allarme disinserita con successo"
#define MSG_TG_ALLARME_NON_INSERIBILE "!! Allarme non attivabile !!.\nAllarme disinserita"

#define MSG_TG_ETHERNET_CONNESSO    "Ethernet Connesso"
#define MSG_TG_ETHERNET_DISCONNESSO "Ethernet Disconnesso"

#define MSG_TG_WiFi_CONNESSO    "WiFi Connesso"
#define MSG_TG_WiFi_DISCONNESSO "WiFi Disconnesso"

#define MSG_TG_NTP_NO_CONNECTION "Orario non disponibile, impossibile raggiungere il server NTP."

#define MSG_TG_MANCANZA_POWERLINE   "Lina AC Assente"
#define MSG_TG_PRESENZA_POWERLINE   "Linea AC Presente"
#define MSG_TG_RIPRISTINO_POWERLINE "Linea AC Ripristinata"

//--End messaggi comuni

/*
   definisco i messaggi da inviare per la sessione di admin
*/
#define MSG_TG_ADMIN_SESSION_WELCOM_FROM    "Ciao Admin, Inserisci la password di accesso! Hai solo 3 tentativi consecutivi ogni 24h!\n"
#define MSG_TG_ADMIN_SESSION_LOGOUT_MESSAGE "L'utente è stato disconnesso"
#define MSG_TG_ADMIN_SESSION_INCORRECT_PSW  "Password Errata!\nProva ancora"
#define MSG_TG_ADMIN_SESSION_CORRECT_PSW    "Login effettuato!"

#define MSG_TG_ADMIN_SESSION_INFO_IDLE_TIME        "Hai il tempo limitato prima che la sessione scada, invia il firmware.bin ORA\nASSICURATI CHE SIA QUELLO CORRETTO\nOppure usa /logout per chiudere la sessione."
#define MSG_TG_ADMIN_SESSION_INFO_ABORT_UPDATE     "Hai annullato l'aggiornamento."
#define MSG_TG_ADMIN_SESSION_INFO_CONFIRM_UPDATE   "Hai confermato l'aggiornamento."
//#define MSG_TG_ADMIN_SESSION_INFO_UPDATE_DONE      "Aggiornamento avvenuto con successo.\nIl sistema si riavviera in pochi secondi.."
#define MSG_TG_ADMIN_SESSION_INFO_UPDATE_FAIL      "Aggiornamento fallito, nessuna modifica è stata apportata alla centralina."
#define MSG_TG_ADMIN_SESSION_INFO_UPDATE_NO_SPACE  "Aggiornamento fallito, spazio nella Flash insufficiente!.\nAggiornare tramite usb."

#define MSG_TG_ADMIN_SESSION_ALLERT_INVALIDFILE         "Il file che hai inserito non è valido!!\nRiprova."
#define MSG_TG_ADMIN_SESSION_ALLERT_LOGIN_BLOCKED       "Sei stato bloccato per 24h."
#define MSG_TG_ADMIN_SESSION_ALLERT_IS_FULL             "Impossibile avviare la sessione in quanto un altro utente è già loggato.\n Massimo un utente per volta!."
#define MSG_TG_ADMIN_SESSION_ALLERT_EXPIRIED_TIME       "LogOut forzato\nSessione scaduta, hai superato il tempo limite a tua disposizione!."
#define MSG_TG_ADMIN_SESSION_ALLERT_DISABLED_PRG_JMPR   "La sessione admin non può essere abilitata!."

#define MSG_TG_ADMIN_SESSION_ACCEPT_FW_UPDATE             "Please, reply to this message with 'yes' to confirm"
#define MSG_TG_ADMIN_SESSION_INVALID_SIZE_FW_UPDATE       "File is unavailable. Maybe size limit 20MB was reached or file deleted.\nTry to restart Allarmino"
#define MSG_TG_ADMIN_SESSION_START_FW_UPDATE              "Start flashing... please wait (~30/60s)"
/*
    Definizione nomi della tastiera di HELP
*/

#define MSG_TG_HELP_COMMAND "/help"

#define BUTTON_TG_TURN_ON_OUTPUT1 "/Output1_ON"
#define BUTTON_TG_TURN_ON_OUTPUT2 "/Output2_ON"

#define BUTTON_TG_TURN_OFF_OUTPUT1 "/Output1_OFF"
#define BUTTON_TG_TURN_OFF_OUTPUT2 "/Output2_OFF"

#define BUTTON_TG_READ_INPUT1 "/getinput1"
#define BUTTON_TG_READ_INPUT2 "/getinput2"

#define BUTTON_TG_READ_POWERLINE "/getpowerline"

#define BUTTON_TG_ST_ALlARME "/Stato_Allarme"

#define BUTTON_TG_TURN_ON_ALLARME "/Attiva_Allarme"
#define BUTTON_TG_TURN_OFF_ALLARME "/Disattiva_Allarme"

#define BUTTON_TG_STATUS_TAMPER "/get_tamper"
#define BUTTON_TG_STATUS_ZONE "/get_zone"

#define BUTTON_TG_FW_VERS "/fw_version"
#define BUTTON_TG_ADMIN_LOGIN "/advanced_login"
