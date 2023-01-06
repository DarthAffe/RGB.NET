[RGB.NET](https://github.com/DarthAffe/RGB.NET) Device-Provider-Package for RGB.NET-Devices based on the Raspi Pi Pico.

To create your own Raspberry PI Pico based controller check the RGB.NET Firmware at [https://github.com/DarthAffe/RGB.NET-PicoPi](https://github.com/DarthAffe/RGB.NET-PicoPi)

## Usage
This provider follows the default pattern and does not require additional setup.

```csharp
surface.Load(PicoPiDeviceProvider.Instance);
```

# Required SDK
This provider does not require an additional SDK.
