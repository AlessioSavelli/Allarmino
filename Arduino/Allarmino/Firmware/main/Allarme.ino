void loop_allarme() {




  static unsigned long allarme_timer0 = 0;
  static unsigned long allarme_timer1 = 0;

  static unsigned int allarme_counter0 = 0;

  static byte mask_esclusioni = 0; //Maschera esclusioni permanenti per il ciclo dell'allarme
  static byte mask_temp_esclusioni = 0; //Maschera esclusioni temporanee

  static byte hold_allarm_sensor = 0; // Salva i sensori che vanno in allarme nel ciclo della sirena

  static byte map_sensor = 0; // Mappa stato sensori attuale

  String notify_message_allarme = "";


  if (stato_key_allarme == ST_DISATTIVA_ALLARME && ST_allarme != ST_ALLARME_WAIT_KEY) {
    ST_allarme = ST_ALLARME_RESET_ALL; // Resetta l'algorittimo dell'allarme
  } else if (stato_key_allarme == ST_ATTIVA_ALLARME && ST_allarme == ST_ALLARME_WAIT_KEY) {
    ST_allarme = ST_ALLARME_TEMPO_DI_USCITA; // fa partire l'algorittimo dell'allarme
    START_TIMER(allarme_timer0); // fa partire il timer per il tempo d'ingresso
    START_TIMER(allarme_timer1); // fa partire il timer per il beep del buzzer
  }
  if (ST_allarme != ST_ALLARME_WAIT_KEY) Serial.println(ST_allarme);
  switch (ST_allarme) {
    case ST_ALLARME_WAIT_KEY:
      stato_allarme == RIPOSO;
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
      mask_esclusioni = ZONA1_ESCLUDIBILE;
      mask_esclusioni = mask_esclusioni | ( ZONA2_ESCLUDIBILE << 1);
      mask_esclusioni = mask_esclusioni | ( ZONA3_ESCLUDIBILE << 2);
      mask_esclusioni = mask_esclusioni | ( ZONA4_ESCLUDIBILE << 3);
      mask_esclusioni = mask_esclusioni | ( TAMPER_ESCLUDIBILE << 4);
      //Salvo la maschera dei sensori autoescludibili applicando la maschera temporanea alla mappa dei sensori
      mask_esclusioni = map_sensor & mask_esclusioni; // Salvo la maschera dei sensori in allamre che possono andare in autoesclusione
      //Metto gli stati dei sensori in modalità esclusa
      if (GETBIT8(mask_esclusioni, BIT_ZONE1))stato_zona1 = AUTOESCLUSIONE; // indica che la zona è esclusa. il valore binario è la maschera di controllo esclusione di zona
      if (GETBIT8(mask_esclusioni, BIT_ZONE2))stato_zona2 = AUTOESCLUSIONE; // indica che la zona è esclusa. il valore binario è la maschera di controllo esclusione di zona
      if (GETBIT8(mask_esclusioni, BIT_ZONE3))stato_zona3 = AUTOESCLUSIONE; // indica che la zona è esclusa. il valore binario è la maschera di controllo esclusione di zona
      if (GETBIT8(mask_esclusioni, BIT_ZONE4))stato_zona4 = AUTOESCLUSIONE; // indica che la zona è esclusa. il valore binario è la maschera di controllo esclusione di zona

      if (GETBIT8(mask_esclusioni, BIT_TAMPER))stato_tamper = AUTOESCLUSIONE;


      if (map_sensor != mask_esclusioni) { //Allarme non attivabile
        ST_allarme = ST_ALLARME_ALLERT_BUZZER1; // Va alla segnalazione allarme non attivabile
      } else {
        ST_allarme = ST_ALLARME_ALLERT_BUZZER2; // Va alla segnalazione allarme attivabile
      }
     /* Serial.println("----CHECK----");
      Serial.print("MASK ESCLUSIONI : ");
      Serial.println(mask_esclusioni, HEX);
      Serial.print("MAPPA SENSORI : ");
      Serial.println(map_sensor, HEX);
      Serial.println("-----------");*/
      RESET_COUNTER(allarme_counter0); //Resetto il counter per i beep
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
        notify_message_allarme = "!! Allarme non attivabile !!.\n";
        notify_message_allarme += string_zone_builder();
        notify_message_allarme += "ALLARME NON ATTIVA";

        send_telegram_message(notify_message_allarme);
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
        } else { // se allarme_counter0>4 allora passa all'allarte successivo

          notify_message_allarme = "!! Alcune zone escluse !!.\n";
          notify_message_allarme += string_zone_builder();
          send_telegram_message(notify_message_allarme);

          RESET_COUNTER(allarme_counter0); //Resetta il counter per contare i beep
          START_TIMER(allarme_timer1); // Resetta il timer per il beep
          ST_allarme = ST_ALLARME_ALLERT_BUZZER3; //Si sposta all'allert successivo
        }
      } else {
        ST_allarme = ST_ALLARME_ALLERT_BUZZER3; //Si sposta all'allert successivo senza segnalare nulla
      }
      break;
    case ST_ALLARME_ALLERT_BUZZER3:

      if (CHECKBITMASK(mask_esclusioni, BUILDBITMASK8(0, 0, 0, 1, 0, 0, 0, 0))) { //Beep per indicare che il tamper è escluso
        if (allarme_counter0 == 0 && IS_DELAYED(allarme_timer1, 500)) { // Aspetta 500ms prima di iniziare la sequenza dei beep
          allarme_counter0++;
          digitalWrite(BUZZER, !digitalRead(BUZZER)); //Fa fare il beep del buzzer
          START_TIMER(allarme_timer1); // Resetta il timer per il beep
        } else if (allarme_counter0 <= ALLARME_CICLI_BEEP(2) && IS_DELAYED(allarme_timer1, 500)) {// Fa 2 beep lenti da 500ms
          allarme_counter0++;
          digitalWrite(BUZZER, !digitalRead(BUZZER)); //Fa fare il beep del buzzer
          START_TIMER(allarme_timer1); // Resetta il timer per il beep
        } else { // se allarme_counter0>4 allora passa all'allarte successivo
          notify_message_allarme = "!! Tamper Escluso !!.\nALLARME ATTIVO";
          send_telegram_message(notify_message_allarme);

          RESET_COUNTER(allarme_counter0); //Resetta il counter per contare i beep
          START_TIMER(allarme_timer1); // Resetta il timer per il beep
          STOP_TIMER(allarme_timer0);
          ST_allarme = ST_ALLARME_BUILD_TEMP_MASK; //Inizia il ciclo di allarme
        }
      } else {
        send_telegram_message("ALLARME ATTIVATO CON SUCCESSO");
        STOP_TIMER(allarme_timer0);
        ST_allarme = ST_ALLARME_BUILD_TEMP_MASK; //Inizia il ciclo di allarme
      }
      break;
    case ST_ALLARME_BUILD_TEMP_MASK:
      //Serial.println("----BUILD TEMP MASK----");
      //popola la mappa dei sensori in allarme
      map_allarm_sensor(&map_sensor);
      //controlla se deve eseguire il codice che identifica le autoesclusioni temporanee dopo il ciclo di allarme
      if (hold_allarm_sensor != 0) {
        //Filtriamo i valori delle autoesclusioni dalla mappa sensori
        map_sensor = map_sensor & (!mask_esclusioni | !mask_temp_esclusioni);
        //ora in map_sensor ci sono solo i sensori in allarme non esclusi
        //Facciamo la stessa operazione per i valori in hold_allarm_sensor
        hold_allarm_sensor = hold_allarm_sensor & (!mask_esclusioni | !mask_temp_esclusioni);
        //ora confrontiamo le maschere e ti riamo fuori solo i sensori che non sono tornati a riposo dopo il ciclo di allarme
        hold_allarm_sensor = map_sensor & hold_allarm_sensor; // evidenziamo solo i valori uguali
        //Ora hold_allarm_sensor ha in memoria i valori dei sensori che non sono tornati a riposo
        //Esclude i sensori ancora in allarme e autoinclude quelli tornati a riposo
        mask_temp_esclusioni = hold_allarm_sensor; // Identifica la nuova maschera temporanea delle esclusioni
        //Serial.print("HOLD MAPPA SENSORI : ");
       // Serial.println(hold_allarm_sensor, HEX);
        hold_allarm_sensor = 0; //Resetta l'hold allarm sensor
      } else {//esegue il codice che controlla se deve autoincludere un sensore che torna a riposo
        //Filtriamo i valori delle autoesclusioni dalla mappa sensori
        //map_sensor = map_sensor & (!mask_esclusioni);
        //mask_temp_esclusioni = map_sensor & (!mask_temp_esclusioni); // identifica le nuove autoesclusioni e rinclusioni
        mask_temp_esclusioni = map_sensor & (!mask_esclusioni) & (!mask_temp_esclusioni);

      }
