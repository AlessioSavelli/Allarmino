/*
  Questo è un file di configurazione della centralina.
  Qui devono essere definiti tutti i parametri di comunicazione delle centralina per il WiFi,
  Come devono essere gestiti gli input/output programmabili,
  Filtri anti rimbalzo delle 4 zone e deltamper,
  Parametri del bot telegram e della chat con cui può comunicare,
  Cicli d'allarme e durata sirena.
  Configurazione SMTP per invio e-mail di notifica
  Escludibilità delle zone e del tamper
  pin-out dell'hardware

  -I messaggi di avviso telegram e email sono preconfigurati e si possono modificare direttamente da codice nel file myWiFi.ino
*/
#define FW_VERSION "1.0V"

//Configurazione pin-out board
#define SIRENA  33
#define OUTPUT1 25
#define OUTPUT2 12
#define INPUT1  13
#define INPUT2  32

#define ZONA1   34
#define ZONA2   35
#define ZONA3   26
#define ZONA4   27
#define TAMPER  14

#define LED_AL  2
#define LED_ST  15
#define BUZZER  17 // Corrisponde a TX2

#define CS1_SPI 5
#define CS2_SPI 4

//-----------

//----Definisco gli eventi a  cui possono essere associati gli input-----
#define INPUT_AS_NONE         0x00 // serve per identificare che l'input non è collegato a nulla
#define INPUT_AS_KEY          0x01 // serve per configurare l'input viene usato come chiave di attivazione del sistema
#define INPUT_AS_POWER_CHECK  0x02 // serve per configurare l'input come la presenza rete elettrica
//------
//----Definisco gli eventi a cui possono essere associate gli output----
#define OUTPUT_AS_ST_ALLARM       0x00 // serve per pilotare l'output quando la sirena è in allarme
#define OUTPUT_AS_NO_INTERNET     0x01 // serve per pilotare l'output in assenza di connessione WiFi
#define OUTPUT_AS_NO_POWER        0x02 // serve per pilotare l'output quando manca l'energia elettrica
#define OUTPUT_AS_TAMPER_ESCLUSO  0x03 // serve per pilotare l'output quando il tamper viene escluso dal ciclo di controllo
#define OUTPUT_AS_TAMPER_ALLARME  0x04 // serve per pilotare l'output quando il tamper viene manomesso ( Real-Time )
#define OUTPUT_AS_TELEGRAM        0x05 // serve per pilotare l'output attraverso i comandi Telegram
//-----

//---Configuro gli eventi che voglio associare agli input
#define INPUT1_AS_EVENT INPUT_AS_KEY
#define INPUT2_AS_EVENT INPUT_AS_POWER_CHECK
//-----

//---Configuro gli eventi che voglio associare agli OUTPUT
#define OUTPUT1_AS_EVENT OUTPUT_AS_TELEGRAM
#define OUTPUT2_AS_EVENT OUTPUT_AS_NO_INTERNET
//-----

//---Configurazioni tempi sirena e attivazione allarme---
#define MAX_TIME_SIRENA_ON      600 * SECONDS
#define CYCLE_TIMER_SIRENA_ON   120 * SECONDS 
#define SIRENA_ON_DELAYED       15 * SECONDS  
#define ALLARME_ON_DELAYED      15 * SECONDS 
//------

//---Configuro le zone che si possono auto escludere---
#define ZONA1_ESCLUDIBILE true
#define ZONA2_ESCLUDIBILE true
#define ZONA3_ESCLUDIBILE true
#define ZONA4_ESCLUDIBILE true

#define TAMPER_ESCLUDIBILE false
//----

//Configurazione anti rimbalzo
#define DEBOUNCE_ZONA1 100 // 100ms di filtro sulla zona1
#define DEBOUNCE_ZONA2 100 // 100ms di filtro sulla zona2
#define DEBOUNCE_ZONA3 100 // 100ms di filtro sulla zona3
#define DEBOUNCE_ZONA4 100 // 100ms di filtro sulla zona4

#define DEBOUNCE_TAMPER 100 // 100ms di filtro sull'anti tamper

#define DEBOUNCE_INPUT1 100 // 100ms di filtro sull'input1
#define DEBOUNCE_INPUT2 100 // 100ms di filtro sull'input2
//--------


//---Configurazione WiFi-----
#define WiFi_SSID ""
#define WiFi_PASS ""
#define WiFi_MODE 0 // 0 significa WIFI_STA , 1 significa WIFI_AP_STA


#define WiFi_CONNECTION_TRIES_DELAY 5000 //5000ms tra una prova e l'altra
#define WiFi_CONNECTION_ERROR_AFTER_TRIES 2 // dopo 2 volte che prova a connettersi triggera il flag di errore wifi. tentativi massimi 255
//--------

//Configurazione Telegram BOOT
#define ENABLE_TELEGRAM //Se commenti il define il bot viene escluso
#define TELEGRAM_TOKEN  ""

//Il bot funziona solo nei gruppi e non accetta messaggi in chat privata, la risposta arriva a tutti i membri del gruppo di cui fa parte.
//Inserire i dati del gruppo su cui viene abilitato il bot
#define TELEGRAM_GROUP_NAME   ""
#define TELEGRAM_GROUP_ID     -9999999


//------

//Configurazione NTP per RTC over WiFi
/*GMT TABLE
   San Paolo  GMT  -3
   San Tiago  GMT  -4
   New York   GMT  -5
   Arizona    GMT  -7
   London     GMT   0
   Parigi     GMT  +1
   Berlino    GMT  +1
   Roma       GMT  +1
   Instabul   GMT  +2
   Mosca      GMT  +3
*/
//Impostare GMT per la timezone
#define GMT_TIME            1                    //GTM standard time-zone
#define GMT_ADJUSTMENT      GETGMT(GMT_TIME)     
#define NTP_SERVER          "europe.pool.ntp.org"
#define AUTO_ADJUSTMENT                       //Se commenti questo define togli l'auto adjustment del tempo in base all'ora legale e solare

#ifdef AUTO_ADJUSTMENT // configurazione fuso orario per la timezone
#define GMT_AUTOADJUSTMENT  "CET-1CEST-2,M3.5.0/02:00:00,M10.5.0/03:00:00"
#endif
//------

//Configurazione E-Mail


#define ENABLE_EMAIL   //Se commenti il define l’invio dell'email viene escluso
#define SMTP_SENDER_NAME  ""

#define SMTP_HOST ""
#define SMTP_PORT 0

//Credenziali di accesso all'email
#define SMTP_AUTHOR_EMAIL ""
#define SMTP_AUTHOR_PASSWORD ""

#define SMTP_USER_DOMAIN ""
//Destinatario Email
#define RECIPIENT_EMAIL ""

#define ORARIO_INVIO_REPORT "16:30"  // Importante è tenere il seguente formato HH:MM es. 12:00, invierà il report ogni giorno alle ore 12h

//-------
