void loop_allarme() {




  static unsigned long allarme_timer0 = 0;
  static unsigned long allarme_timer1 = 0;
  static unsigned long allarme_timer2 = 0;

  static unsigned int allarme_counter0 = 0;

  static byte mask_esclusioni = 0; //Maschera esclusioni permanenti per il ciclo dell'allarme
  static byte mask_temp_esclusioni = 0; //Maschera esclusioni temporanee

  static byte hold_allarm_sensor = 0; // Salva i sensori che vanno in allarme nel ciclo della sirena

  static byte map_sensor = 0; // Mappa stato sensori attuale

  String notify_message_allarme = "";

  static bool startup_flag = false;

  if ((stato_key_allarme == ST_DISATTIVA_ALLARME) && (ST_allarme != ST_ALLARME_WAIT_KEY) && (ST_allarme != ST_ALLARME_TAMPER_MANOMESSO)) {
    ST_allarme = ST_ALLARME_RESET_ALL ;//(startup_flag) ? (ST_ALLARME_RESET_ALL) : (ST_ALLARME_WAIT_KEY); // Resetta l'algorittimo dell'allarme se necessario , altrimenti rimane nello stato di ST_ALLARME_WAIT_KEY
  } else if ((stato_key_allarme == ST_ATTIVA_ALLARME) && (ST_allarme == ST_ALLARME_WAIT_KEY || ST_allarme == ST_ALLARME_TAMPER_MANOMESSO)) {
    ST_allarme = ST_ALLARME_INIZIALIZZA; // fa partire l'algorittimo dell'allarme
  }
  // if (ST_allarme != ST_ALLARME_WAIT_KEY) Serial.println(ST_allarme);
  switch (ST_allarme) {
    case ST_ALLARME_WAIT_KEY: //controlla se il tamper va in allarme
      //Aggiorna la maschera dei sensori
      map_allarm_sensor(&map_sensor);
      // isola solo lo stato del tamper
      map_sensor = map_sensor & (BITMASK8(BIT_TAMPER));
      //isola solo lo stato del tamper dalle autoesclusioni
      hold_allarm_sensor = hold_allarm_sensor & (BITMASK8(BIT_TAMPER));
      //Se non viene autoescluso il tamper allora l'allarme suona sempre
      if (AUTOINCLUSIONENEEDED(hold_allarm_sensor)) {
        //attende di autoincludere il tamper
        if (map_sensor == RIPOSO) BITMASKRESET(hold_allarm_sensor); //Resetta l'auto esclusione temporanea del tamper

      } else {
        if (map_sensor != RIPOSO) {
          //va in allarme
          STOP_TIMER(allarme_timer2); // Stoppo il timer2
          STOP_TIMER(allarme_timer0); // stoppo il timer0
          ST_allarme = ST_ALLARME_TAMPER_MANOMESSO; // fa suonare istantaneamente la sirena per tamper manomesso
        }
      }
      break;
    case ST_ALLARME_INIZIALIZZA:
      trg_sirena = true;
      new_st_sirena = SIRENA_OFF;
      //Resetto le autoesclusioni temporanee
      mask_temp_esclusioni = 0;
      //Resetto la mappa delle esclusioni
      mask_esclusioni = 0;
      //Resetto tutti i contatori
      RESET_COUNTER(allarme_counter0); //Resetto il counter 0
      //Resetto tutti i timer
      STOP_TIMER(allarme_timer0); // Resetta i timer
      STOP_TIMER(allarme_timer1); // Resetta i timer
      STOP_TIMER(allarme_timer2); // Resetta i timer

      //Resetto gli stati delle auto esclusioni
      SETAUTOESCLUSIONE(AUTOESCLUSIONE_NO, stato_zona1);
      SETAUTOESCLUSIONE(AUTOESCLUSIONE_NO, stato_zona2);
      SETAUTOESCLUSIONE(AUTOESCLUSIONE_NO, stato_zona3);
      SETAUTOESCLUSIONE(AUTOESCLUSIONE_NO, stato_zona4);

      SETAUTOESCLUSIONE(AUTOESCLUSIONE_NO, stato_tamper);

      BITMASKRESET(hold_allarm_sensor);
      START_TIMER(allarme_timer0); // fa partire il timer per il tempo d'ingresso
      START_TIMER(allarme_timer1); // fa partire il timer per i beep del buzzer

      digitalWrite(BUZZER, LOW); //Mi assicuro che il buzzer sia sullo stato di riposo
      ST_allarme = ST_ALLARME_TEMPO_DI_USCITA;
      break;
    case ST_ALLARME_TEMPO_DI_USCITA:
      if (IS_DELAYED(allarme_timer0, ALLARME_ON_DELAYED)) { // Attende che sia passato il tempo d'ingresso
        digitalWrite(BUZZER, LOW);
        ST_allarme = ST_ALLARME_AUTO_ESCLUSIONI;
      } else { // fa fare un beep del buzzer
        if (IS_DELAYED(allarme_timer1, 350)) {
          digitalWrite(BUZZER, !digitalRead(BUZZER)); //Fa fare il beep del buzzer
          START_TIMER(allarme_timer1); // Resetta il timer per il beep
        }
      }
      break;
    case ST_ALLARME_AUTO_ESCLUSIONI:

      //popola la mappa dei sensori in allarme
      map_allarm_sensor(&map_sensor);
      //mappo temporaneamente la maschera di escludibilità zone
      mask_esclusioni = ZONA1_ESCLUDIBILE << BIT_ZONE1;
      mask_esclusioni = mask_esclusioni | ( ZONA2_ESCLUDIBILE << BIT_ZONE2);
      mask_esclusioni = mask_esclusioni | ( ZONA3_ESCLUDIBILE << BIT_ZONE3);
      mask_esclusioni = mask_esclusioni | ( ZONA4_ESCLUDIBILE << BIT_ZONE4);
      mask_esclusioni = mask_esclusioni | ( TAMPER_ESCLUDIBILE << BIT_TAMPER);
      //Salvo la maschera dei sensori autoescludibili applicando la maschera dei sensori attualmente in allarme
      mask_esclusioni = map_sensor & mask_esclusioni; // Salvo la maschera dei sensori in allamre che possono andare in autoesclusione
      //Attualizzo lo stato dei sensori in modalità auto esclusa
      SETAUTOESCLUSIONE((GETBIT8(mask_esclusioni, BIT_ZONE1)) ? ( AUTOESCLUSIONE_SI) : (AUTOESCLUSIONE_NO), stato_zona1);
      SETAUTOESCLUSIONE((GETBIT8(mask_esclusioni, BIT_ZONE2)) ? ( AUTOESCLUSIONE_SI) : (AUTOESCLUSIONE_NO), stato_zona2);
      SETAUTOESCLUSIONE((GETBIT8(mask_esclusioni, BIT_ZONE3)) ? ( AUTOESCLUSIONE_SI) : (AUTOESCLUSIONE_NO), stato_zona3);
      SETAUTOESCLUSIONE((GETBIT8(mask_esclusioni, BIT_ZONE4)) ? ( AUTOESCLUSIONE_SI) : (AUTOESCLUSIONE_NO), stato_zona4);

      SETAUTOESCLUSIONE((GETBIT8(mask_esclusioni, BIT_TAMPER)) ? ( AUTOESCLUSIONE_SI) : (AUTOESCLUSIONE_NO), stato_tamper);


      if (map_sensor != mask_esclusioni) { //Allarme non attivabile
        ST_allarme = ST_ALLARME_ALLERT_BUZZER1; // Va alla segnalazione allarme non attivabile
      } else {
        ST_allarme = ST_ALLARME_ALLERT_BUZZER2; // Va alla segnalazione allarme attivabile
      }

      RESET_COUNTER(allarme_counter0);
      START_TIMER(allarme_timer1); // Resetta il timer per il beep


      break;
    case ST_ALLARME_ALLERT_BUZZER1://Allarme non attivabile
      //fa 3 beep per segnalare l'errore
      if (allarme_counter0 >= ALLARME_CICLI_BEEP(3)) {
        ST_allarme = ST_ALLARME_RESET_ALL;
        //Disattiva la chiave
        trg_change_key = true;
        new_st_key = ST_DISATTIVA_ALLARME;
        //invio notifica di non attivabilità
        notify_message_allarme = MSG_TG_ALLARME_NON_INSERIBILE;
        notify_message_allarme += "\n" + string_zone_builder();
        send_notify(notify_message_allarme);


      } else if (IS_DELAYED(allarme_timer1, 1000)) {
        digitalWrite(BUZZER, !digitalRead(BUZZER)); //Fa fare il beep del buzzer
        allarme_counter0++;
        START_TIMER(allarme_timer1); // Resetta il timer per il beep
      }
      break;
    case ST_ALLARME_ALLERT_BUZZER2://Allarme attivabile , controlla lo stato delle esclusioni
      if (CHECKBITMASK(mask_esclusioni, BUILDBITMASK8(0, 0, 0, 0, 1, 1, 1, 1))) { // Beep per indicare un sensore escluso
        if (allarme_counter0 == 0 && IS_DELAYED(allarme_timer1, 500)) { // Aspetta 500ms prima di iniziare la sequenza dei beep
          allarme_counter0++;
          digitalWrite(BUZZER, !digitalRead(BUZZER)); //Fa fare il beep del buzzer
          START_TIMER(allarme_timer1); // Resetta il timer per il beep
        } else if (allarme_counter0 <= ALLARME_CICLI_BEEP(2) && IS_DELAYED(allarme_timer1, 250)) {// Fa 2 beep veloci 250ms
          allarme_counter0++;
          digitalWrite(BUZZER, !digitalRead(BUZZER)); //Fa fare il beep del buzzer
          START_TIMER(allarme_timer1); // Resetta il timer per il beep
        } else { // se allarme_counter0>4 allora passa all'allert successivo

          notify_message_allarme = MSG_TG_ALLERT_ZONE_ESCLUSE;
          notify_message_allarme += "\n" + string_zone_builder();
          send_notify(notify_message_allarme);

          RESET_COUNTER(allarme_counter0); //Resetta il counter per contare i beep
          START_TIMER(allarme_timer1); // Resetta il timer per il beep
          ST_allarme = ST_ALLARME_ALLERT_BUZZER3; //Si sposta all'allert successivo
        }
      } else {
        ST_allarme = ST_ALLARME_ALLERT_BUZZER3; //Si sposta all'allert successivo senza segnalare nulla
      }
      break;
    case ST_ALLARME_ALLERT_BUZZER3:

      if (CHECKBITMASK(mask_esclusioni, BITMASK8(BIT_TAMPER))) { //Beep per indicare che il tamper è escluso
        if (allarme_counter0 == 0 && IS_DELAYED(allarme_timer1, 500)) { // Aspetta 500ms prima di iniziare la sequenza dei beep
          allarme_counter0++;
          digitalWrite(BUZZER, !digitalRead(BUZZER)); //Fa fare il beep del buzzer
          START_TIMER(allarme_timer1); // Resetta il timer per il beep
        } else if (allarme_counter0 <= ALLARME_CICLI_BEEP(2) && IS_DELAYED(allarme_timer1, 500)) {// Fa 2 beep lenti da 500ms
          allarme_counter0++;
          digitalWrite(BUZZER, !digitalRead(BUZZER)); //Fa fare il beep del buzzer
          START_TIMER(allarme_timer1); // Resetta il timer per il beep
        } else { // se allarme_counter0>4 allora passa all'allert successivo
          send_notify(MSG_TG_ALLERT_TAMPER_ESCLUSO_ALLARME_ON);
          RESET_COUNTER(allarme_counter0); //Resetta il counter per contare i beep
          START_TIMER(allarme_timer1); // Resetta il timer per il beep
          STOP_TIMER(allarme_timer0);
          ST_allarme = ST_ALLARME_BUILD_TEMP_MASK; //Inizia il ciclo di allarme
        }
      } else {
        send_notify(MSG_TG_ALLARME_INSERITA);

        STOP_TIMER(allarme_timer0);
        ST_allarme = ST_ALLARME_BUILD_TEMP_MASK; //Inizia il ciclo di allarme
      }
      break;
    case ST_ALLARME_BUILD_TEMP_MASK:
      //Aggiorno la mappa dei sensori in allarme
      map_allarm_sensor(&map_sensor);
      if (AUTOINCLUSIONENEEDED(hold_allarm_sensor)) {//esegue il codice che controlla se deve autoincludere un sensore che torna a riposo
        //In questa fase controlla che i sensori segnalati in hold_allarm_sensor vengano trasferiti nella mappa delle esclusioni temporanee
        mask_temp_esclusioni = mask_temp_esclusioni | hold_allarm_sensor; // somma la vecchia maschera temporanea delle esclusioni aggiungendo i nuovi sensori da autoescludere
        BITMASKRESET(hold_allarm_sensor);//resetta la bitmap dei sensori andati in allarme durante il ciclo di Allarme
      }
      //Aggiorniamo la lettura dei sensori rimuovendo quelli autoesclusi permanenti
      map_sensor = map_sensor & (~mask_esclusioni);
      //Aggiorno la maschera delle esclusioni temporanee
      mask_temp_esclusioni = mask_temp_esclusioni & map_sensor;


      //Una volta aggiornata le maschere di esclusione sensori , passo al controllo anti-intrusione
      ST_allarme = ST_ALLARME_CHECK_SENSOR;
      START_TIMER(allarme_timer1);
      break;
    case ST_ALLARME_CHECK_SENSOR:
      if (IS_DELAYED(allarme_timer1, ALLARME_CHECK_DELAY)) {
        //Aggiorna la mappa dei sensori in allarme
        map_allarm_sensor(&map_sensor);
        //Aggiorniamo la lettura dei sensori rimuovendo quelli autoesclusi permanenti
        map_sensor = map_sensor & (~(mask_esclusioni | mask_temp_esclusioni));
        //Setta il prossimo stato della macchina cosi da creare un loop di controllo dei sensori
        ST_allarme = ST_ALLARME_BUILD_TEMP_MASK;

        if (CHECKBITMASK(map_sensor, BITMASK8(BIT_TAMPER))) { // se va in allarme per il tamper
          // faccio suonare direttamente la sirena
          STOP_TIMER(allarme_timer2); // Stoppo il timer2
          STOP_TIMER(allarme_timer0); // stoppo il timer0
          ST_allarme = ST_ALLARME_CYCLE_SIRENA_ON; // si sposta allo stato per far suonare la sirena
        } else  if (map_sensor != RIPOSO) { // Se un sensore va in allarme si sposta alla procedura di segnalazione
          hold_allarm_sensor = map_sensor; //Copio i valori dei sensori che sono andati in allarme
          if (!ISALIVE_TIMER(allarme_timer0))START_TIMER(allarme_timer0); // fa partire il timer per il tempo d'ingresso
          STOP_TIMER(allarme_timer1);

          //Setto il prossimo stato della macchina in tempo d'ingresso
          ST_allarme = ST_ALLARME_TEMPO_DI_INGRESSO;
        }
      }
      break;
    case ST_ALLARME_TEMPO_DI_INGRESSO:
      //Gestisce il beep del tempo d'ingresso
      if (IS_DELAYED(allarme_timer2, 350)) {
        digitalWrite(BUZZER, !digitalRead(BUZZER));
        START_TIMER(allarme_timer2);
      } else if (!ISALIVE_TIMER(allarme_timer2)) {
        START_TIMER(allarme_timer2);
        digitalWrite(BUZZER, HIGH);
      }
      if (IS_DELAYED(allarme_timer0, SIRENA_ON_DELAYED)) {
        //chiude il buzzer
        digitalWrite(BUZZER, LOW);
        STOP_TIMER(allarme_timer2); // Stoppo il timer2
        STOP_TIMER(allarme_timer0); // stoppo il timer0
        ST_allarme = ST_ALLARME_CYCLE_SIRENA_ON; // fa suonare la sirena
      } else {
        ST_allarme = ST_ALLARME_HOLD_SENSOR; //controllo gli eventuali sensori in allarme
      }
      break;
    case ST_ALLARME_HOLD_SENSOR:
      //Aggiorna la mappa dei sensori in allarme
      map_allarm_sensor(&map_sensor);
      hold_allarm_sensor = hold_allarm_sensor | map_sensor; //mantengo in memoria se nuovi sensori vanno in allarme
      if (ISALIVE_TIMER(allarme_timer0)) {
        ST_allarme = ST_ALLARME_TEMPO_DI_INGRESSO; // se l'hold sensor viene richiamato da TEMPO_DI_INGRESSO allora ritorna a quello stato
      } else {
        ST_allarme = ST_ALLARME_CYCLE_SIRENA_ON; // se l'hold sensor viene richiamato da CYCLE_SIRENA_ON allora ritorna a quello stato
      }
      break;
    case ST_ALLARME_CYCLE_SIRENA_ON:

      if (!ISALIVE_TIMER(allarme_timer1)) { // Se la sirena è spenta ma non è partito il timer allora fa partire la sirena
        trg_sirena = true;
        new_st_sirena = SIRENA_ON;
        //notifico che c'è stato un'evento d'allarme
        notify_message_allarme = MSG_TG_ALLERT_INTRUSIONE_RILEVATA;
        notify_message_allarme+="\n";
        //Applica la maschera di Esclusione dei sensori cosi da inviare una segnalazione sulle zone realemnte in allarme
        if (GETBIT8(hold_allarm_sensor & (~(mask_temp_esclusioni | mask_esclusioni)), BIT_ZONE1)) {
          notify_message_allarme += MSG_TG_ALLARME_ZONA1 ;
           notify_message_allarme +"\n";
        }
        if (GETBIT8(hold_allarm_sensor & (~(mask_temp_esclusioni | mask_esclusioni)), BIT_ZONE2)){
          notify_message_allarme +=  MSG_TG_ALLARME_ZONA2;
          notify_message_allarme +"\n";
        }
        if (GETBIT8(hold_allarm_sensor & (~(mask_temp_esclusioni | mask_esclusioni)), BIT_ZONE3)){
          notify_message_allarme +=  MSG_TG_ALLARME_ZONA3;
          notify_message_allarme +"\n";
        }
        if (GETBIT8(hold_allarm_sensor & (~(mask_temp_esclusioni | mask_esclusioni)), BIT_ZONE4)){
          notify_message_allarme +=  MSG_TG_ALLARME_ZONA4;
          notify_message_allarme +"\n";
        }
        if (GETBIT8(hold_allarm_sensor & (~(mask_temp_esclusioni | mask_esclusioni)), BIT_TAMPER)){
          notify_message_allarme +"\n";
          notify_message_allarme +=  MSG_TG_ALLARME_TAMPER ;
          notify_message_allarme +"\n";
        }

        send_notify(notify_message_allarme);
      
        START_TIMER(allarme_timer1); // fa partire il timer per il tempo di ciclo sirena

      } else if (IS_DELAYED(allarme_timer1, CYCLE_TIMER_SIRENA_ON)) { // se la sirena va in timeout per il tempo massimo di ciclo allarme allora si spegne
        trg_sirena = true;
        new_st_sirena = SIRENA_OFF; // quando il relè si eccita la sirena si spegne.
        STOP_TIMER(allarme_timer1);//Stoppa il timer usato per il ciclo sirena
        //Il timer2 per il tempo massimo di accensione sirena viene stoppato solo in BUILD_TEMP_MASK se i sensori che sono andati in allarme tornano a riposo
        ST_allarme = ST_ALLARME_BUILD_TEMP_MASK; //controlla se deve fare le maschere di autoesclusione e ricomincia il ciclo per rilevare nuovamente altre intrusioni
      } else {
        ST_allarme = ST_ALLARME_HOLD_SENSOR; // ritorna in fase di hold sensor
      }
      //Se la macchina sta per uscire da questo stato e va in fase BUILD_TEMP_MASK, allora segnala che è suonata l'allarme
      if (ST_allarme == ST_ALLARME_BUILD_TEMP_MASK) send_telegram_message("-Fine Ciclo Sirena-"); // invia una notifica di quali sensori sono andati in allarme
      break;
    case ST_ALLARME_TAMPER_MANOMESSO:

      if (!ISALIVE_TIMER(allarme_timer1)) { // Se la sirena è spenta ma è partito il timer allora fa partire la sirena
        trg_sirena = true;
        new_st_sirena = SIRENA_ON;
        //notifico che c'è stato un'evento d'allarme

        send_notify(MSG_TG_ALLARME_TAMPER);

        START_TIMER(allarme_timer1); // fa partire il timer per il tempo di ciclo sirena

      } else if (IS_DELAYED(allarme_timer1, CYCLE_TIMER_SIRENA_ON)) { // se la sirena va in timeout per il tempo massimo di ciclo allarme allora si spegne
        trg_sirena = true;
        new_st_sirena = SIRENA_OFF;
        STOP_TIMER(allarme_timer1);//Stoppa il timer usato per il ciclo sirena
        //Il timer2 per il tempo massimo di accensione sirena viene stoppato solo in BUILD_TEMP_MASK se i sensori che sono andati in allarme tornano a riposo
        ST_allarme = ST_ALLARME_RESET_ALL; //controlla se deve fare le maschere di autoesclusione e ricomincia il ciclo per rilevare nuovamente altre intrusioni
      } else {
        //Mantiene in memoria se il sensore è in allarme per tutto il ciclo della sirena
        map_allarm_sensor(&map_sensor);
        hold_allarm_sensor = hold_allarm_sensor | map_sensor;
      }
      //Se la macchina sta per uscire da questo stato e va in fase BUILD_TEMP_MASK, allora segnala che è suonata l'allarme
      if (ST_allarme == ST_ALLARME_RESET_ALL) send_telegram_message("-Fine Ciclo Sirena-"); // invia una notifica di quali sensori sono andati in allarme

      break;
    case ST_ALLARME_RESET_ALL:

      trg_sirena = true;
      new_st_sirena = SIRENA_OFF; // quando il relè si eccita la sirena si spegne.

      digitalWrite(BUZZER, LOW); //Mi assicuro che il buzzer sia sullo stato di riposo
      //Resetto le autoesclusioni temporanee
      mask_temp_esclusioni = 0;
      //Resetto la mappa delle esclusioni
      mask_esclusioni = 0;
      //Resetto tutti i contatori
      RESET_COUNTER(allarme_counter0); //Resetto il counter 0
      //Resetto tutti i timer
      STOP_TIMER(allarme_timer0); // Resetta i timer
      STOP_TIMER(allarme_timer1); // Resetta i timer
      STOP_TIMER(allarme_timer2); // Resetta i timer

      //Resetto gli stati delle auto esclusioni
      SETAUTOESCLUSIONE(AUTOESCLUSIONE_NO, stato_zona1);
      SETAUTOESCLUSIONE(AUTOESCLUSIONE_NO, stato_zona2);
      SETAUTOESCLUSIONE(AUTOESCLUSIONE_NO, stato_zona3);
      SETAUTOESCLUSIONE(AUTOESCLUSIONE_NO, stato_zona4);

      SETAUTOESCLUSIONE(AUTOESCLUSIONE_NO, stato_tamper);

      BITMASKRESET(hold_allarm_sensor);
      send_notify(MSG_TG_ALLARME_DISINSERITA);
      ST_allarme = ST_ALLARME_WAIT_KEY;
      break;
  }
}

