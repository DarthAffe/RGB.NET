[RGB.NET](https://github.com/DarthAffe/RGB.NET) Device-Provider-Package for [WLED](https://kno.wled.ge/)-devices.

## Usage
This provider does not load anything by default and requires additional configuration to work.

```csharp
// Add as many WLED-devices as you like
WledDeviceProvider.Instance.AddDeviceDefinition(new WledDeviceDefinition("<hostname>"));

surface.Load(WledDeviceProvider.Instance);
```

You can also override the manufacturer and device model in the DeviceDefinition.

# Required SDK
This provider does not require an additional SDK.

UDP realtime needs to be enabled on the WLED device.