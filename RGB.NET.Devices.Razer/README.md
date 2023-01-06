[RGB.NET](https://github.com/DarthAffe/RGB.NET) Device-Provider-Package for Razer-Devices.

## Usage
This provider follows the default pattern and does not require additional setup.

```csharp
surface.Load(RazerDeviceProvider.Instance);
```

Since the Razer SDK does not provide device information only known devices will work.
You can add detection for additional devices by adding entires for them to the respective static `DeviceDefinitions` on the `RazerDeviceProvider`.

# Required SDK
The SDK needs to be installed inside Synapse by the user and is not redistributed.
