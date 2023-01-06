[RGB.NET](https://github.com/DarthAffe/RGB.NET) Device-Provider-Package for Asus-Devices.

## Usage
This provider follows the default pattern and does not require additional setup.

```csharp
surface.Load(AsusDeviceProvider.Instance);
```

# Required SDK
This providers requires the `Interop.AuraServiceLib.dll` to be present on the system. Normally this dll is installed with ASUS Aura and registered in the GAC.
There are some known issue with this dll missing and it's recommended to pack it with your application. You can get it from your Aura-Installation or get a copy from [https://redist.arge.be/Asus/Interop.AuraServiceLib.dll](https://redist.arge.be/Asus/Interop.AuraServiceLib.dll).
If packed it has to be located in a spot that your application is loading runtime dlls from.
