/*
   Questo file contiene tutte le funzioni, variabili e macchine a stati che vengono utilizzati per l'algorittimo dell'aggiornamento Firmware
*/

#include "Client.h"
#include <Update.h>

//----Define Hedaers per la lettura del firmware
#define FW_MAGIC_BYTE   0xE9  //Identifica il primo byte del firmware

//codici di result per capire com'è andato l'aggiornamento del FW
enum fw_update_result {
  HTTP_UPDATE_CLIENT_ERROR,
  HTTP_UPDATE_FLUSH_TIMEOUT,
  HTTP_UPDATE_READING_TIMEOUT,
  HTTP_UPDATE_NOHEADER_TIMEOUT,
  HTTP_UPDATE_NO_SPACE,
  HTTP_UPDATE_FAILED,
  HTTP_UPDATE_OK,
  HTTP_UPDATE_OK_NO_FLUSH
};

#define FW_UPDATING_BUFFER_SIZE 1*KByte //Dimensione ram esp32 4520 KB / Il client scarica massimo a 1Kb
byte firmwarebuffer[FW_UPDATING_BUFFER_SIZE];

unsigned long fw_timeout = 5 * MINUTES; //Timeout standard

void fw_update_timeout(unsigned long time);
unsigned int fw_https_download(Client &updatingClient, String HostName, String fw_path, size_t size);
void fw_apply_update();
