using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using RGB.NET.Core;
using RGB.NET.Devices.Wooting.Generic;
using RGB.NET.Devices.Wooting.Keyboard;
using RGB.NET.Devices.Wooting.Native;

namespace RGB.NET.Devices.Wooting;

/// <inheritdoc />
/// <summary>
/// Represents a device provider responsible for Wooting devices.
/// </summary>
public class WootingDeviceProvider : AbstractRGBDeviceProvider
{
    #region Properties & Fields

    private static WootingDeviceProvider? _instance;
    /// <summary>
    /// Gets the singleton <see cref="WootingDeviceProvider"/> instance.
    /// </summary>
    public static WootingDeviceProvider Instance => _instance ?? new WootingDeviceProvider();

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
    public static List<string> PossibleNativePathsMacOS { get; } = new() { "x64/libwooting-rgb-sdk.dylib" };

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="WootingDeviceProvider"/> class.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if this constructor is called even if there is already an instance of this class.</exception>
    public WootingDeviceProvider()
    {
        if (_instance != null) throw new InvalidOperationException($"There can be only one instance of type {nameof(WootingDeviceProvider)}");
        _instance = this;
    }

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

        GC.SuppressFinalize(this);
    }

    #endregion
}