void map_allarm_sensor(byte * map_var) {
  //Mappo lo stato dei sensori
  *map_var = (stato_zona1 == ALLARME || stato_zona1 == AUTOESCLUSIONE_ALLARME ) << BIT_ZONE1;
  *map_var = *map_var |  ((stato_zona2 == ALLARME || stato_zona2 == AUTOESCLUSIONE_ALLARME) << BIT_ZONE2);
  *map_var = *map_var |  ((stato_zona3 == ALLARME || stato_zona3 == AUTOESCLUSIONE_ALLARME) << BIT_ZONE3);
  *map_var = *map_var |  ((stato_zona4 == ALLARME || stato_zona4 == AUTOESCLUSIONE_ALLARME) << BIT_ZONE4);
  *map_var = *map_var |  ((stato_tamper == ALLARME || stato_tamper == AUTOESCLUSIONE_ALLARME) << BIT_TAMPER);

}
String string_zone_builder() {


  String notify_builder1 = "";
  String notify_builder2 = "";

  notify_builder2 = ((stato_zona1   == RIPOSO) ? MSG_TG_RIPOSO_ZONA1 : MSG_TG_ALLARME_ZONA1);
  notify_builder1 +=  notify_builder2 + " Escludibile : ";
  notify_builder2 = ((ZONA1_ESCLUDIBILE   == true) ? "SI  " : "NO ");
  notify_builder1 += notify_builder2 + "\n";

  notify_builder2 = ((stato_zona2   == RIPOSO) ? MSG_TG_RIPOSO_ZONA2 : MSG_TG_ALLARME_ZONA2);
  notify_builder1 += notify_builder2 + " Escludibile : ";
  notify_builder2 = ((ZONA2_ESCLUDIBILE   == true) ? "SI  " : "NO ");
  notify_builder1 += notify_builder2 + "\n";



  notify_builder2 = ((stato_zona3   == RIPOSO) ? MSG_TG_RIPOSO_ZONA3 : MSG_TG_ALLARME_ZONA3);
  notify_builder1 +=  notify_builder2 + " Escludibile : ";
  notify_builder2 = ((ZONA2_ESCLUDIBILE   == true) ? "SI  " : "NO ");
  notify_builder1 += notify_builder2 + "\n";




  notify_builder2 = ((stato_zona4   == RIPOSO) ? MSG_TG_RIPOSO_ZONA4 : MSG_TG_ALLARME_ZONA4);
  notify_builder1 +=  notify_builder2 + " Escludibile : ";
  notify_builder2 = ((ZONA4_ESCLUDIBILE   == true) ? "SI  " : "NO ");
  notify_builder1 += notify_builder2 + "\n";



  notify_builder2 = ((stato_tamper   == RIPOSO) ? MSG_TG_RIPOSO_TAMPER :MSG_TG_ALLARME_TAMPER);
  notify_builder1 +=  notify_builder2 + " Escludibile : ";
  notify_builder2 = ((TAMPER_ESCLUDIBILE   == true) ? "SI  " : "NO ");
  notify_builder1 += notify_builder2 + "\n";
  return notify_builder1;
}
