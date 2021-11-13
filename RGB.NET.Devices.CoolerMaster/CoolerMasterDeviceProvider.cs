// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using RGB.NET.Core;
using RGB.NET.Devices.CoolerMaster.Helper;
using RGB.NET.Devices.CoolerMaster.Native;

namespace RGB.NET.Devices.CoolerMaster;

/// <inheritdoc />
/// <summary>
/// Represents a device provider responsible for Cooler Master devices.
/// </summary>
public class CoolerMasterDeviceProvider : AbstractRGBDeviceProvider
{
    #region Properties & Fields

    private static CoolerMasterDeviceProvider? _instance;
    /// <summary>
    /// Gets the singleton <see cref="CoolerMasterDeviceProvider"/> instance.
    /// </summary>
    public static CoolerMasterDeviceProvider Instance => _instance ?? new CoolerMasterDeviceProvider();

    /// <summary>
    /// Gets a modifiable list of paths used to find the native SDK-dlls for x86 applications.
    /// The first match will be used.
    /// </summary>
    public static List<string> PossibleX86NativePaths { get; } = new() { "x86/CMSDK.dll" };

    /// <summary>
    /// Gets a modifiable list of paths used to find the native SDK-dlls for x64 applications.
    /// The first match will be used.
    /// </summary>
    public static List<string> PossibleX64NativePaths { get; } = new() { "x64/CMSDK.dll" };

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CoolerMasterDeviceProvider"/> class.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if this constructor is called even if there is already an instance of this class.</exception>
    public CoolerMasterDeviceProvider()
    {
        if (_instance != null) throw new InvalidOperationException($"There can be only one instance of type {nameof(CoolerMasterDeviceProvider)}");
        _instance = this;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override void InitializeSDK()
    {
        _CoolerMasterSDK.Reload();
        if (_CoolerMasterSDK.GetSDKVersion() <= 0) Throw(new RGBDeviceException("Failed to initialize CoolerMaster-SDK"), true);
    }

    /// <inheritdoc />
    protected override IEnumerable<IRGBDevice> LoadDevices()
    {
        foreach (CoolerMasterDevicesIndexes index in Enum.GetValues(typeof(CoolerMasterDevicesIndexes)))
        {
            RGBDeviceType deviceType = index.GetDeviceType();
            if (deviceType == RGBDeviceType.None) continue;

            if (_CoolerMasterSDK.IsDevicePlugged(index))
            {
                if (!_CoolerMasterSDK.EnableLedControl(true, index))
                    Throw(new RGBDeviceException("Failed to enable LED control for device " + index));
                else
                {
                    switch (deviceType)
                    {
                        case RGBDeviceType.Keyboard:
                            yield return new CoolerMasterKeyboardRGBDevice(new CoolerMasterKeyboardRGBDeviceInfo(index, _CoolerMasterSDK.GetDeviceLayout(index)), GetUpdateTrigger());
                            break;

                        case RGBDeviceType.Mouse:
                            yield return new CoolerMasterMouseRGBDevice(new CoolerMasterMouseRGBDeviceInfo(index), GetUpdateTrigger());
                            break;

                        default:
                            Throw(new RGBDeviceException("Unknown Device-Type"));
                            break;
                    }
                }
            }
        }
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        base.Dispose();

        try { _CoolerMasterSDK.Reload(); }
        catch { /* Unlucky.. */ }
    }

    #endregion
}