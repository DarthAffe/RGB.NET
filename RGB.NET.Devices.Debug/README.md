[RGB.NET](https://github.com/DarthAffe/RGB.NET) Device-Provider-Package for Debug-Devices.

> This provider is only for debug purposes!

## Usage
This provider does not load anything by default and requires additional configuration to work.

```csharp
// Add as many debug devices as you like
DebugDeviceProvider.Instance.AddFakeDeviceDefinition(DeviceLayout.Load("<Path to layout-file>"));

surface.Load(DebugDeviceProvider.Instance);
```

# Required SDK
This provider does not require an additional SDK.
