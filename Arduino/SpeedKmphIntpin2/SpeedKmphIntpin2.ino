/*
30/09/2016 Wim Van Weyenberg
Speedometer: external interrupt on PIN2 high to low edge: read timervalue and reset
clock speed ATMega328 = 16 Mc
*/
/*
//calculations
//wheel circomference 24.6 cm
//max speed of 10 km/uur =  277,8 cm/second
//max rps = 11.29 rps
//min time ms/rev = 88.56 ms/rev = 88560 micros

12km/h => 73800
*/

#define OPTOINPUT 2//pin connected to read switch
#define LEDONBOARD 13
#define WHEELCIRCOMFERENCECM 24.6 //NOG AANPASSEN

volatile unsigned long microsecondsNow;
unsigned long microsecondsOld = 0;
unsigned long deltamicros;
double kmph;

void setup() 
{
  Serial.begin(9600);
    pinMode(LEDONBOARD, OUTPUT); //moet dit niet op HIGH gezet worden?
    pinMode(OPTOINPUT,INPUT_PULLUP);
    //bij ons niet falling, maar rising
    attachInterrupt(digitalPinToInterrupt(OPTOINPUT), ISRreadTime, RISING);
}

void loop(void)
{
  if(microsecondsNow!= microsecondsOld)
    {
      deltamicros = microsecondsNow - microsecondsOld;
      microsecondsOld = microsecondsNow;
      if(deltamicros > 73000) //wat is 73000 hier?, nog aanpassen waarschijnlijk
      {
      //Serial.println(deltamicros);
      kmph = WHEELCIRCOMFERENCECM*36000/deltamicros;
      Serial.print(kmph);
      Serial.println(" km/h");
      }
    }
}

void ISRreadTime(void) //method when interrupt has appeared
{
  microsecondsNow=micros();
}


