/*
   Questo file contiene tutte le funzioni, variabili e macchine a stati che vengono utilizzati per l'algorittimo della gestione messaggi telegram
*/
#ifdef ENABLE_TELEGRAM //include la libreria telegram se è abilitato



#include "fw_update.h"

#include <AsyncTelegram2.h>
#include <SSLClient.h>
#include "ssl_cert.h"
//#include <FS.h>

#if INTERNET_MODE  == INTERNET_Eth // include il client SSL in base a quale periferica viene utilizzata
EthernetClient  client_1;//Ethernet client usato per telegram
EthernetClient  client_2;//Ethernet client usato per l'aggiornamento FW
#elif INTERNET_MODE  == INTERNET_WiFi
WiFiClient client_1;//Ethernet client usato per telegram
WiFiClient client_2;//Ethernet client usato per l'aggiornamento FW
#endif

//creo un define per applicare la mia chiave SSL
//#define TAs       _TAs
//#define TAs_NUM   _TAs_NUM
//Ho inserito manualmente la mia chiave ssl per gli sslclient
//Aggiungo il layer SSL per i client creati
SSLClient client(client_1, _TAs, (size_t)_TAs_NUM, A0, 1, SSLClient::SSL_ERROR );
SSLClient client_fw_Update(client_2, _TAs, (size_t)_TAs_NUM, A0, 1, SSLClient::SSL_ERROR );

AsyncTelegram2 myBot(client);
ReplyKeyboard myHelpBoard;   // reply keyboard object helper

//----Define Macro per telegram
#define TG_START_TIMEOUT_ADMIN           START_TIMER(tg_timer0)
#define TG_ABORT_TIMEOUT_ADMIN           STOP_TIMER(tg_timer0)
#define TG_EXPIRED_TIME_ADMIN_SESSION    IS_DELAYED(tg_timer0, TELEGRAM_INSTALLATORE_IDLE)
#define TG_ALIVE_TIMEOUT_ADMIN           ISALIVE_TIMER(tg_timer0)
//----


//Macchina a stati che gestisce il login telegram per l'admin
enum st_tg_admin {
  TG_ADMIN_LOGIN, TG_ADMIN_READ_PSW
};

byte st_tg_admin = TG_ADMIN_LOGIN;

/*
   questi buffer di invio dati sono importanti quando manca internet poichè mantengono in memoria gli eventi da inviare
*/
#define BUFSIZE_TELEGRAM_SEND  10 // Mette il buffer a 10 mex 
#define BANLIST_TG_SIZE        20 //Gestisce il ban di massimo 20 utenti

unsigned long tg_banlist[BANLIST_TG_SIZE][2] = {0};
byte tg_banlist_index = 0;

//Variabile dove vengono salvati i messaggi di telegram
String Telegram_Buffer_Text[BUFSIZE_TELEGRAM_SEND];
unsigned int telegram_index_buffer = 0;
unsigned int telegram_index_buffer_sended = 0;

static unsigned long tg_timer0 = 0; // Usato per la gestione del timeout


void  telegram_loop();

void telegram_admin_session(TBMessage &msg, bool &session_login);
bool telegram_admin_user_is_banned(int64_t *req_admin_session_user_id);
void telegram_admin_loop(TBMessage &_msg, int64_t *req_admin_session_user_id, bool *_telegram_admin_login_status, bool *telegram_admin_sessiong_login_banned) ;
bool telegram_admin_check_password(String password);


void telegram_init();
void telegram_init(bool make_help_board);
void create_telegram_helpboard();
void my_custom_telegram_begin();
bool need_tx_telegram_message();//controlla se occore inviare i messaggi di telegram
bool end_tx_telegram_message();//invia i messaggi durente il loop del wifi
void send_telegram_message(String _message); //carica i messaggi da inviare nel buffer di telegram


#endif
