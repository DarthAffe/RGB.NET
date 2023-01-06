[RGB.NET](https://github.com/DarthAffe/RGB.NET) Device-Provider-Package for msi-Devices.

## Usage
This provider follows the default pattern and does not require additional setup.

```csharp
surface.Load(MsiDeviceProvider.Instance);
```

# Required SDK
This providers requires native SDK-dlls.
You can get them directly from Msi at [https://de.msi.com/Landing/mystic-light-rgb-gaming-pc/download](https://de.msi.com/Landing/mystic-light-rgb-gaming-pc/download) (Direct Link: [https://download.msi.com/uti_exe/Mystic_light_SDK.zip](https://download.msi.com/uti_exe/Mystic_light_SDK.zip))

Since the SDK-dlls are native it's important to use the correct architecture you're building your application for. (If in doubt you can always include both.)

### x64
`MysticLight_SDK_x64.dll` from the SDK-zip needs to be distributed as `<application-directory>\x64\MysticLight_SDK.dll`

You can use other, custom paths by adding them to `MsiDeviceProvider.PossibleX64NativePaths`.

### x86
`MysticLight_SDK.dll` from the SDK-zip needs to be distributed as `<application-directory>\x86\MysticLight_SDK.dll`

You can use other, custom paths by adding them to `MsiDeviceProvider.PossibleX86NativePaths`.
