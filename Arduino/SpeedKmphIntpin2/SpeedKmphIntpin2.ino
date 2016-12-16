/*
//calculations
//wheel circomference 2.3 cm*pi
//max speed of 10 km/uur =  277,8 cm/second => 10 / 0.036
//max rps = 277.8 cm/second / 2.3 cm = 120.783 rps
//min time ms/rev = 9360 micros = 9.360 ms/rev
//ZIE PAPIER VOOR MAX rotatie bij 12km/uur
*/

#define OPTOINPUT 2//pin connected to read switch
#define LEDONBOARD 13
#define WHEELPINDISTANCE 0.723 //wiel dat draait op band > OMTREK!! dlen door 10 mdat 10 pinnen

volatile unsigned long microsecondsNow;
unsigned long microsecondsOld = 0;
unsigned long deltamicros;
double kmph;

void setup() 
{
  Serial.begin(9600);
    pinMode(LEDONBOARD, OUTPUT); 
    pinMode(OPTOINPUT,INPUT); //mag geen pullup zijn door led
    attachInterrupt(digitalPinToInterrupt(OPTOINPUT), ISRreadTime, FALLING);
}

void loop(void)
{
  if(microsecondsNow!= microsecondsOld)
    {
      deltamicros = microsecondsNow - microsecondsOld;
      microsecondsOld = microsecondsNow;
      if(deltamicros > 2000) //als tijd te lang (stilstaan), tijd negeren (was 73000) tijd voor 12 km/uur afronden voor de veiligheid
      {
      kmph = WHEELPINDISTANCE*36000/deltamicros; //omtrek * 36000 (voor km) / tijd, 10 waarschijnlijk voor aantal flanken
      Serial.print(kmph);
      Serial.println(" km/h");
      }
    }
}

void ISRreadTime(void) //method when interrupt has appeared
{
  microsecondsNow=micros();
}


