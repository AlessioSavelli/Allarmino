/*
 * Questo modulo include la chiave publica per la lettura della firma digitale del firmware che viene caricato da remoto.
 * N.B: Da remoto può essere essere effettuato il flashing di un fiwmware firmato con estenzione .bin.
 *      mentre in locale con cavo usb può essere caricato qualsiasi firmware tramite IDE di arduino o il software dedicato(Allarmino Suite) per la configurazione di Allarmino.
 */
 #define SECURE_FLASH_CHECK_SIGN   "Allarmino Firmware Signed"
