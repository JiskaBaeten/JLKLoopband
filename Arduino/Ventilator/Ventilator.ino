int ventilator = 6;
int buttonState;
int led = 13;
int test;

void setup()
{
  Serial.begin(9600);
  pinMode(ventilator, OUTPUT);
  pinMode(led, OUTPUT);
  digitalWrite(led, HIGH);
}

void loop()
{  
  if (Serial.available() > 0) {//if there's an input in the serial monitor
    test = Serial.read();
      VentiON();
  }
  
}

void VentiON()
{
  if (test == 'a')
  {
     digitalWrite(led, LOW);
     digitalWrite(ventilator, HIGH);
     delay(2000);
     digitalWrite(led, HIGH);
     digitalWrite(ventilator, LOW);
  }
}
