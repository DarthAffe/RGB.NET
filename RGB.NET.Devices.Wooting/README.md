[RGB.NET](https://github.com/DarthAffe/RGB.NET) Device-Provider-Package for Wooting-Devices.

## Usage
This provider follows the default pattern and does not require additional setup.

```csharp
surface.Load(WootingDeviceProvider.Instance);
```

# Required SDK
This providers requires native SDK-dlls.
You can get them directly from Wooting at [https://github.com/WootingKb/wooting-rgb-sdk/releases](https://github.com/WootingKb/wooting-rgb-sdk/releases)

Since the SDK-dlls are native it's important to use the correct architecture you're building your application for. (If in doubt you can always include both.)

### x64
`wooting-rgb-sdk.dll` from the x64-zip needs to be distributed as `<application-directory>\x64\wooting-rgb-sdk.dll`

You can use other, custom paths by adding them to `WootingDeviceProvider.PossibleX64NativePaths`.

### x86
`wooting-rgb-sdk.dll` from the x86-zip needs to be distributed as `<application-directory>\x86\wooting-rgb-sdk.dll`

You can use other, custom paths by adding them to `WootingDeviceProvider.PossibleX86NativePaths`.