/*
      Serial.print("MASK ESCLUSIONI : ");
      Serial.println(mask_esclusioni, HEX);
      Serial.print("MAPPA SENSORI : ");
      Serial.println(map_sensor, HEX);
      Serial.print("MASK ESCLUSIONI TEMP : ");
      Serial.println(mask_temp_esclusioni, HEX);

      Serial.println("-----------");
*/

      //Una volta creata la maschera delle esclusioni temporanea controlla i sensori
      ST_allarme = ST_ALLARME_CHECK_SENSOR;
      START_TIMER(allarme_timer1);
      break;
    case ST_ALLARME_CHECK_SENSOR:
      if (IS_DELAYED(allarme_timer1, ALLARME_CHECK_DELAY)) {
        //popola la mappa dei sensori in allarme
        map_allarm_sensor(&map_sensor);
        //Serial.print("MAPPA SENSORI : ");
        //Serial.println(map_sensor, HEX);
        //torna alla build temp mask per fare il loop di controllo
        ST_allarme = ST_ALLARME_BUILD_TEMP_MASK;
        if (map_sensor != (mask_esclusioni | mask_temp_esclusioni)) { // Se un sensore va in allarme si sposta alla procedura di segnalazione
          ST_allarme = ST_ALLARME_TEMPO_DI_INGRESSO;
          hold_allarm_sensor = map_sensor; //Copio i valori dei sensori che sono andati in allarme
          if (!ISALIVE_TIMER(allarme_timer0))START_TIMER(allarme_timer0); // fa partire il timer per il tempo d'ingresso
          STOP_TIMER(allarme_timer1);
        }
      }
      break;
    case ST_ALLARME_TEMPO_DI_INGRESSO:
      if (IS_DELAYED(allarme_timer0, SIRENA_ON_DELAYED)) {
        ST_allarme = ST_ALLARME_CYCLE_SIRENA_ON; // fa suonare la sirena
      } else {
        ST_allarme = ST_ALLARME_HOLD_SENSOR; //controllo gli eventuali sensori in allarme
      }
      break;
    case ST_ALLARME_HOLD_SENSOR:
      //popola la mappa dei sensori in allarme
      map_allarm_sensor(&map_sensor);
      hold_allarm_sensor = hold_allarm_sensor | map_sensor; //mantengo in memoria se nuovi sensori vanno in allarme
      if (ISALIVE_TIMER(allarme_timer0)) {
        ST_allarme = ST_ALLARME_TEMPO_DI_INGRESSO; // se l'hold sensor viene richiamato da TEMPO_DI_INGRESSO allora ritorna a quello stato
      } else {
        ST_allarme = ST_ALLARME_CYCLE_SIRENA_ON; // se l'hold sensor viene richiamato da CYCLE_SIRENA_ON allora ritorna a quello stato
      }
      break;
    case ST_ALLARME_CYCLE_SIRENA_ON:
      STOP_TIMER(allarme_timer0); // stoppo il timer0
      if ((new_st_sirena == SIRENA_OFF) && (digitalRead(SIRENA) == SIRENA_OFF) && (ISALIVE_TIMER(allarme_timer1))) { // Se la sirena è spenta ma è partito il timer allora fa partire la sirena
        trg_sirena = true;
        new_st_sirena = SIRENA_ON; // quando il relè si diseccita la sirena si accende.
        //notifico che c'è stato un'evento d'allarme
        notify_message_allarme = "!! INTRUSIONE RILEVATA !!";
       // Serial.print("1st Hold Mask ");
       // Serial.println(hold_allarm_sensor, HEX);
        hold_allarm_sensor = hold_allarm_sensor & !(mask_temp_esclusioni | mask_esclusioni);
        //Serial.print("2st Hold Mask ");
        //Serial.println(hold_allarm_sensor, HEX);
        if (GETBIT8(hold_allarm_sensor, BIT_ZONE1)) notify_message_allarme += "Zona1\t";
        if (GETBIT8(hold_allarm_sensor, BIT_ZONE2)) notify_message_allarme += "Zona2\t";
        if (GETBIT8(hold_allarm_sensor, BIT_ZONE3)) notify_message_allarme += "Zona3\t";
        if (GETBIT8(hold_allarm_sensor, BIT_ZONE4)) notify_message_allarme += "Zona4\t";
        if (GETBIT8(hold_allarm_sensor, BIT_TAMPER)) notify_message_allarme += "\n!!Manomissione rilevata!!\t";
        send_telegram_message(notify_message_allarme);
        START_TIMER(allarme_timer1); // fa partire il timer per il tempo di ciclo sirena

      } else if (IS_DELAYED(allarme_timer1, CYCLE_TIMER_SIRENA_ON)) { // se la sirena va in timeout per il tempo massimo di ciclo allarme allora si spegne
        trg_sirena = true;
        new_st_sirena = SIRENA_OFF; // quando il relè si eccita la sirena si spegne.
        STOP_TIMER(allarme_timer1);//Stoppa il timer usato per il ciclo sirena
        //Il timer2 per il tempo massimo di accensione sirena viene stoppato solo in BUILD_TEMP_MASK se i sensori che sono andati in allarme tornano a riposo
        ST_allarme = ST_ALLARME_BUILD_TEMP_MASK; //controlla se deve fare le maschere di autoesclusione e ricomincia il ciclo di alla
      } else {
        ST_allarme = ST_ALLARME_HOLD_SENSOR; // ritorna in fase di hold sensor
      }
      //Se la macchina sta per uscire da questo stato e va in fase BUILD_TEMP_MASK, allora segnala che è suonata l'allarme
      if (ST_allarme == ST_ALLARME_BUILD_TEMP_MASK) send_telegram_message("-Fine Ciclo Sirena-"); // invia una notifica di quali sensori sono andati in allarme
      break;
    case ST_ALLARME_RESET_ALL:
      trg_sirena = true;
      new_st_sirena = SIRENA_OFF; // quando il relè si eccita la sirena si spegne.
      //indico che l'allarme è disinserita
      stato_allarme = RIPOSO;
      //Resetto le autoesclusioni temporanee
      mask_temp_esclusioni = 0;
      //Resetto la mappa delle esclusioni
      mask_esclusioni = 0;
      //Resetto tutti i contatori
      RESET_COUNTER(allarme_counter0); //Resetto il counter 0
      //Resetto tutti i timer
      STOP_TIMER(allarme_timer0); // Resetta i timer
      STOP_TIMER(allarme_timer1); // Resetta i timer

      digitalWrite(BUZZER, LOW); //Mi assicuro che il buzzer sia sullo stato di riposo

      ST_allarme = ST_ALLARME_WAIT_KEY;
      break;
  }
}
void map_allarm_sensor(byte *map_var) {
  //Mappo lo stato dei sensori
  *map_var = (stato_zona1 == ALLARME) << BIT_ZONE1;
  *map_var = *map_var |  ((stato_zona2 == ALLARME) << BIT_ZONE2);
  *map_var = *map_var |  ((stato_zona3 == ALLARME) << BIT_ZONE3);
  *map_var = *map_var |  ((stato_zona4 == ALLARME) << BIT_ZONE4);
  *map_var = *map_var |  ((stato_tamper == ALLARME) << BIT_TAMPER);

}
String string_zone_builder() {


  String notify_builder1 = "";
  String notify_builder2 = "";

  notify_builder2 = ((stato_zona1   == RIPOSO) ? "Riposo  " : "Allarme ");
  notify_builder1 += "Zona 1 : " + notify_builder2 + " Escludibile : ";
  notify_builder2 = ((ZONA1_ESCLUDIBILE   == true) ? "SI  " : "NO ");
  notify_builder1 += notify_builder2 + "\n";

  notify_builder2 = ((stato_zona2   == RIPOSO) ? "Riposo  " : "Allarme ");
  notify_builder1 += "Zona 2 : " + notify_builder2 + " Escludibile : ";
  notify_builder2 = ((ZONA2_ESCLUDIBILE   == true) ? "SI  " : "NO ");
  notify_builder1 += notify_builder2 + "\n";



  notify_builder2 = ((stato_zona3   == RIPOSO) ? "Riposo  " : "Allarme ");
  notify_builder1 += "Zona 3 : " + notify_builder2 + " Escludibile : ";
  notify_builder2 = ((ZONA2_ESCLUDIBILE   == true) ? "SI  " : "NO ");
  notify_builder1 += notify_builder2 + "\n";




  notify_builder2 = ((stato_zona4   == RIPOSO) ? "Riposo  " : "Allarme ");
  notify_builder1 += "Zona 4 : " + notify_builder2 + " Escludibile : ";
  notify_builder2 = ((ZONA4_ESCLUDIBILE   == true) ? "SI  " : "NO ");
  notify_builder1 += notify_builder2 + "\n";



  notify_builder2 = ((stato_tamper   == RIPOSO) ? "Riposo  " : "Allarme ");
  notify_builder1 += "Tamper : " + notify_builder2 + " Escludibile : ";
  notify_builder2 = ((TAMPER_ESCLUDIBILE   == true) ? "SI  " : "NO ");
  notify_builder1 += notify_builder2 + "\n";
  return notify_builder1;
}
