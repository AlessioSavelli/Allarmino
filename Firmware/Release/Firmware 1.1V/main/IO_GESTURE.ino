/*
   Questo file contiene tutte le funzioni, variabili e macchine a stati che vengono utilizzate per gli algorittimi di gestione Input , Output e trigger dei flag
*/
void input_loop() {

  static unsigned long io_timer0 = 0;

  //Read stato sensori , anti tamper e input
  static unsigned int counter_i = 0;//indice per il ciclo di lettura inpu della macchina a stati
  switch (readSens_st) {
    case READSENS_RESET_I:
      RESET_COUNTER(counter_i);
      readSens_st = READSENS_READ_VALUE;
      break;
    case READSENS_READ_VALUE://Controlla se qualche sensore viene triggerato
      if ( !ISALIVE_TIMER(timer_array[counter_i])) {//Se il timer non è attivo allora controlla il pin
        if (digitalRead(sensor_array[counter_i]) && (ISSENSORRIPOSO(*st_sens[counter_i]))) { // See il pin passa da Riposo ad HIGH triggera il filtro per poi modificare lo stato
          START_TIMER(timer_array[counter_i]); // Triggera l'evento dell'input
        }
      }
      counter_i++;
      if (counter_i >= 7) {
        counter_i = 0;
        readSens_st = READSENS_CHECK_TIME;
      }
      break;
    case READSENS_CHECK_TIME: // controlla se gli input triggerati rimangano in allarme per il tempo minimo definito di debounce
      if (IS_DELAYED(timer_array[counter_i], debounce_array[counter_i])) {
        STOP_TIMER(timer_array[counter_i]);//Resetta il timer di debounce
        SETSENSORSTATUS(ALLARME, *st_sens[counter_i]); // mette il flag allarme sull'input, se dopo il tempo di debounce è ancora in allarme
      } else if (!digitalRead(sensor_array[counter_i])) { // Se il pin torna a riposo resetta il timer di debounce
        STOP_TIMER(timer_array[counter_i]);
        SETSENSORSTATUS(RIPOSO, *st_sens[counter_i]);
      }


      counter_i++;
      if (counter_i >= 7) {
        counter_i = 0;
        START_TIMER(io_timer0);
        readSens_st = READSENS_WAIT_LOOP;
      }
      break;
    case READSENS_WAIT_LOOP: // Attende del tempo prima di ricominciare il ciclo di lettura input
      if (IS_DELAYED(io_timer0, DELAY_IO_LOOP)) readSens_st = READSENS_RESET_I;
      break;
  }

}
void flag_trg_loop() { // questo void gestisce tutti i triggering dei flag da parte di input multipli associando gli stati corretti in base a come vengono effettuate le richieste
  if (trg_change_key) { //Gestisce il flag di attivazione
    trg_change_key = false;
    stato_key_allarme = new_st_key; //Attiva e disattiva l'allarme
  }
  if (trg_power_state) {
    trg_power_state = false;
    power_state = new_st_power_state; //Indica lo stato dell'alimentazione
    if (power_state == ST_NO_POWER) {
      send_notify(MSG_TG_MANCANZA_POWERLINE);
    } else if (power_state == ST_YES_POWER) {
      send_notify(MSG_TG_RIPRISTINO_POWERLINE);
    }
  }

}
void output_loop() { // questo void gestisce tutte le richiesta di pilotaggio output in base a come vengono effettuate le richieste
  static unsigned long timer_timeout_sirena = 0;
  static unsigned long timer_debounce_sirena = 0;

  if (trg_output1) {//Gestisce l'output1
    trg_output1 = false;
    digitalWrite(OUTPUT1, new_st_output1);
    //Notifica la variazione di stato via telegram se quest'ultimo è abilitato
    if (OUTPUT1_AS_EVENT == OUTPUT_AS_TELEGRAM) {
      (new_st_output1) ? (send_notify(MSG_TG_OUTPUT1_ON,true)) : (send_notify(MSG_TG_OUTPUT1_OFF,true));
    }
  }
  if (trg_output2) {//Gestisce l'output2
    trg_output2 = false;
    digitalWrite(OUTPUT2, new_st_output2);
    //Notifica la variazione di stato via telegram se quest'ultimo è abilitato
    if (OUTPUT2_AS_EVENT == OUTPUT_AS_TELEGRAM) {
      (new_st_output2) ? (send_notify(MSG_TG_OUTPUT2_ON,true)) : (send_notify(MSG_TG_OUTPUT2_OFF,true));
    }
  }
  if (IS_DELAYED(timer_timeout_sirena, MAX_TIME_SIRENA_ON)) {
    trg_sirena = true;
    if (new_st_sirena == SIRENA_OFF)STOP_TIMER(timer_timeout_sirena); // Forza il pilotaggio della sirena ad OFF fin tanto che non riamane veramente spenta per almeno un intero ciclo di main
    new_st_sirena = SIRENA_OFF;
  }
  if (trg_sirena) {//Gestisce l'output della sirena
    if (!ISALIVE_TIMER(timer_debounce_sirena)) {
      START_TIMER(timer_debounce_sirena);
      hold_reqst_sirena = new_st_sirena;
    } else if (IS_DELAYED(timer_debounce_sirena, 25) && new_st_sirena == hold_reqst_sirena ) {
      trg_sirena = false;
      STOP_TIMER(timer_debounce_sirena);
      digitalWrite(SIRENA, new_st_sirena);
      //disattiva il timeout se viene spenta la sirena
      if ((new_st_sirena == SIRENA_OFF) && (ISALIVE_TIMER(timer_timeout_sirena)))STOP_TIMER(timer_timeout_sirena);
      //attiva il timeout se viene accesa la sirena
      if ((new_st_sirena == SIRENA_ON) && (!ISALIVE_TIMER(timer_timeout_sirena)))START_TIMER(timer_timeout_sirena);
    } else if (new_st_sirena != hold_reqst_sirena) {
      hold_reqst_sirena = new_st_sirena;
      START_TIMER(timer_debounce_sirena);
    }
  }
}


