# RGB.NET
[![GitHub release (latest by date)](https://img.shields.io/github/v/release/DarthAffe/RGB.NET?style=for-the-badge)](https://github.com/DarthAffe/RGB.NET/releases)
[![Nuget](https://img.shields.io/nuget/v/RGB.NET.Core?style=for-the-badge)](https://www.nuget.org/packages?q=rgb.net)
[![GitHub](https://img.shields.io/github/license/DarthAffe/RGB.NET?style=for-the-badge)](https://github.com/DarthAffe/RGB.NET/blob/master/LICENSE)
[![GitHub Repo stars](https://img.shields.io/github/stars/DarthAffe/RGB.NET?style=for-the-badge)](https://github.com/DarthAffe/RGB.NET/stargazers)
[![Discord](https://img.shields.io/discord/366163308941934592?logo=discord&logoColor=white&style=for-the-badge)](https://discord.gg/9kytURv) 

> **IMPORTANT NOTE**   
This is a library to integrate RGB-devices into your own application. It does not contain any executables!   
If you're looking for a full blown software solution to manage your RGB-devices, take a look at [Artemis](https://artemis-rgb.com/).

## Getting Started
### Setup
1. Add the [RGB.NET.Core](https://www.nuget.org/packages/RGB.NET.Core) and [Devices](https://www.nuget.org/packages?q=rgb.net.Devices)-Nugets for all devices you want to use.
2. For some of the vendors SDK-libraries are needed. Check the contained Readmes for more information in that case.
3. Create a new `RGBSurface`.
```csharp
RGBSurface surface = new RGBSurface();
```

4. Initialize the providers for all devices you want to use and add the devices to the surface. For example:
```csharp
CorsairDeviceProvider.Instance.Initialize(throwExceptions: true);
surface.Attach(CorsairDeviceProvider.Instance.Devices);
```
The `Initialize`-method allows to load only devices of specific types by setting a filter and for debugging purposes allows to enable exception throwing. (By default they are catched and provided through the `Exception`-event.)
You can also use the `Load`-Extension on the surface.
```csharp
surface.Load(CorsairDeviceProvider.Instance);
```
> While most device-providers are implemented in a way that supports fast loading like this some may have a different loading procedures. (For example the `WS281XDeviceProvider` requires device-definitions before loading.)

5. Add an update-trigger. In most cases the TimerUpdateTrigger is preferable, but you can also implement your own to fit your needs.
```csharp
surface.RegisterUpdateTrigger(new TimerUpdateTrigger());
```
> If you want to trigger updates manually the `ManualUpdateTrigger` should be used.

6. *This step is optional but recommended.* For rendering the location of each LED on the surface can be important. Since not all SDKs provide useful layout-information you might want to add Layouts to your devices. (TODO: add wiki article for this)
Same goes for the location of the device on the surface. If you don't care about the exact location of the devices you can use:
```csharp
surface.AlignDevices();
```

The basic setup is now complete and you can start setting up your rendering.

### Basic Rendering
As an example we'll add a moving rainbow over all devices on the surface.
1. Create a led-group containing all leds on the surface (all devices)
```csharp
ILedGroup allLeds = new ListLedGroup(surface, surface.Leds);
``` 

2. Create a rainbow-gradient.
```csharp
RainbowGradient rainbow = new RainbowGradient();
```

3. Add a decorator to the gradient to make it move. (Decorators are 
```csharp
rainbow.AddDecorator(new MoveGradientDecorator(surface));
```

4. Create a texture (the size - in this example 10, 10 - is not important here since the gradient shoukd be stretched anyway)
```csharp
ITexture texture = new ConicalGradientTexture(new Size(10, 10), rainbow);
```

5. Add a brush rendering the texture to the led-group
```csharp
allLeds.Brush = new TextureBrush(texture);
```

### Full example
```csharp
RGBSurface surface = new RGBSurface();
surface.Load(CorsairDeviceProvider.Instance);
surface.AlignDevices();

surface.RegisterUpdateTrigger(new TimerUpdateTrigger());

ILedGroup allLeds = new ListLedGroup(surface, surface.Leds);
RainbowGradient rainbow = new RainbowGradient();
rainbow.AddDecorator(new MoveGradientDecorator(surface));
ITexture texture = new ConicalGradientTexture(new Size(10, 10), rainbow);
allLeds.Brush = new TextureBrush(texture);
```