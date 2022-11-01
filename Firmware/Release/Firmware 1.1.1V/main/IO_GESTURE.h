#define DELAY_IO_LOOP 10 // fa un ciclo di lettura ogni 10ms



bool hold_reqst_sirena = SIRENA_OFF;

//Creo dei vettori per gestire i 7 Input
byte sensor_array[7] = {ZONA1, ZONA2, ZONA3, ZONA4, TAMPER, INPUT1, INPUT2};
unsigned long timer_array[7] = {0, 0, 0, 0, 0, 0, 0};
unsigned int debounce_array[7] = {DEBOUNCE_ZONA1, DEBOUNCE_ZONA2, DEBOUNCE_ZONA3, DEBOUNCE_ZONA4, DEBOUNCE_TAMPER, DEBOUNCE_INPUT1, DEBOUNCE_INPUT2};

//Creo un array di puntatori per modificare i valori corrispondenti ai vari stati associati
byte *st_sens[7] = {&stato_zona1, &stato_zona2, &stato_zona3, &stato_zona4, &stato_tamper, &stato_input1, &stato_input2};

//Macchina a stati per la lettura dei sensori delle varie zone e input programmabili
enum st_readSens {
  READSENS_RESET_I,
  READSENS_READ_VALUE,
  READSENS_CHECK_TIME,
  READSENS_WAIT_LOOP
};
byte readSens_st = READSENS_RESET_I;


void input_loop();
void output_loop();
void flag_trg_loop();
void event_io_loop();
void do_input_event(byte st_input, int event_type);
void do_output_event(bool st_output, bool *trg_var, bool *st_var, int event_type);
