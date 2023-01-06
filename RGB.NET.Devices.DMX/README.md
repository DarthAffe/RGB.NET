[RGB.NET](https://github.com/DarthAffe/RGB.NET) Device-Provider-Package for E1.31 DMX-Devices.

## Usage
This provider does not load anything by default and requires additional configuration to work.

```csharp
// Add as many DMX devices as you like
DMXDeviceProvider.Instance.AddDeviceDefinition(new E131DMXDeviceDefinition("<hostname>"));

surface.Load(DMXDeviceProvider.Instance);
```

You can also configure additional things like device names, custom ports, heartbeats and so on in the DeviceDefinition.

# Required SDK
This provider does not require an additional SDK.
