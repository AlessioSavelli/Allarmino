/*
  Questo è un file di configurazione della centralina.
  Qui devono essere definiti tutti i parametri di comunicazione delle centralina per il WiFi,
  Come devono essere gestiti gli input programmabili e come gli output,
  Filtri anti rimbalzo delle 4 zone e dell'antitamper,
  SSID e PASSWORD di connessione al WiFi,
  Parametri del bot telegram e della chat con cui può comunicare,
  Cicli d'allarme e durata sirena.
  Configurazione SMTP per invio e-mail di notifica
  Escludibilità delle zone e del tamper
  pinout dell'hardware

  -I messaggi di avviso telegram e email sono preconfigurati e si possono modificare direttamente da codice nel file myWiFi.ino
*/

//----Definisco gli eventi a  cui possono essere associati gli input-----
#define INPUT_AS_NONE         0x00 // serve per identificare che l'input non è collegato a nulla
#define INPUT_AS_KEY          0x01 // serve per identificare che su quell'input c'è la chiave
#define INPUT_AS_POWER_CHECK  0x02 // serve per identificare che su quell'input c'è la segnalazione presenza rete elettrica
//------
//----Definisco gli eventi a cui possono essere associate gli output----
#define OUTPUT_AS_ST_ALLARM       0x00 // serve per identificare che su quell'output si vuole sapere quando suona l'allarme
#define OUTPUT_AS_NO_INTERNET     0x01 // serve per identificare che su quell'output si vuole sapere quando manca la connessione internet
#define OUTPUT_AS_NO_POWER        0x02 // serve per identificare che su quell'output si vuole sapere quando manca la rete elettrica
#define OUTPUT_AS_TAMPER_ESCLUSO  0x03 // serve per identificare che su quell'output si vuole sapere quando il tamper viene escluso  
#define OUTPUT_AS_TAMPER_ALLARME  0x04 // serve per identificare che su quell'output si vuole sapere quando c'è una manomissione sul tamper 
#define OUTPUT_AS_TELEGRAM        0x05 // serve per identificare che su quell'output si vuole pilotarlo tramite telegram
//-----


//Configurazione pinOut board
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

//---Configuro gli eventi che voglio associare agli input
#define INPUT1_AS_EVENT INPUT_AS_NONE
#define INPUT2_AS_EVENT INPUT_AS_NONE//INPUT_AS_KEY

//-----

//---Configuro gli eventi che voglio associare agli OUTPUT
#define OUTPUT1_AS_EVENT OUTPUT_AS_TELEGRAM
#define OUTPUT2_AS_EVENT OUTPUT_AS_NO_INTERNET
//-----

//---Configuro le zone che si possono auto escludere---
/*
   N.B Le zone che non vengono utilizzate si devono impostare come escludibili, il tamper ( anti manomissione ) è consigliato non escluderlo
*/
#define ZONA1_ESCLUDIBILE true
#define ZONA2_ESCLUDIBILE true
#define ZONA3_ESCLUDIBILE true
#define ZONA4_ESCLUDIBILE true

#define TAMPER_ESCLUDIBILE false

//----

//---Configurazioni tempi sirena e attivazzione allarme---
#define MAX_TIME_SIRENA_ON      10 *MINUTES  // massimo 10 minuti consecutivi dopo di che la sirena si stoppa a prescindere dagli eventi
#define CYCLE_TIMER_SIRENA_ON   10*1000//2  *MINUTES // 2minuti di sirena per ciclo di allarme. Tempo per cui la sirena deve suonare in un ciclo di allarme , N.B deve essere inferirore a MAX_TIME_SIRENA_ON
#define SIRENA_ON_DELAYED       15 * SECONDS      //(TEMPO D'INGRESSO)  Aspetta 15 secondi prima di far suonare la sirena , se in questo periodo l'allarme viene disattivata allora non suonerà
#define ALLARME_ON_DELAYED      15 * SECONDS      //(TEMPO D'USCITA)    Attende 15 secondi prima di attivare l'allarme , se dopo questo tempo i sensori sono in allarme ed escludibili allora li esclude
//------

//---Configurazione WiFi-----
#define WiFi_SSID "Internet"
#define WiFi_PASS "Francesca+73"
#define WiFi_MODE 0 // 0 significa WIFI_STA , 1 significa WIFI_AP_STA


#define WiFi_CONNECTION_TRIES_DELAY 500 //500ms tra una prova e l'altra
#define WiFi_CONNECTION_ERROR_AFTER_TRYES 5 // dopo 5 volte che prova a connettersi triggera il flag di errore wifi. valore massimo 255VOlte
//--------

//Configurazione Telegram BOOT
#define ENABLE_TELEGRAM //Se commenti il define il bot viene escluso
#define TELEGRAM_TOKEN  "1897870651:AAG_EAGlfoEfaV69d7kI3-mRzma80wT0YRE"

//Il bot funziona solo nei gruppi e non accetta messaggi in chat privata, la risposta arriva a tutti.
//Inserire i dati del gruppo su cui viene abilitato il bot
#define TELEGRAM_GROUP_NAME   "MyHouse"
#define TELEGRAM_GROUP_ID     -476690085

//------

//Configurazione NTP per la config automatica dell'RTC tramite il WiFi
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
//Set GMT per la timezone
#define GMT_ADJUSTMENT      GETGMT(+1)
#define NTP_SERVER          "europe.pool.ntp.org"
#define NTP_SYNC_REQUIRED   1 * MINUTES
//------


//Configurazione anti rimbalzo
#define DEBOUNCE_ZONA1 100 // 100ms di filtro sulla zona1
#define DEBOUNCE_ZONA2 100 // 100ms di filtro sulla zona2
#define DEBOUNCE_ZONA3 100 // 100ms di filtro sulla zona3
#define DEBOUNCE_ZONA4 100 // 100ms di filtro sulla zona4

#define DEBOUNCE_TAMPER 100 // 100ms di filtro sull'anti tamper

#define DEBOUNCE_INPUT1 100 // 100ms di filtro sull'input1
#define DEBOUNCE_INPUT2 100 // 100ms di filtro sull'input2


//--------
