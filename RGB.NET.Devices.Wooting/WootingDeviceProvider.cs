using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using RGB.NET.Core;
using RGB.NET.Devices.Wooting.Generic;
using RGB.NET.Devices.Wooting.Keyboard;
using RGB.NET.Devices.Wooting.Native;

namespace RGB.NET.Devices.Wooting;

/// <inheritdoc />
/// <summary>
/// Represents a device provider responsible for Wooting devices.
/// </summary>
public sealed class WootingDeviceProvider : AbstractRGBDeviceProvider
{
    #region Properties & Fields

    private static Lazy<WootingDeviceProvider> _instance = new(LazyThreadSafetyMode.ExecutionAndPublication);
    /// <summary>
    /// Gets the singleton <see cref="WootingDeviceProvider"/> instance.
    /// </summary>
    public static WootingDeviceProvider Instance => _instance.Value;

    /// <summary>
    /// Gets a modifiable list of paths used to find the native SDK-dlls for x86 windows applications.
    /// The first match will be used.
    /// </summary>
    public static List<string> PossibleX86NativePathsWindows { get; } = new() { "x86/wooting-rgb-sdk.dll" };

    /// <summary>
    /// Gets a modifiable list of paths used to find the native SDK-dlls for x64 windows applications.
    /// The first match will be used.
    /// </summary>
    public static List<string> PossibleX64NativePathsWindows { get; } = new() { "x64/wooting-rgb-sdk64.dll" };

    /// <summary>
    /// Gets a modifiable list of paths used to find the native SDK-dlls for x64 linux applications.
    /// The first match will be used.
    /// </summary>
    public static List<string> PossibleNativePathsLinux { get; } = new() { "x64/libwooting-rgb-sdk.so" };

    /// <summary>
    /// Gets a modifiable list of paths used to find the native SDK-dlls for x64 MacOS applications.
    /// The first match will be used.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static List<string> PossibleNativePathsMacOS { get; } = new() { "x64/libwooting-rgb-sdk.dylib" };

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override void InitializeSDK()
    {
        lock (_WootingSDK.SdkLock)
        {
            _WootingSDK.Reload();
        }
    }

    /// <inheritdoc />
    protected override IEnumerable<IRGBDevice> LoadDevices()
    {
        lock (_WootingSDK.SdkLock)
        {
            if (_WootingSDK.KeyboardConnected())
            {
                for (byte i = 0; i < _WootingSDK.GetDeviceCount(); i++)
                {
                    WootingUpdateQueue updateQueue = new(GetUpdateTrigger(), i);
                    _WootingSDK.SelectDevice(i);
                    _WootingDeviceInfo nativeDeviceInfo = (_WootingDeviceInfo)Marshal.PtrToStructure(_WootingSDK.GetDeviceInfo(), typeof(_WootingDeviceInfo))!;

                    yield return new WootingKeyboardRGBDevice(new WootingKeyboardRGBDeviceInfo(nativeDeviceInfo, i), updateQueue);
                }
            }
        }
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        base.Dispose();

        lock (_WootingSDK.SdkLock)
        {
            try { _WootingSDK.UnloadWootingSDK(); }
            catch { /* at least we tried */ }
        }
    }

    #endregion
}