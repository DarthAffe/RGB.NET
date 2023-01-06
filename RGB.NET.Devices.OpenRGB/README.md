[RGB.NET](https://github.com/DarthAffe/RGB.NET) Device-Provider-Package for E1.31 DMX-Devices.

## Usage
This provider does not load anything by default and requires additional configuration to work.

```csharp
// Add as many OpenRGB-instances as you like
OpenRGBDeviceProvider.Instance.AddDeviceDefinition(new OpenRGBServerDefinition());

surface.Attach(OpenRGBDeviceProvider.Instance.Devices);
```

You can also configure additional things like IP and Port in the ServerDefinition.

# Required SDK
This provider does not require an additional SDK.
