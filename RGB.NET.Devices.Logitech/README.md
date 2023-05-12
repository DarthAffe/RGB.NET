[RGB.NET](https://github.com/DarthAffe/RGB.NET) Device-Provider-Package for Logitech-Devices.

## Usage
This provider follows the default pattern and does not require additional setup.

```csharp
surface.Load(LogitechDeviceProvider.Instance);
```

Since the logitech SDK does not provide device information only known devices will work.   
You can add detection for additional devices by adding entires for them to the respective static `DeviceDefinitions` on the `LogitechDeviceProvider`.

# Required SDK
This providers requires native SDK-dlls.   
You can get them directly from Logitech at [https://www.logitechg.com/en-us/innovation/developer-lab.html](https://www.logitechg.com/en-us/innovation/developer-lab.html) (Direct Link: [https://www.logitechg.com/sdk/LED_SDK_9.00.zip](https://www.logitechg.com/sdk/LED_SDK_9.00.zip))

Since the SDK-dlls are native it's important to use the correct architecture you're building your application for. (If in doubt you can always include both.)

### x64
`Lib\LogitechLedEnginesWrapper\x64\LogitechLedEnginesWrapper.dll` from the SDK-zip needs to be distributed as `<application-directory>\x64\LogitechLedEnginesWrapper.dll`

You can use other, custom paths by adding them to `LogitechDeviceProvider.PossibleX64NativePaths`.

### x86
`Lib\LogitechLedEnginesWrapper\x86\LogitechLedEnginesWrapper.dll` from the SDK-zip needs to be distributed as `<application-directory>\x86\LogitechLedEnginesWrapper.dll`

You can use other, custom paths by adding them to `LogitechDeviceProvider.PossibleX86NativePaths`.
