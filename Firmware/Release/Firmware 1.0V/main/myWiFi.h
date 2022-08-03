/*
   Questo file contiene tutte le funzioni, variabili e macchine a stati che vengono utilizzate per gestire la connessione WiFi, notifiche Telegram ed E-Mail
*/

#include <WiFi.h>

#include "time.h"
#ifdef ENABLE_TELEGRAM
#include "CTBot.h"
#endif
#ifdef ENABLE_EMAIL
#include <ESP_Mail_Client.h>
#endif
struct tm RTCTime; //ottiene l'informazioni dell'rtc

#define TELEGRAM_LOOP_TIME 300 // effettua un controllo dei messaggi telegram ogni 300ms
/*
   questi buffer di invio dati sono importanti quando manca internet poichè mantengono in memoria gli eventi da inviare
*/
#define BUFSIZE_TELEGRAM_SEND  10 // Mette il buffer a 10 mex 
/*
   BUFSIZE_EMAIL_SEND da tenere tra 20 e 50
   l'email vengono inviate ogni volta che l'rtc raggiunge l'orario specificato in Setup nel define ORARIO_INVIO_REPORT
*/
#define BUFSIZE_EMAIL_SEND     30 // Mette in buffer max 30 eventi da inviare
//-----------------

String Telegram_Buffer_Text[BUFSIZE_TELEGRAM_SEND];
unsigned int telegram_index_buffer = 0;

String Email_Buffer_Text[BUFSIZE_EMAIL_SEND];
unsigned int email_index_buffer = 0;


#ifdef ENABLE_TELEGRAM
CTBot myBot;
//Variabile dove vengono salvati i messaggi di telegram
TBMessage msg;
#endif
#ifdef ENABLE_EMAIL
//Creazione SMTP per Email
SMTPSession smtp;
ESP_Mail_Session session;
SMTP_Message message;
#endif

//Macchina a stati che gestisce il WiFi e la comunicazione
enum st_wifi {
  WIFI_SYSTEM_STARTUP,
  WIFI_CONNECTION1, WIFI_CONNECTION2, WIFI_CONNECTION3, // Attende la connessione WiFi
  WIFI_SET_TELEGRAM, WIFI_SET_EMAIL, WIFI_SET_NTP, // Inizializzo i componenti usati dal WiFi
  WIFI_CHECK_WIFI,
  WIFI_LOOP_TELEGRAM, WIFI_TELEGRAM_SEND,
  WIFI_LOOP_EMAIL, WIFI_EMAIL_BODY, WIFI_EMAIL_SEND,
  WIFI_RESET_WIFI
};

byte wifi_st = WIFI_SYSTEM_STARTUP;



void WiFiLoop();
#ifdef ENABLE_TELEGRAM
void  telegram_loop();
#endif
String status_IO_tostring(byte status);

void send_telegram_message(String _message);
void send_email_message(String _message);
#ifdef ENABLE_EMAIL
void smtpCallback(SMTP_Status status);
#endif
String getTimeToString();
String getHoursToString();
String convertToString(char* a, int size); // usato per la conversione in stringa
