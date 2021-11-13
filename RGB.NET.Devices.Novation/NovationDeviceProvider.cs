// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.Linq;
using RGB.NET.Core;
using Sanford.Multimedia.Midi;

namespace RGB.NET.Devices.Novation;

/// <inheritdoc />
/// <summary>
/// Represents a device provider responsible for Novation  devices.
/// </summary>
public class NovationDeviceProvider : AbstractRGBDeviceProvider
{
    #region Properties & Fields

    private static NovationDeviceProvider? _instance;
    /// <summary>
    /// Gets the singleton <see cref="NovationDeviceProvider"/> instance.
    /// </summary>
    public static NovationDeviceProvider Instance => _instance ?? new NovationDeviceProvider();

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="NovationDeviceProvider"/> class.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if this constructor is called even if there is already an instance of this class.</exception>
    private NovationDeviceProvider()
    {
        if (_instance != null) throw new InvalidOperationException($"There can be only one instance of type {nameof(NovationDeviceProvider)}");
        _instance = this;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override void InitializeSDK() { }

    /// <inheritdoc />
    protected override IEnumerable<IRGBDevice> LoadDevices()
    {
        for (int index = 0; index < OutputDeviceBase.DeviceCount; index++)
        {
            MidiOutCaps outCaps = OutputDeviceBase.GetDeviceCapabilities(index);
            if (outCaps.name == null) continue;

            NovationDevices? deviceId = (NovationDevices?)Enum.GetValues(typeof(NovationDevices))
                                                              .Cast<Enum>()
                                                              .FirstOrDefault(x => x.GetDeviceId()?.ToUpperInvariant().Contains(outCaps.name.ToUpperInvariant()) ?? false);

            if (deviceId == null) continue;

            NovationColorCapabilities colorCapability = deviceId.GetColorCapability();
            if (colorCapability == NovationColorCapabilities.None) continue;

            yield return new NovationLaunchpadRGBDevice(new NovationLaunchpadRGBDeviceInfo(outCaps.name, index, colorCapability, deviceId.GetLedIdMapping()), GetUpdateTrigger());
        }
    }

    #endregion
}