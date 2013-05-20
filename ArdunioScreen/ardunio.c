#include <LiquidCrystal.h>
LiquidCrystal lcd(8, 9, 4, 5, 6, 7);

int nChars     = 16;
int nLines     = 2;

void setup()
{
    lcd.begin(nChars, nLines);
    Serial.begin(9600);
}

void loop()
{
 String text = "";
 char currentChar;
  while(true)
 { 
 if(Serial.available() > 0) {   
      currentChar = Serial.read();
      if(currentChar != '\0')
      text = text + String(currentChar);
    }
      if(currentChar == '\0')
      {
       Serial.print(text);
      break;
      }

 }
 //Detta skall kunna göras till en for-loop om tex det finns mer lines än 2
 lcd.setCursor(0,0);
 lcd.print(text.substring(0,nChars-1));
 lcd.setCursor(0,1);
 lcd.print(text.substring(nChars));
}