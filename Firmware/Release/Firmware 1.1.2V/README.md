[+] Aggiunta espanzione Ethernet
	[+]Funziona con W5500
	[+]Supporta il led : linkStatus
	[+]Telegram funziona anche con Ethernet
[+] Novità Telegram
	[+]Abilitando il PRG JMPR è possibile attivare l'aggiornamento firmware tramite telegram
	[+]Admin Session aggiunta per garantire l'aggiornamento firmware solo dall'amministratore
[+] FIX BUG
 -compatibilità NTP con Ethernet UDP
 -freez allarmino dovuto a periferiche internet non inizializzabili
 -riconnessione ethernet per Telegram con Ethernet
[+] Aggiunte
   [+]Aggiunta lampeggio led AL e ST durante lo start-up delle periferiche internet
   in caso di errore dovuto all'ethernet il timeout dura 10s/20s prima che la centralina
   inizii a funzionare senza internet.
   [+]Aggiunta segnalazione di assenza internet o errore inizializzazione ethernet con l'accensione fissa del led AL
   [+]Aggiunta segnalazione stato inserimento allarme con il led ST. Led spento = Allarme Attiva, Led Accesso = Allarme disattiva
