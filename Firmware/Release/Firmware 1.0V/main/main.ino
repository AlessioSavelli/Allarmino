/*
   Titolo   : Allarmino - Elettronica IN
   Hardware : only ESP32
   Ver      : 1.0V
   Data     : 13/11/2021
   Autore   : Alessio Savelli - Futura Group
*/
#include "Macro.h"
#include "setup.h"
#include "Global.h"
#include "myWiFi.h"
#include "IO_GESTURE.h"
#include "Allarme.h"

//Creiamo un task parallelo sul secondo core dell'esp32 che gestisce il secondo loop
TaskHandle_t mainTask1;
void loop2(void * pvParameters );
//Creiamo un task parallelo sul secondo core dell'esp32 che gestisce il terzo loop
TaskHandle_t mainTask2;
void loop3(void * pvParameters );

void setup() {
  Serial.begin(115200);
  delay(10);

  //Avvia il task il secondo loop
  xTaskCreate(
    loop2,           /* Task function. */
    "mainTask1",     /* name of task. */
    8 * 1024,          /* Stack size of task */
    NULL,            /* parameter of the task */
    1,               /* priority of the task */
    &mainTask1      /* Task handle to keep track of created task */
    );             
  //Avvia il task il secondo terzo
  xTaskCreate(
    loop3,           /* Task function. */
    "mainTask2",     /* name of task. */
    10 * 1024,          /* Stack size of task */
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

  pinMode(INPUT1  , INPUT);
  pinMode(INPUT2  , INPUT);

  pinMode(ZONA1  , INPUT);
  pinMode(ZONA2  , INPUT);
  pinMode(ZONA3  , INPUT);
  pinMode(ZONA4  , INPUT);

  pinMode(TAMPER  , INPUT);

  //fase di start up -  attende 5s prima di avviare il task del loop princiaple - in questo periodo si collega al WiFi e aggiorna l'RTC
  for (int i = 0; i < 10; i++) {
    digitalWrite(LED_AL, HIGH);
    digitalWrite(LED_ST, HIGH);
    TASKDELAY(250);
    digitalWrite(LED_AL, LOW);
    digitalWrite(LED_ST, LOW);
    TASKDELAY(250);
  }

}

void loop() {//task run on core 1 - 8Kb di stack
  WiFiLoop();//Gestisce tutti gli eventi wifi e le comunicazioni via internet
  (stato_key_allarme == ST_ATTIVA_ALLARME) ? (digitalWrite(LED_ST, HIGH)) : (digitalWrite(LED_ST, LOW));
  //il led AL per ora copia il funzionamento del buzzer
  digitalWrite(LED_AL, digitalRead(BUZZER));
  // Questa porzione di codice aiuta a calcoalre lo stackUsage del loop secondario
  //Serial.print("xTask stack usage: ");
  //Serial.println(uxTaskGetStackHighWaterMark( NULL ));
  TASKDELAY(3);// attende 3ms
  yield();
}

void loop2(void * pvParameters ) {//task run on core 1 - 8Kb di stacksize
  for (;;) {
    loop_allarme();    //Gestisce l'algorittimo dell'anti intrusione
    event_io_loop();   //Gestisce gli eventi associati ai vari Input e Output
    input_loop();      //Gestisce la lettura di tutti gli input della scheda
    TASKDELAY(3);// attende 3ms
    yield();
  }
}
void loop3(void * pvParameters ) { //task run on core 1  - 10Kb di stacksize
  for (;;) {
    flag_trg_loop();   //Gestisce il triggering degli eventi su flag interni del software - solo quelli che possono essere modificati da più operazioni che si svolgono in contemporanea
    output_loop();     //Gestisce il pilotaggio degli output
    TASKDELAY(1);// attende 1ms
    yield();
  }
}
