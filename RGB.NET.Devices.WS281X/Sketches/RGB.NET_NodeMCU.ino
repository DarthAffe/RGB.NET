#define FASTLED_ESP8266_RAW_PIN_ORDER

#include "FastLED.h"
#include <ESP8266WiFi.h>
#include <ESP8266WebServer.h>
#include <WiFiUdp.h>
#include "base64.hpp"

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

#define WEBSERVER_PORT 80

//#######################

CRGB leds_channel_1[LEDS_CHANNEL_1];
CRGB leds_channel_2[LEDS_CHANNEL_2];
CRGB leds_channel_3[LEDS_CHANNEL_3];
CRGB leds_channel_4[LEDS_CHANNEL_4];

ESP8266WebServer server(WEBSERVER_PORT);
WiFiUDP Udp;

bool isUDPEnabled;
int udpPort;
byte incomingPacket[767]; // 255 (max leds) * 3 + 2 (header)
byte lastSequenceNumbers[CHANNELS];

bool checkSequenceNumber(int channel, byte currentSequenceNumber)
{
  bool isValid = (currentSequenceNumber > lastSequenceNumbers[channel]) || ((lastSequenceNumbers[channel] > 200) && (currentSequenceNumber < 50));
  if(isValid)
  {
    lastSequenceNumbers[channel] = currentSequenceNumber;
  }
  return isValid;
}

void processUDP()
{
  int packetSize = Udp.parsePacket();
  if (packetSize)
  {
    // receive incoming UDP packets
    byte sequenceNumber = Udp.read();
    byte channel = Udp.read();
    if(checkSequenceNumber(channel, sequenceNumber))
    {
      switch(channel)
      {
        case 1: // set leds of channel 1
          Udp.read((uint8_t*)leds_channel_1, (LEDS_CHANNEL_1 * 3));
          FastLED.show();
          break;

        // ### channel 2 ###
        case 2: // set leds of channel 2        
          Udp.read((uint8_t*)leds_channel_2, (LEDS_CHANNEL_2 * 3));
          FastLED.show();
          break;

        // ### channel 3 ###
        case 3: // set leds of channel 3
          Udp.read((uint8_t*)leds_channel_3, (LEDS_CHANNEL_3 * 3));
          FastLED.show();
          break;

        // ### channel 4 ###
        case 4: // set leds of channel 4
          Udp.read((uint8_t*)leds_channel_4, (LEDS_CHANNEL_4 * 3));
          FastLED.show();
          break;

        // ### default ###
        default:
          break;
      }
    }
  }
}

void handleRoot()
{
  String infoSite = (String)"<html>\
  <head>\
    <title>RGB.NET</title>\
  </head>\
  <body>\
    <h1>RGB.NET</h1>\
    This device is currently running the NodeMCU WS281X RGB.NET-Sketch.<br />\
    <br />\
    Check <a href=\"https://github.com/DarthAffe/RGB.NET\">https://github.com/DarthAffe/RGB.NET</a> for more info and the latest version of this sketch.<br />\
    <br />\
    <h3>Configuration:</h3>\
    <b>UDP:</b>\ " + (isUDPEnabled ? ((String)"enabled (" + udpPort + ")") : "disabled") + "<br />\
    <br />\    
    <b>Channel 1</b><br />\
    Leds: " + LEDS_CHANNEL_1 + "<br />\
    Pin: " + PIN_CHANNEL_1 + "<br />\
    <br />\
    <b>Channel 2</b><br />\
    Leds: " + LEDS_CHANNEL_2 + "<br />\
    Pin: " + PIN_CHANNEL_2 + "<br />\
    <br />\
    <b>Channel 4</b><br />\
    Leds: " + LEDS_CHANNEL_3 + "<br />\
    Pin: " + PIN_CHANNEL_3 + "<br />\
    <br />\
    <b>Channel 4</b><br />\
    Leds: " + LEDS_CHANNEL_4 + "<br />\
    Pin: " + PIN_CHANNEL_4 + "<br />\
    <br />\
  </body>\
</html>";

  server.send(200, "text/html", infoSite);
}

