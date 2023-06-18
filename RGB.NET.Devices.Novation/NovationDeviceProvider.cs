// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using RGB.NET.Core;
using Sanford.Multimedia.Midi;

namespace RGB.NET.Devices.Novation;

/// <inheritdoc />
/// <summary>
/// Represents a device provider responsible for Novation  devices.
/// </summary>
public sealed class NovationDeviceProvider : AbstractRGBDeviceProvider
{
    #region Properties & Fields

    private static Lazy<NovationDeviceProvider> _instance = new(LazyThreadSafetyMode.ExecutionAndPublication);
    /// <summary>
    /// Gets the singleton <see cref="NovationDeviceProvider"/> instance.
    /// </summary>
    public static NovationDeviceProvider Instance => _instance.Value;

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

            string deviceName = outCaps.name.ToUpperInvariant();
            NovationDevices? deviceId = (NovationDevices?)Enum.GetValues(typeof(NovationDevices))
                                                              .Cast<Enum>()
                                                              .Where(x => x.GetDeviceId() != null)
                                                              .FirstOrDefault(x => deviceName.Contains(x.GetDeviceId()!.ToUpperInvariant()));

            if (deviceId == null) continue;

            NovationColorCapabilities colorCapability = deviceId.GetColorCapability();
            if (colorCapability == NovationColorCapabilities.None) continue;

            yield return new NovationLaunchpadRGBDevice(new NovationLaunchpadRGBDeviceInfo(outCaps.name, index, colorCapability, deviceId.GetLedIdMapping()), GetUpdateTrigger());
        }
    }

    #endregion
}