void event_io_loop() {

  //variabili per gestire gli input solo quando cambiano stato
  static bool old_stato_input1 = RIPOSO;
  static bool old_stato_input2 = RIPOSO;


  //gli input triggerano gli eventi solo quando cambia il fronte
  if (old_stato_input1 != stato_input1) {
    do_input_event(stato_input1, INPUT1_AS_EVENT ); // associa gli input agli eventi
    old_stato_input1 = stato_input1;
  }
  //Serial.println(stato_input2);
  if (old_stato_input2 != stato_input2) {

    do_input_event(stato_input2, INPUT2_AS_EVENT ); // associa gli input agli eventi
    old_stato_input2 = stato_input2;
  }

  do_output_event(digitalRead(OUTPUT1), &trg_output1, &new_st_output1, OUTPUT1_AS_EVENT); // associa gli output agli eventi
  do_output_event(digitalRead(OUTPUT2), &trg_output2, &new_st_output2, OUTPUT2_AS_EVENT); // associa gli output agli eventi
}

void do_input_event(byte st_input, int event_type) {
  switch (event_type) {
    case INPUT_AS_NONE :
      break;
    case INPUT_AS_KEY:
      //triggera l'evento che gestisce l'attivazione e disattivazione dell'allarme
      trg_change_key = true;
      new_st_key = (st_input == ALLARME) ? (ST_ATTIVA_ALLARME) : (ST_DISATTIVA_ALLARME);
      break;
    case INPUT_AS_POWER_CHECK:
      trg_power_state = true;
      new_st_power_state = (st_input == RIPOSO) ? (ST_YES_POWER) : (ST_NO_POWER); //Indica lo stato dell'alimentazione

      break;
  }

}
void do_output_event(bool st_output, bool * trg_var, bool * st_var, int event_type) {
  switch (event_type) {
    case OUTPUT_AS_ST_ALLARM:

      if (st_output != ISALLARMEON) {// se lo stato di allarme non coincide con l'output1 allora triggera l'evento di allinare i valori di stato
        *st_var = ISALLARMEON; // indica lo stato a cui deve essere portato l'output
      }

      break;
    case OUTPUT_AS_NO_INTERNET:
      if (st_output != (!internet_connected)) { // Siccome il flag sta a true quando è connesso e lo stato dell'output funziona al contrario , controllo che sia diverso dal valore negato del flag
        if (millis() <= (10 * SECONDS)) return; // l'evento viene inibito per i primi 10s dopo l'avvio dell'esp per evitare false segnalazioni in fase di startup
        *st_var = !internet_connected; // indica lo stato a cui deve essere portato l'output
      }
      break;
    case OUTPUT_AS_NO_POWER:
      if (power_state == ST_UNSET_POWER) return; // Se l'evento non è stato configurate su un'input allora non esegue nulla
      if (power_state == ST_NO_POWER) {
        *st_var = true;
      } else if (power_state == ST_YES_POWER) {
        *st_var = false;
      }
      break;
    case OUTPUT_AS_TAMPER_ESCLUSO:

      if (ISSENSORESCLUSO(stato_tamper)) { // Se il tamper si è auto escluso piloto l'output per segnalare l'evento
        *st_var = true;
      } else {
        *st_var = false;
      }
      break;
    case OUTPUT_AS_TAMPER_ALLARME:
      if ( stato_tamper == ALLARME || stato_tamper == AUTOESCLUSIONE_ALLARME) { //piloto in tempo reale l'evento di tamper manomesso
        //*trg_var = true;//triggera l'evento
        *st_var = true;
      } else {
        //*trg_var = true;//triggera l'evento
        *st_var = false;
      }

      break;
    case OUTPUT_AS_TELEGRAM:
      //N.B la notifica di telegram viene gestita del trigger degli I/O

      break;
  }
  if (*st_var != st_output) { // Se i valori di output e di flag sono incongruenti allinea i dati
    *trg_var = true;//triggera l'evento
  }
}