void handleConfig()
{
  String config = (String)"{\
  \"channels\": [\
    {\
      \"channel\": 1,\
      \"leds\": " + LEDS_CHANNEL_1 + "\
    },\
    {\
      \"channel\": 2,\
      \"leds\": " + LEDS_CHANNEL_2 + "\
    },\
    {\
      \"channel\": 3,\
      \"leds\": " + LEDS_CHANNEL_3 + "\
    },\
    {\
      \"channel\": 4,\
      \"leds\": " + LEDS_CHANNEL_4 + "\
    }\
  ]\
}";

  server.send(200, "application/json", config);
}

void handleEnableUDP()
{
  if(isUDPEnabled)
  {
    Udp.stop();
  }
  
  udpPort = server.arg(0).toInt();
    
  Udp.begin(udpPort);
  isUDPEnabled = true;
  
  server.send(200, "text/html", "");
}

void handleDisableUDP()
{
  if(isUDPEnabled)
  {
    Udp.stop();
    isUDPEnabled = false;
  }
  
  server.send(200, "text/html", "");
}

void handleReset()
{
  for(int i = 0; i < CHANNELS; i++)
  {
    lastSequenceNumbers[i] = 0;                
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
  
  server.send(200, "text/html", "");
}

void handleUpdate()
{
  unsigned int dataLength = decode_base64((unsigned char*)server.arg(0).c_str(), incomingPacket);
  
  byte channel = (byte)incomingPacket[1];
  switch(channel)
  {
    case 1: // set leds of channel 1
      memcpy((uint8_t*)leds_channel_1, &incomingPacket[2], (LEDS_CHANNEL_1 * 3));
      FastLED.show();
      break;

    // ### channel 2 ###
    case 2: // set leds of channel 2        
      memcpy((uint8_t*)leds_channel_2, &incomingPacket[2], (LEDS_CHANNEL_2 * 3));
      FastLED.show();
      break;

    // ### channel 3 ###
    case 3: // set leds of channel 3
      memcpy((uint8_t*)leds_channel_3, &incomingPacket[2], (LEDS_CHANNEL_3 * 3));
      FastLED.show();
      break;

    // ### channel 4 ###
    case 4: // set leds of channel 4
      memcpy((uint8_t*)leds_channel_4, &incomingPacket[2], (LEDS_CHANNEL_4 * 3));
      FastLED.show();
      break;

    // ### default ###
    default:
      break;
  }
  
  server.send(200, "text/html", "");
}

void setup() 
{
  if(LEDS_CHANNEL_1 > 0) { FastLED.addLeds<NEOPIXEL, PIN_CHANNEL_1>(leds_channel_1, LEDS_CHANNEL_1); }
  if(LEDS_CHANNEL_2 > 0) { FastLED.addLeds<NEOPIXEL, PIN_CHANNEL_2>(leds_channel_2, LEDS_CHANNEL_2); }
  if(LEDS_CHANNEL_3 > 0) { FastLED.addLeds<NEOPIXEL, PIN_CHANNEL_3>(leds_channel_3, LEDS_CHANNEL_3); }
  if(LEDS_CHANNEL_4 > 0) { FastLED.addLeds<NEOPIXEL, PIN_CHANNEL_4>(leds_channel_4, LEDS_CHANNEL_4); }

  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED)
  {
    delay(500);
  }
  
  delay(100);

  server.on("/", handleRoot);
  server.on("/config", handleConfig);
  server.on("/enableUDP", handleEnableUDP);
  server.on("/disableUDP", handleDisableUDP);
  server.on("/reset", handleReset);
  server.on("/update", handleUpdate);
  server.onNotFound(handleRoot);
  
  server.begin();

  handleReset();
}

void loop()
{
  server.handleClient();
  
  if(isUDPEnabled)
  {
    processUDP();
  }
}
 
