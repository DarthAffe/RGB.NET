[RGB.NET](https://github.com/DarthAffe/RGB.NET) Device-Provider-Package for Cooler Master-Devices.

## Usage
This provider follows the default pattern and does not require additional setup.

```csharp
surface.Load(CoolerMasterDeviceProvider.Instance);
```

# Required SDK
This providers requires native SDK-dlls.   
You can get them directly from Cooler Master at [https://templates.coolermaster.com/](https://templates.coolermaster.com/) (Direct Link: [https://templates.coolermaster.com/assets/sdk/coolermaster-sdk.zip](https://templates.coolermaster.com/assets/sdk/coolermaster-sdk.zip))

Since the SDK-dlls are native it's important to use the correct architecture you're building your application for. (If in doubt you can always include both.)

### x64
`Src\SDK\x64\SDKDLL.dll` from the SDK-zip needs to be distributed as `<application-directory>\x64\CMSDK.dll`

You can use other, custom paths by adding them to `CoolerMasterDeviceProvider.PossibleX64NativePaths`.

### x86
`Src\SDK\x86\SDKDLL.dll` from the SDK-zip needs to be distributed as `<application-directory>\x86\CMSDK.dll`

You can use other, custom paths by adding them to `CoolerMasterDeviceProvider.PossibleX86NativePaths`.
