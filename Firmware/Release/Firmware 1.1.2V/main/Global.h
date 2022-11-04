/*
   In  questo file vengono definiti tutti gli stati che sono utilizzati per le varie segnalazioni o lettura degli vari stati
*/
#define SIRENA_ON       LOW
#define SIRENA_OFF      HIGH

#define RIPOSO                  0x00
#define ALLARME                 0x01
#define AUTOESCLUSIONE_RIPOSO   0x03 // Indica lo stato di autoesclusione e che attualmente il sensore è a riposo
#define AUTOESCLUSIONE_ALLARME  0x04 // Indica lo stato di autoesclusione e che attualmente il sensore è in allarme

//offset per l'autoesclusione dei sensori
#define AUTOESCLUSIONE_OFFSET AUTOESCLUSIONE_RIPOSO

#define AUTOESCLUSIONE_SI   0x05
#define AUTOESCLUSIONE_NO   0x06

#define ST_NO_POWER     0x07
#define ST_YES_POWER    0x08
#define ST_UNSET_POWER  0x09

#define ST_ATTIVA_ALLARME      0x0A
#define ST_DISATTIVA_ALLARME   0x0B

//---Tutti gli eventi di flag / trigger ecc. sono globali
bool trg_output1 = false;
bool trg_output2 = false;
bool trg_sirena = false;

bool new_st_output1 = false;
bool new_st_output2 = false;
bool new_st_sirena = SIRENA_OFF;

bool trg_change_key = false;
byte new_st_key = ST_DISATTIVA_ALLARME;

bool trg_power_state = false;
byte new_st_power_state = ST_UNSET_POWER;

byte power_state = ST_UNSET_POWER;

//-------
bool internet_connected = false;

byte stato_key_allarme = ST_ATTIVA_ALLARME; // Di base l'allarme si porta allo stato di attiva
bool stato_allarme = RIPOSO;

byte stato_zona1 = RIPOSO;
byte stato_zona2 = RIPOSO;
byte stato_zona3 = RIPOSO;
byte stato_zona4 = RIPOSO;

byte stato_tamper = RIPOSO;


byte stato_input1 = RIPOSO;
byte stato_input2 = RIPOSO;
