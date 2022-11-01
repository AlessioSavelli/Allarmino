
/*
 * Definisco delle macro generiche che vanno bene per tutto i moduli
 * N.B Ogni modulo/funzione può avere le sue macro specifiche solo per lei. Sono tutte dichiarata nei file .h o all'inizio delle funzioni
 */

//----Definisco un Delay da usare per i core nel freertos---
#define TASKDELAY(MS) vTaskDelay(MS / portTICK_RATE_MS);
//------

//----Definisco Macro per gestire i timer----
#define IS_DELAYED(TIMER,MY_TIMER)  ((TIMER!=0)?((millis()-((unsigned long)TIMER))>=MY_TIMER):false)
#define START_TIMER(TIMER)          (TIMER=millis())
#define STOP_TIMER(TIMER)           (TIMER=0)
#define ISALIVE_TIMER(TIMER)        (TIMER>0)
//-------------
//-----Definisco le macro per counter----
#define RESET_COUNTER(counter)           (counter=0)
//----
//----Definsico le macro per i buffer circolare---
#define CIRC_BUFFER_MULTISCROLL(index,scroll,maxItem) (index=(index+scroll)%maxItem)
#define CIRC_BUFFER_ONESCROLL(index,maxItem)          (CIRC_BUFFER_MULTISCROLL(index,1,maxItem))
#define REMOVE_FROM_INDEX(index,len)  (index-=len)
#define ADD_TO_INDEX(index,len)  (index+=len)
//------
//-----Definisco le macro per gestire i bit----
#define BITMASK8(bitnum)                        (1<<bitnum)
#define GETBIT8(val,bitnum)                     ((val & BITMASK8(bitnum))!=0)
#define BUILDBITMASK8(b7,b6,b5,b4,b3,b2,b1,b0)  ((b0)|(b1<<1)|(b2<<2)|(b3<<3)|(b4<<4)|(b5<<5)|(b6<<6)|(b7<<7))
#define CHECKBITMASK(val,bitmask)               ((val&bitmask) == bitmask)
#define BITMASKRESET(bitmask)                   (bitmask=0x00)
//----


//---Definisco macro per le conversione dei tempi
#define SECONDS 1000
#define MINUTES 60*SECONDS
//------

//--Definisco macro per la conversione dei byte
#define KByte 1024
#define MByte 1024*KByte
//----------

//---Definisco macro per configurare il timezone dell'ntp (rtc over wifi)
#define GETGMT(adj) (adj * 3600)
//----
