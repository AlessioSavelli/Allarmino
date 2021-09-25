
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
//------
//-----Definisco le macro per gestire i bit----
#define BITMASK8(bitnum) (1<<bitnum)
#define GETBIT8(val,bitnum) ((val & BITMASK8(bitnum))!=0)
#define BUILDBITMASK8(b7,b6,b5,b4,b3,b2,b1,b0) ((b0)|(b1<<1)|(b2<<2)|(b3<<3)|(b4<<4)|(b5<<5)|(b6<<6)|(b7<<7))
#define CHECKBITMASK(val,bitmask) ((val&bitmask) == bitmask)
//----
//---Definisco macro per le conversione dei tempi
#define SECONDS 1000
#define MINUTES 60*SECONDS
//------

//---Definisco macro per configurare il timezone dell'ntp (rtc over wifi)
#define GETGMT(adj) (adj * 3600)
//----
