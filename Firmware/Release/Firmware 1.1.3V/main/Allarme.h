/*
   Questo file contiene tutte le funzioni, variabili e macchine a stati che vengono utilizzati per l'algorittimo d'Allarme
*/



//----Definisco una macro per capire lo stato dell'allarme----
#define ISALLARMEON   (((ST_allarme>=ST_ALLARME_BUILD_TEMP_MASK) && (ST_allarme<ST_ALLARME_TAMPER_MANOMESSO)))
#define ISALLARMEOFF  (((stato_allarme<=ST_ALLARME_WAIT_KEY) || (ST_allarme>=ST_ALLARME_TAMPER_MANOMESSO)))
//----
//----Definisco delle macro per gestire lo stato dei sensori----
#define ISSENSORESCLUSO(sensor)         ((sensor==AUTOESCLUSIONE_RIPOSO)||(sensor==AUTOESCLUSIONE_ALLARME))
#define SETSENSORSTATUS(st,sensor)      ((ISSENSORESCLUSO(sensor))?(sensor=(st==RIPOSO)?(AUTOESCLUSIONE_RIPOSO):(AUTOESCLUSIONE_ALLARME)):(sensor=st))
#define SETAUTOESCLUSIONE(st,sensor)    (st == AUTOESCLUSIONE_SI)?(ADDESCLUSIONEOFFSET(sensor)):(REMOVEESCLUSIONEOFFSET(sensor))
#define ADDESCLUSIONEOFFSET(sensor)     ((!ISSENSORESCLUSO(sensor))?(sensor+=AUTOESCLUSIONE_OFFSET):(false))
#define REMOVEESCLUSIONEOFFSET(sensor)  ((ISSENSORESCLUSO(sensor))?(sensor-=AUTOESCLUSIONE_OFFSET):(false))
#define ISSENSORRIPOSO(sensor)          ((sensor==RIPOSO)||(sensor==AUTOESCLUSIONE_RIPOSO))
#define AUTOINCLUSIONENEEDED(mask)      (mask!=0x00)
//----



#define ALLARME_CICLI_BEEP(value) (value*2-1)
#define ALLARME_CHECK_DELAY       20 // controlla i sensori ogni 20ms
//definisce la posizione dei BIT per le varie zone
enum bit_zones {
  BIT_ZONE1, BIT_ZONE2, BIT_ZONE3, BIT_ZONE4,
  BIT_TAMPER
};
//Macchina a stati per la gestione dell'allarme
enum st_allarme {
  ST_ALLARME_WAIT_KEY,
  ST_ALLARME_INIZIALIZZA,                     //Inizializza lo stato base dell'algorittimo di allarme
  ST_ALLARME_TEMPO_DI_USCITA,                 //Attende prima di inserire l'allarme, nel frattempo fa un beep del buzzer
  ST_ALLARME_AUTO_ESCLUSIONI,                 //Controlla se ci sono zone in allarme e le esclude se sono escludibili
  ST_ALLARME_ALLERT_BUZZER1, ST_ALLARME_ALLERT_BUZZER2, ST_ALLARME_ALLERT_BUZZER3, // Avvisa delle auto esclusioni tramite il buzzer on board
  ST_ALLARME_BUILD_TEMP_MASK, ST_ALLARME_CHECK_SENSOR,   // Crea una maschera temporanea di esclusione e controlla i sensori se sono in allarme
  ST_ALLARME_TEMPO_DI_INGRESSO, ST_ALLARME_HOLD_SENSOR,  // Salva i sensori che vanno in allarme durante il tempo d'ingresso
  ST_ALLARME_CYCLE_SIRENA_ON,                 //Fa suonare la sirena per il tempo impostato
  ST_ALLARME_TAMPER_MANOMESSO,                //Fa suonare la sirena per il tempo impostato solo in caso di tamper manomesso
  ST_ALLARME_RESET_ALL
};
byte ST_allarme = ST_ALLARME_WAIT_KEY;
/*

*/

void loop_allarme();

void map_allarm_sensor(byte *map_var);

String string_zone_builder();
