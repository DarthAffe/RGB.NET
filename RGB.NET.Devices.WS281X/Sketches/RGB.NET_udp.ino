#define FASTLED_ESP8266_RAW_PIN_ORDER

#include "FastLED.h"
#include <ESP8266WiFi.h>
#include <WiFiUdp.h>

//#### CONFIGURATION ####

// WLAN settings
const char* ssid = ""; // WLAN-network-name
const char* password = ""; // WLAN-password

#define CHANNELS 4 // change this only if you add or remove channels in the implementation-part. To disable channels set them to 0 leds.

// should not exceed 168 leds, since that results in the maximum paket size that is safe to transmit. Everything above could potentially be dropped.
// no more than 255 leds per channel (hard limit)
#define LEDS_CHANNEL_1 3
#define LEDS_CHANNEL_2 0
#define LEDS_CHANNEL_3 0
#define LEDS_CHANNEL_4 0

#define PIN_CHANNEL_1 15 // D8
#define PIN_CHANNEL_2 13 // D7
#define PIN_CHANNEL_3 12 // D6
#define PIN_CHANNEL_4 14 // D5

#define PORT 1872 // if changed needs to be configured in RGB.NET (default: 1872)

#define WEBSERVER_PORT 80

//#######################

CRGB leds_channel_1[LEDS_CHANNEL_1];
CRGB leds_channel_2[LEDS_CHANNEL_2];
CRGB leds_channel_3[LEDS_CHANNEL_3];
CRGB leds_channel_4[LEDS_CHANNEL_4];

WiFiServer server(WEBSERVER_PORT);
WiFiUDP Udp;

byte incomingPacket[767]; // 255 (max leds) * 3 + 2 (header)
byte lastSequenceNumbers[CHANNELS];

String header;

void setup() {
  if(LEDS_CHANNEL_1 > 0) { FastLED.addLeds<NEOPIXEL, PIN_CHANNEL_1>(leds_channel_1, LEDS_CHANNEL_1); }
  if(LEDS_CHANNEL_2 > 0) { FastLED.addLeds<NEOPIXEL, PIN_CHANNEL_2>(leds_channel_2, LEDS_CHANNEL_2); }
  if(LEDS_CHANNEL_3 > 0) { FastLED.addLeds<NEOPIXEL, PIN_CHANNEL_3>(leds_channel_3, LEDS_CHANNEL_3); }
  if(LEDS_CHANNEL_4 > 0) { FastLED.addLeds<NEOPIXEL, PIN_CHANNEL_4>(leds_channel_4, LEDS_CHANNEL_4); }

  for(int i = 0; i < CHANNELS; i++)
  {
    lastSequenceNumbers[i] = 255;
  }
   
  WiFi.begin(ssid, password); 
  while (WiFi.status() != WL_CONNECTED)
  {
    delay(500);
  }  
  delay(100);
  
  Udp.begin(PORT);
  server.begin();
}

bool checkSequenceNumber(int channel, byte currentSequenceNumber)
{
  bool isValid = (currentSequenceNumber > lastSequenceNumbers[channel]) || ((lastSequenceNumbers[channel] > 200) && (currentSequenceNumber < 50));
  if(isValid)
  {
    lastSequenceNumbers[channel] = currentSequenceNumber;
  }
  return isValid;
}

void loop() {
  // Web client
  WiFiClient client = server.available();
  if (client) 
  {
    String currentLine = "";
    while (client.connected())
    {
      if (client.available())
      {
        char c = client.read();
        header += c;
        if (c == '\n') {
          if (currentLine.length() == 0) {
            client.println("HTTP/1.1 200 OK");
            client.println("Content-type:text/html");
            client.println("Connection: close");
            client.println();
            
            if (header.indexOf("GET /reset") >= 0)
            {
              for(int i = 0; i < CHANNELS; i++)
              {
                lastSequenceNumbers[i] = 255;                
              }
              for(int i = 0; i < LEDS_CHANNEL_1; i++)
              {
                leds_channel_1[i] = CRGB::Black;               
              }
              for(int i = 0; i < LEDS_CHANNEL_2; i++)
              {
                leds_channel_2[i] = CRGB::Black;               
              }
              for(int i = 0; i < LEDS_CHANNEL_3; i++)
              {
                leds_channel_3[i] = CRGB::Black;               
              }
              for(int i = 0; i < LEDS_CHANNEL_4; i++)
              {
                leds_channel_4[i] = CRGB::Black;               
              }
              FastLED.show();
            }
            else if (header.indexOf("GET /channels") >= 0)
            {
              client.println(CHANNELS);
            }
            else if (header.indexOf("GET /channel/1") >= 0)
            {
              client.println(LEDS_CHANNEL_1);
            }
            else if (header.indexOf("GET /channel/2") >= 0)
            {
              client.println(LEDS_CHANNEL_2);
            }
            else if (header.indexOf("GET /channel/3") >= 0)
            {
              client.println(LEDS_CHANNEL_3);
            }
            else if (header.indexOf("GET /channel/4") >= 0)
            {
              client.println(LEDS_CHANNEL_4);
            }
            client.println();
            
            break;
          }
          else 
          {
            currentLine = "";
          }
        }
        else if (c != '\r') 
        {
          currentLine += c;
        }
      }
    }
    header = "";
    client.stop();
  }

  // Color update
  int packetSize = Udp.parsePacket();
  if (packetSize)
  {
    // receive incoming UDP packets
    byte sequenceNumber = Udp.read();
    byte command = Udp.read();
    switch(command)
    {
      // ### channel 1 ###
      case 0x12: // set leds of channel 1
        if(checkSequenceNumber(0, sequenceNumber))
        {
          Udp.read(((uint8_t*)leds_channel_1), (LEDS_CHANNEL_1 * 3));
          FastLED.show();
        }
        break;

      // ### channel 2 ###
      case 0x22: // set leds of channel 2        
        if(checkSequenceNumber(1, sequenceNumber))
        {
          Udp.read(((uint8_t*)leds_channel_2), (LEDS_CHANNEL_2 * 3));
          FastLED.show();
        }
        break;

      // ### channel 3 ###
      case 0x32: // set leds of channel 3
        if(checkSequenceNumber(2, sequenceNumber))
        {
          Udp.read(((uint8_t*)leds_channel_3), (LEDS_CHANNEL_3 * 3));
          FastLED.show();
        }
        break;

      // ### channel 4 ###
      case 0x42: // set leds of channel 4
        if(checkSequenceNumber(3, sequenceNumber))
        {
          Udp.read(((uint8_t*)leds_channel_4), (LEDS_CHANNEL_4 * 3));
          FastLED.show();
        }
        break;

      // ### default ###
      default:
        break;
    }
  }
}
