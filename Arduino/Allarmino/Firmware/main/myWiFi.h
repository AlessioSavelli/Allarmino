/*
   Questo file contiene tutte le funzioni, variabili e macchine a stati che vengono utilizzati per gestire la connessione WiFi, notifiche Telegram ed E-Mail
*/

#include <WiFi.h>
#include <NTPClient.h>
#include <WiFiUdp.h>
#include "CTBot.h"


WiFiUDP ntpUDP;
NTPClient WiFiRTC(ntpUDP, NTP_SERVER, GMT_ADJUSTMENT, NTP_SYNC_REQUIRED);


#define TELEGRAM_LOOP_TIME 300 // ogni 300ms
/*
   questi buffer di invio dati sono importanti quando manca internet che mantengono in memoria gli eventi da inviare
*/
#define BUFSIZE_TELEGRAM_SEND  10 // Mette il buffer a 10 mex 
/*
   BUFSIZE_EMAIL_SEND da tenere tra 20 e 50
   l'email vengono inviate ogni volta che passano 120s da l'ultimo evento che ha fatto richiesta
*/
#define BUFSIZE_EMAIL_SEND     30 // Mette in buffer max 30 messaggi da inviare
//-----------------

String Telegram_Buffer_Text[BUFSIZE_TELEGRAM_SEND];
unsigned int telegram_index_buffer = 0;

String Email_Buffer_Text[BUFSIZE_EMAIL_SEND];
unsigned int email_index_buffer = 0;



CTBot myBot;
// a variable to store telegram message data
TBMessage msg;

//Macchina a stati che gestisce il WiFi e la comunicazione 
enum st_wifi {
  WIFI_CONNECTION1, WIFI_CONNECTION2, WIFI_CONNECTION3,
  WIFI_SET_TELEGRAM, WIFI_SET_EMAIL, WIFI_SET_NTP,
  WIFI_CHECK_WIFI, WIFI_LOOP_TELEGRAM, WIFI_LOOP_EMAIL, WIFI_LOOP_RTC,
  WIFI_RESET_WIFI
};

byte wifi_st = WIFI_CONNECTION1;



void WiFiLoop();

void  telegram_loop();

String status_IO_tostring(byte status);

void send_telegram_message(String _message);
void send_email_message(String _message);
