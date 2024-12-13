/*
  Titolo   : Allarmino - Elettronica IN
  Hardware : only ESP32
  Ver      : 1.1.3V
  Data     : 24/02/2024
  Autore   : Alessio Savelli - Futura Group

  da fare : aggiustamento header con gli include al posto corretto
*/
#include "Macro.h"
#include "setup.h"
#include "CustomMessage.h"
#include "Global.h"

#include "Internet.h"
#include "Telegram.h"
#include "IO_GESTURE.h"
#include "Allarme.h"

//----Definisco macro per il main
#define NO_INTERNET_CONNECTION            (internet_st==INTERNET_CONNECTION3)
#define NO_INTERNET_NOTIFY                ((internet_st==INTERNET_ETHERNET_FATAL_FAIL)  || NO_INTERNET_CONNECTION)
#define CLONE_PIN_STATUS(PINA,PINB)       digitalWrite(PINB,digitalRead(PINA))
#define TURN_ON_LED_IF_ALLARM_ON(LEDPIN)  (stato_key_allarme == ST_ATTIVA_ALLARME) ? (digitalWrite(LEDPIN, HIGH)) : (digitalWrite(LEDPIN, LOW))
#define TURN_OFF_LED_IF_ALLARM_ON(LEDPIN)  (stato_key_allarme == ST_ATTIVA_ALLARME) ? (digitalWrite(LEDPIN, LOW)) : (digitalWrite(LEDPIN, HIGH))
#define TURN_ON_LED_IF_NO_INTERNET(LEDPIN)   (NO_INTERNET_NOTIFY) ? (digitalWrite(LEDPIN, HIGH)) : (digitalWrite(LEDPIN, LOW))

//----

//----Definisco una macro per attendere che la macchina a stati di internet sia Apposto

#define CHECK_INTERNET_STARTUP     ((internet_st>=INTERNET_CHECK)||((millis()>(13*SECONDS)) && (internet_st==INTERNET_CONNECTION3)))                //Verifica che la macchina a stati di internet sia operativa e abbia inizializzato tutto

//-------

bool waiting_startup = true;

//Creiamo un task parallelo sul secondo core dell'esp32 che gestisce il secondo loop
TaskHandle_t mainTask1;
void loop2(void * pvParameters );
//Creiamo un task parallelo sul secondo core dell'esp32 che gestisce il terzo loop
TaskHandle_t mainTask2;
void loop3(void * pvParameters );

void helloStartUP() {

  //fase di start up -  attende 5s prima di avviare il task del loop princiaple - in questo periodo si collega al WiFi e aggiorna l'RTC

  while (!CHECK_INTERNET_STARTUP) { //fa lampeggiare i led, solo per i primi 25s dall'accensione, altrimenti significa che la periferica non e' ancora pronta per collegarsi a internet
    digitalWrite(LED_AL, !digitalRead(LED_AL));
    digitalWrite(LED_ST, !digitalRead(LED_ST));
    TASKDELAY(350);
  }
  digitalWrite(LED_AL, LOW);
  digitalWrite(LED_ST, LOW);
}

void setup() {
  Serial.begin(115200);
  delay(10);

  //Avvia il task il secondo loop
  xTaskCreate(
    loop2,           /* Task function. */
    "mainTask1",     /* name of task. */
    8 * KByte,          /* Stack size of task */
    NULL,            /* parameter of the task */
    1,               /* priority of the task */
    &mainTask1      /* Task handle to keep track of created task */
  );
  //Avvia il task il secondo terzo
  xTaskCreate(
    loop3,           /* Task function. */
    "mainTask2",     /* name of task. */
    20 * KByte,          /* Stack size of task */
    NULL,            /* parameter of the task */
    1,               /* priority of the task */
    &mainTask2      /* Task handle to keep track of created task */
  );


  pinMode(SIRENA, OUTPUT);
  digitalWrite(SIRENA, SIRENA_OFF);
  pinMode(OUTPUT1, OUTPUT);
  pinMode(OUTPUT2, OUTPUT);

  pinMode(LED_AL, OUTPUT);
  pinMode(LED_ST, OUTPUT);
  pinMode(BUZZER, OUTPUT);

  pinMode(PRG_JUMPER, INPUT);

  pinMode(INPUT1  , INPUT);
  pinMode(INPUT2  , INPUT);

  pinMode(ZONA1  , INPUT);
  pinMode(ZONA2  , INPUT);
  pinMode(ZONA3  , INPUT);
  pinMode(ZONA4  , INPUT);

  pinMode(TAMPER  , INPUT);



}

void loop() {//task run on core 1 - 8Kb di stack
  
  InternetLoop();//Gestisce tutti gli eventi wifi e le comunicazioni via internet
  TURN_OFF_LED_IF_ALLARM_ON(LED_ST);//accende il led ST per indicare se l'allarme è attiva o spenta
  //il led AL per ora copia il funzionamento del buzzer
  //CLONE_PIN_STATUS(BUZZER, LED_AL); // Clona  lo stato del buzzer e lo imposta sul pin LED_AL
  TURN_ON_LED_IF_NO_INTERNET(LED_AL);
  // Questa porzione di codice aiuta a calcoalre lo stackUsage del loop secondario
  //Serial.print("xTask stack usage: ");
  //Serial.println(uxTaskGetStackHighWaterMark( NULL ));
  TASKDELAY(3);// attende 3ms
  yield();
}

void loop2(void * pvParameters ) {//task run on core 1 - 8Kb di stacksize
  if (waiting_startup) { //Esegue questo codice solo all'avvio
    helloStartUP(); //mentre si avvia le periferica di Internet e le comunicazioni fa il messaggio di Hello
    waiting_startup = false;
  }
  for (;;) {
    loop_allarme();    //Gestisce l'algorittimo dell'anti intrusione
    event_io_loop();   //Gestisce gli eventi associati ai vari Input e Output
    input_loop();      //Gestisce la lettura di tutti gli input della scheda
    TASKDELAY(1);// attende 1ms
    yield();
  }
}
void loop3(void * pvParameters ) { //task run on core 1  - 10Kb di stacksize
  while (!CHECK_INTERNET_STARTUP) { // Attende che la macchina a stati di internet sia operativa e abbia inizializzato tutto
    output_loop();     //Gestisce il pilotaggio degli output
    TASKDELAY(3);// attende 3ms
    yield();
  }
  for (;;) {
    flag_trg_loop();   //Gestisce il triggering degli eventi su flag interni del software - solo quelli che possono essere modificati da più operazioni che si svolgono in contemporanea
    output_loop();     //Gestisce il pilotaggio degli output
    TASKDELAY(3);// attende 3ms
    yield();
  }
}
