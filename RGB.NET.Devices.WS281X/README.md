[RGB.NET](https://github.com/DarthAffe/RGB.NET) Device-Provider-Package for RGB.NET-Devices based on Aurdion or ESP8266.

To create your own Aurdion/ESP8266 based controller check the Sketches at [https://github.com/DarthAffe/RGB.NET/tree/master/RGB.NET.Devices.WS281X/Sketches](https://github.com/DarthAffe/RGB.NET/tree/master/RGB.NET.Devices.WS281X/Sketches)

> If you start a new project it's recommended to use the Raspberry Pi Pico due to it's better performance and lower cost. Check the `RGB.NET.Devices.PicoPi`-package.

> This provider does not work with WLED. If you want to use that check the `RGB.NET.Devices.DMX`-package.

## Usage
This provider does not load anything by default and requires additional configuration to work.

```csharp
// Add as many Arduino or NodeMCU devices as you like
WS281XDeviceProvider.Instance.AddDeviceDefinition(new ArduinoWS281XDeviceDefinition("COM3"));

surface.Load(WS281XDeviceProvider.Instance);
```

# Required SDK
This provider does not require an additional SDK.
