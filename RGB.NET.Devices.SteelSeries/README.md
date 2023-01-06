[RGB.NET](https://github.com/DarthAffe/RGB.NET) Device-Provider-Package for Steel Series-Devices.

## Usage
This provider follows the default pattern and does not require additional setup.

```csharp
surface.Load(SteelSeriesDeviceProvider.Instance);
```

Since the Steel Series SDK does not provide device information only known devices will work.   
You can add detection for additional devices by adding entires for them to the respective static `DeviceDefinitions` on the `SteelSeriesDeviceProvider`.

# Required SDK
This provider does not require an additional SDK.
