#include "FastLED.h"

// This sketch allows to use the arduino as a BitwizardWS2812USBDevice.
// Since the communication is text-based it's to slow to process realtime data,
// but it can be used from a basic terminal by hand.
//
// If you want to use it only with RGB.NET you should in nearly all cases prefer the 'RGB.NET_Arduino'-sketch.

//#### CONFIGURATION ####

#define NUM_LEDS 384
#define LED_PIN 6

#define BAUD_RATE 115200
#define SERIAL_PROMPT '>'

//#######################

bool serialCommandReceived = false;
int bufferLength = 0;
char inputBuffer[256] = "";

CRGB leds[NUM_LEDS];

//-----------------------

void setLed(int led, long r, long g, long b)
{
  leds[led].setRGB(r, g, b);
}

void setLed(int led, long color)
{  
  long r = color >> 16;
  long g = color >> 8 & 0xFF;
  long b = color & 0xFF;
  setLed(led, r, g, b);
}

void setAllLeds(long r, long g, long b)
{
  fill_solid(leds, NUM_LEDS, CRGB(r, g, b));
}

void updateLeds()
{
  FastLED.show(); 
}

void setup()
{
  FastLED.addLeds<NEOPIXEL, LED_PIN>(leds, NUM_LEDS); 
  
  Serial.begin(BAUD_RATE);
  Serial.println(SERIAL_PROMPT);
}

void loop() 
{
  if(serialCommandReceived)
  {
    if(strncmp(&inputBuffer[0], "set", 3) == 0)
    {
      char *end;
      int led = (int)strtol(&inputBuffer[3], &end, 10);
      long color = strtol(end, NULL, 16);
      setLed(led, color);
    }
    else if(strncmp(&inputBuffer[0], "update", 6) == 0)
    {
      updateLeds();
    }
    else if(strncmp(&inputBuffer[0], "pix", 3) == 0) // shortcut for set and update (as in the bitwizard controller)
    {
      char *end;
      int led = (int)strtol(&inputBuffer[3], &end, 10);
      long color = strtol(end, NULL, 16);
      setLed(led, color);
      updateLeds();
    }
    else if(strncmp(&inputBuffer[0], "black", 5) == 0) // shortcut for all set black and update
    {
      setAllLeds(0, 0, 0);
      updateLeds();
    }
    else if(strncmp(&inputBuffer[0], "white", 5) == 0) // shortcut for all set white and update
    {
      setAllLeds(255, 255, 255);
      updateLeds();
    }
    else if(bufferLength > 0)
    {
      Serial.println("unknown command '" + String(inputBuffer) + "'");      
    }
    
    Serial.println(SERIAL_PROMPT);

    memset(&inputBuffer[0], 0, sizeof(inputBuffer));
    bufferLength = 0;
    serialCommandReceived = false;
  }
}

void serialEvent()
{
  while (Serial.available() && !serialCommandReceived) 
  {
    char inChar = (char)Serial.read();
    switch(inChar)
    {      
      case '\r':
        serialCommandReceived = true;
        break;

      case '\n':
        serialCommandReceived = true;
        break;
      
      default:
        inputBuffer[bufferLength++] = inChar;
        break;      
    }
  }
}
