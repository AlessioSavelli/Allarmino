
/*
   Titolo : Allarmino - Elettronica IN
   Hardware : ESP32
   Ver : 1.0V b
   Data : 30/06/2021
   Autore : Alessio Savelli by Futura Group
*/
#include "Macro.h"
#include "setup.h"
#include "Global.h"
#include "myWiFi.h"
#include "IO_GESTURE.h"
#include "Allarme.h"

//Creiamo un task parallelo sul secondo core dell'esp32 che gestisce la connessione WiFi

TaskHandle_t WiFiTask1;
void loop2(void * pvParameters );
void setup() {
  Serial.begin(115200);
  Serial.println("START-UP");

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


  xTaskCreatePinnedToCore(
    loop2,           /* Task function. */
    "WiFiTask1",     /* name of task. */
    10000,           /* Stack size of task */
    NULL,            /* parameter of the task */
    1,               /* priority of the task */
    &WiFiTask1,      /* Task handle to keep track of created task */
    0);              /* pin task to core 0 */

}
unsigned int main_timer = 0;
void loop() {//Run on core task 1
  output_loop();     //Gestisce il pilotaggio degli output
  input_loop();      //Gestisce la lettura di tutti gli input della scheda

  event_io_loop();   //Gestisce gli eventi associati ai vari Input e Output
  flag_trg_loop();   //Gestisce il triggering degli eventi su flag interni del software - solo quelli che possono essere modificati da più operazioni che si svolgono in contemporanea

  loop_allarme();    //Gestisce l'algorittimo di anti intrusione
  bool b = false;
  if (stato_key_allarme == ST_ATTIVA_ALLARME)b = true;
  digitalWrite(LED_ST, b);
  //Serial.println(stato_input2);

}

void loop2(void * pvParameters ) {//Run on core task 0
  for (;;) {
    WiFiLoop();//Gestisce tutti gli eventi wifi e le comunicazioni via internet
    delay(1);
  }
}
