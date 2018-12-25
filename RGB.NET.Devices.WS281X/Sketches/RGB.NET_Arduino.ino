#include "FastLED.h"

// This sketch allows to use the arduino as ArduinoWS2812USBDevice.
// The amount of leds that can be handled in realtime mostly depends on the speef of the arduino,
// but an arduino UNO is doing quite well.

//#### CONFIGURATION ####

#define CHANNELS 4 // change this only if you add or remove channels in the implementation-part. To disable channels set them to 0 leds.

// no more than 255 leds per channel
#define LEDS_CHANNEL_1 32
#define LEDS_CHANNEL_2 0
#define LEDS_CHANNEL_3 0
#define LEDS_CHANNEL_4 0

#define PIN_CHANNEL_1 6
#define PIN_CHANNEL_2 7
#define PIN_CHANNEL_3 8
#define PIN_CHANNEL_4 9

#define BAUD_RATE 115200
#define SERIAL_PROMPT ">"

//#######################

CRGB leds_channel_1[LEDS_CHANNEL_1];
CRGB leds_channel_2[LEDS_CHANNEL_2];
CRGB leds_channel_3[LEDS_CHANNEL_3];
CRGB leds_channel_4[LEDS_CHANNEL_4];

byte command = 0;

//-----------------------

void setup()
{
  if(LEDS_CHANNEL_1 > 0) { FastLED.addLeds<NEOPIXEL, PIN_CHANNEL_1>(leds_channel_1, LEDS_CHANNEL_1); }
  if(LEDS_CHANNEL_2 > 0) { FastLED.addLeds<NEOPIXEL, PIN_CHANNEL_2>(leds_channel_2, LEDS_CHANNEL_2); }
  if(LEDS_CHANNEL_3 > 0) { FastLED.addLeds<NEOPIXEL, PIN_CHANNEL_3>(leds_channel_3, LEDS_CHANNEL_3); }
  if(LEDS_CHANNEL_4 > 0) { FastLED.addLeds<NEOPIXEL, PIN_CHANNEL_4>(leds_channel_4, LEDS_CHANNEL_4); }

  Serial.begin(BAUD_RATE);
  Serial.print(SERIAL_PROMPT);
}

void loop()
{
  if(command > 0)
  {
    switch(command)
    {
      // ### default ###
      case 0x01: // get channel-count
        Serial.write(CHANNELS);
        break;

      case 0x02: // update
        FastLED.show();
        break;

      case 0x0F: // ask for prompt
        break;

      // ### channel 1 ###
      case 0x11: // get led-count of channel 1
        Serial.write(LEDS_CHANNEL_1);
        break;
      case 0x12: // set leds of channel 1
        Serial.readBytes(((uint8_t*)leds_channel_1), (LEDS_CHANNEL_1 * 3));
        break;

      // ### channel 2 ###
      case 0x21: // get led-count of channel 2
        Serial.write(LEDS_CHANNEL_2);
        break;
      case 0x22: // set leds of channel 2
        Serial.readBytes(((uint8_t*)leds_channel_2), (LEDS_CHANNEL_2 * 3));
        break;

      // ### channel 3 ###
      case 0x31: // get led-count of channel 3
        Serial.write(LEDS_CHANNEL_3);
        break;
      case 0x32: // set leds of channel 3
        Serial.readBytes(((uint8_t*)leds_channel_3), (LEDS_CHANNEL_3 * 3));
        break;

      // ### channel 4 ###
      case 0x41: // get led-count of channel 4
        Serial.write(LEDS_CHANNEL_4);
        break;
      case 0x42: // set leds of channel 4
        Serial.readBytes(((uint8_t*)leds_channel_4), (LEDS_CHANNEL_4 * 3));
        break;

      // ### default ###
      default:
        command = 0;
        return; // no prompt
    }

    Serial.print(SERIAL_PROMPT);
    command = 0;
  }
}

void serialEvent()
{
  if (Serial.available())
  {
    command = Serial.read();
  }
}
