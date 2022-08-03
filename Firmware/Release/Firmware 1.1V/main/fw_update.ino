
void fw_update_timeout(unsigned long time) {
  fw_timeout = time;
}

unsigned int fw_https_download(Client &updatingClient, String HostName, String fw_path, size_t size) {
  //char _HostName[HostName.length()];
  //HostName.toCharArray(_HostName,HostName.length());
  if (updatingClient.connected()) updatingClient.stop(); // il client è gia connesso a qualcosa allora lo ferma
  if (!updatingClient.connect(HostName.c_str(), 443))return HTTP_UPDATE_CLIENT_ERROR; //Non riesce a collegarsi al client
  if (!updatingClient.connected())return HTTP_UPDATE_CLIENT_ERROR;
  digitalWrite(LED_AL, LOW);
  digitalWrite(LED_ST, LOW);
  unsigned long timeout_timer = 0; //Creo un timer per il timeout
  //Compilo la richiesta HTTPS
  String get_request = "GET " + fw_path + " \nHTTP/1.0\n";
#if INTERNET_MODE  == INTERNET_Eth
  get_request += "User-Agent: AllarminoClient SSLClient Eth/ESP32 Board" + String(FW_VERSION) + "\n";
#elif INTERNET_MODE  == INTERNET_WiFi
  get_request += "User-Agent: AllarminoClient WiFiClientSecure Wifi/ESP32 Board" + String(FW_VERSION) + "\n";
#endif
  get_request += "Token: " + String(millis()) + "\n";
  get_request += "Host: " + HostName + "n";
  get_request += "Connection: keep-alive\n";
  get_request += "Accept-Ranges: bytes";
  updatingClient.flush();
  START_TIMER(timeout_timer);
  while (updatingClient.available()) { // pulisce lo stream di lettura
    updatingClient.read();
    if (IS_DELAYED(timeout_timer, 10 * SECONDS))return HTTP_UPDATE_FLUSH_TIMEOUT;
  }
  digitalWrite(LED_ST, HIGH);
  updatingClient.println(get_request); // effettuo la richiesta di download a telegram
  START_TIMER(timeout_timer);
  //rimane in attesa dei dati
  while (updatingClient.available() <= 0) {
    if (IS_DELAYED(timeout_timer, fw_timeout)) {
      digitalWrite(LED_AL, LOW);
      digitalWrite(LED_ST, LOW);
      return HTTP_UPDATE_READING_TIMEOUT;
    }
    TASKDELAY(10);
  }
  //Riamane in attesa dell'header
  digitalWrite(LED_AL, HIGH);
  digitalWrite(LED_ST, LOW);
  byte header;
  while (header != FW_MAGIC_BYTE) { //Aggiungere il timeout
    if (updatingClient.available()) {
      updatingClient.read(&header, 1);
    } else if (IS_DELAYED(timeout_timer, fw_timeout)) {
      digitalWrite(LED_AL, LOW);
      digitalWrite(LED_ST, LOW);
      return HTTP_UPDATE_NOHEADER_TIMEOUT;
    }
  }
  digitalWrite(LED_AL, LOW);
  digitalWrite(LED_ST, LOW);
  if (!Update.begin(size, U_FLASH)) { //Per aggiornare la flash o U_SPIFFS
    updatingClient.stop();
    digitalWrite(LED_AL, LOW);
    digitalWrite(LED_ST, LOW);
    return HTTP_UPDATE_NO_SPACE;
  }
  //Scrive già l'header del firmware
  Update.write(&header, 1);
  REMOVE_FROM_INDEX(size, 1);

  unsigned int reading_buff = 0;
  while (size > 0) {

    if (updatingClient.available() > 0) {
      reading_buff = updatingClient.available();
      if (updatingClient.available() > FW_UPDATING_BUFFER_SIZE)
        reading_buff = FW_UPDATING_BUFFER_SIZE;
      else if (reading_buff > size)
        reading_buff = size;

      if (!updatingClient.read(firmwarebuffer, reading_buff)) {
        digitalWrite(LED_AL, LOW);
        digitalWrite(LED_ST, LOW);
        return HTTP_UPDATE_FAILED;
      }
      Update.write(firmwarebuffer, reading_buff);
      REMOVE_FROM_INDEX(size, reading_buff);
      digitalWrite(LED_AL, !digitalRead(LED_ST));
      digitalWrite(LED_ST, !digitalRead(LED_AL));
    } else {
      TASKDELAY(1);
    }
  }

  //---
  if (size > 0) {
    Update.abort();
    digitalWrite(LED_AL, LOW);
    digitalWrite(LED_ST, LOW);
    return HTTP_UPDATE_FAILED;
  }

  Update.end(false);
  digitalWrite(LED_AL, LOW);
  digitalWrite(LED_ST, LOW);
  updatingClient.flush();
  START_TIMER(timeout_timer);
  while (updatingClient.available()) { // pulisce lo stream di lettura
    updatingClient.read();
    if (IS_DELAYED(timeout_timer, 10 * SECONDS))return HTTP_UPDATE_OK_NO_FLUSH;
  }
  return HTTP_UPDATE_OK;
}
void fw_apply_update() {
  ESP.restart();
}
