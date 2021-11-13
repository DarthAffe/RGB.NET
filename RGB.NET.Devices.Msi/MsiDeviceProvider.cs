// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using RGB.NET.Core;
using RGB.NET.Devices.Msi.Exceptions;
using RGB.NET.Devices.Msi.Native;

namespace RGB.NET.Devices.Msi;

/// <inheritdoc />
/// <summary>
/// Represents a device provider responsible for MSI devices.
/// </summary>
public class MsiDeviceProvider : AbstractRGBDeviceProvider
{
    #region Properties & Fields

    private static MsiDeviceProvider? _instance;
    /// <summary>
    /// Gets the singleton <see cref="MsiDeviceProvider"/> instance.
    /// </summary>
    public static MsiDeviceProvider Instance => _instance ?? new MsiDeviceProvider();

    /// <summary>
    /// Gets a modifiable list of paths used to find the native SDK-dlls for x86 applications.
    /// The first match will be used.
    /// </summary>
    public static List<string> PossibleX86NativePaths { get; } = new() { "x86/MysticLight_SDK.dll" };

    /// <summary>
    /// Gets a modifiable list of paths used to find the native SDK-dlls for x64 applications.
    /// The first match will be used.
    /// </summary>
    public static List<string> PossibleX64NativePaths { get; } = new() { "x64/MysticLight_SDK.dll" };

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="MsiDeviceProvider"/> class.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if this constructor is called even if there is already an instance of this class.</exception>
    public MsiDeviceProvider()
    {
        if (_instance != null) throw new InvalidOperationException($"There can be only one instance of type {nameof(MsiDeviceProvider)}");
        _instance = this;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override void InitializeSDK()
    {
        _MsiSDK.Reload();

        int errorCode;
        if ((errorCode = _MsiSDK.Initialize()) != 0)
            ThrowMsiError(errorCode, true);
    }

    /// <inheritdoc />
    protected override IEnumerable<IRGBDevice> LoadDevices()
    {
        int errorCode;
        if ((errorCode = _MsiSDK.GetDeviceInfo(out string[] deviceTypes, out int[] ledCounts)) != 0)
            ThrowMsiError(errorCode, true);

        for (int i = 0; i < deviceTypes.Length; i++)
        {
            string deviceType = deviceTypes[i];
            int ledCount = ledCounts[i];

            if (deviceType.Equals("MSI_MB"))
            {
                //Hex3l: MSI_MB provide access to the motherboard "leds" where a led must be intended as a led header (JRGB, JRAINBOW etc..) (Tested on MSI X570 Unify)
                yield return new MsiMainboardRGBDevice(new MsiRGBDeviceInfo(RGBDeviceType.Mainboard, deviceType, "MSI", "Motherboard"), ledCount, GetUpdateTrigger());
            }
            else if (deviceType.Equals("MSI_VGA"))
            {
                //Hex3l: Every led under MSI_VGA should be a different graphics card. Handling all the cards together seems a good way to avoid overlapping of leds
                //Hex3l: The led name is the name of the card (e.g. NVIDIA GeForce RTX 2080 Ti) we could provide it in device info.
                yield return new MsiGraphicsCardRGBDevice(new MsiRGBDeviceInfo(RGBDeviceType.GraphicsCard, deviceType, "MSI", "GraphicsCard"), ledCount, GetUpdateTrigger());
            }
            else if (deviceType.Equals("MSI_MOUSE"))
            {
                //Hex3l: Every led under MSI_MOUSE should be a different mouse. Handling all the mouses together seems a good way to avoid overlapping of leds
                //Hex3l: The led name is the name of the mouse (e.g. msi CLUTCH GM11) we could provide it in device info.
                yield return new MsiMouseRGBDevice(new MsiRGBDeviceInfo(RGBDeviceType.Mouse, deviceType, "MSI", "Mouse"), ledCount, GetUpdateTrigger());
            }
        }
    }

    private void ThrowMsiError(int errorCode, bool isCritical = false) => Throw(new MysticLightException(errorCode, _MsiSDK.GetErrorMessage(errorCode)), isCritical);

    /// <inheritdoc />
    public override void Dispose()
    {
        base.Dispose();

        try { _MsiSDK.UnloadMsiSDK(); }
        catch { /* at least we tried */ }
    }

    #endregion
}