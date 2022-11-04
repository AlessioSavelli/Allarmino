/*
   Questo file contiene tutte le funzioni, variabili e macchine a stati che vengono utilizzate per gestire la connessione WiFi, notifiche Telegram ed E-Mail
   Le notiche telegram ha un modulo a parte mentre quello E-mail è incorporato qui
*/
#include "time.h"
#include "sys/time.h"

#define TELEGRAM_LOOP_TIME 200 // effettua un controllo dei messaggi telegram ogni 300ms

#if INTERNET_MODE  == INTERNET_Eth

#if ETHERNET_CHIPSET==EHT_CHIPSET_W5500
#include <Ethernet.h>     //Includo l'ethernet per la compatibilità W5500
#include <EthernetUDP.h>
#elif ETHERNET_CHIPSET==EHT_CHIPSET_ENC28J60
#include <EthernetENC.h>  //Includo l'ethernet per la compatibilità ENC28J60
#endif

#include <NTPClient.h>
EthernetUDP ntpUDP;
NTPClient timeClient(ntpUDP, NTP_SERVER, GMT_TIME, 60 * SECONDS);
byte mac[] = {
  ETH_MAC
};
//Fine include Ethernet
#elif INTERNET_MODE  == INTERNET_WiFi
#include <WiFi.h>
#include <WiFiClient.h>
#endif

#ifdef ENABLE_EMAIL
#include <ESP_Mail_Client.h>
/*
   BUFSIZE_EMAIL_SEND da tenere tra 20 e 50
   l'email vengono inviate ogni volta che l'rtc raggiunge l'orario specificato in Setup nel define ORARIO_INVIO_REPORT
*/
#define BUFSIZE_EMAIL_SEND     30 // Mette in buffer max 30 eventi da inviare
//-----------------

String Email_Buffer_Text[BUFSIZE_EMAIL_SEND];
unsigned int email_index_buffer = 0;

//Creazione SMTP per Email
SMTPSession smtp;
ESP_Mail_Session session;
SMTP_Message message;


void send_email_message(String _message);

#endif

struct tm RTCTime; //ottiene l'informazioni dell'rtc

//Macchina a stati che gestisce il WiFi e la comunicazione
enum st_internet {
  INTERNET_ETHERNET_FATAL_FAIL,
  INTERNET_SYSTEM_STARTUP,
  INTERNET_CONNECTION1, INTERNET_CONNECTION2, INTERNET_CONNECTION3, // Attende la connessione WiFi
  INTERNET_SET_TELEGRAM, INTERNET_SET_EMAIL, INTERNET_SET_NTP, // Inizializzo i componenti usati dal WiFi
  INTERNET_CHECK,
  INTERNET_LOOP_TELEGRAM, INTERNET_TELEGRAM_SEND,
  INTERNET_LOOP_EMAIL, INTERNET_EMAIL_BODY, INTERNET_EMAIL_SEND,
  INTERNET_RESET_WIFI
};

byte internet_st = INTERNET_SYSTEM_STARTUP;

void InternetLoop();

void send_notify(String _message);
void send_notify(String _message, bool type);

String getTimeToString();
String getHoursToString();
String convertToString(char* a, int size); // usato per la conversione in stringa
