using System;
using System.Collections.Generic;
using RGB.NET.Core;

namespace RGB.NET.Devices.Novation;

/// <inheritdoc cref="NovationRGBDevice{TDeviceInfo}" />
/// <summary>
/// Represents a Novation launchpad.
/// </summary>
public class NovationLaunchpadRGBDevice : NovationRGBDevice<NovationLaunchpadRGBDeviceInfo>, ILedMatrix
{
    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Novation.NovationLaunchpadRGBDevice" /> class.
    /// </summary>
    /// <param name="info">The specific information provided by Novation for the launchpad</param>
    /// <param name="updateTrigger">The update trigger used to update this device.</param>
    internal NovationLaunchpadRGBDevice(NovationLaunchpadRGBDeviceInfo info, IDeviceUpdateTrigger updateTrigger)
        : base(info, updateTrigger)
    {
        InitializeLayout();
    }

    #endregion

    #region Methods

    private void InitializeLayout()
    {
        Dictionary<LedId, (byte mode, byte id, int x, int y)> mapping = GetDeviceMapping();

        const int BUTTON_SIZE = 20;
        foreach (LedId ledId in mapping.Keys)
        {
            (_, _, int x, int y) = mapping[ledId];
            AddLed(ledId, new Point(BUTTON_SIZE * x, BUTTON_SIZE * y), new Size(BUTTON_SIZE));
        }
    }

    /// <inheritdoc />
    // ReSharper disable RedundantCast
    protected override object GetLedCustomData(LedId ledId) => GetDeviceMapping().TryGetValue(ledId, out (byte mode, byte id, int _, int __) data) ? (data.mode, data.id) : ((byte)0x00, (byte)0x00);
    // ReSharper restore RedundantCast

    /// <summary>
    /// Gets the mapping used to access the LEDs of the device based on <see cref="NovationLaunchpadRGBDeviceInfo.LedMapping"/>.
    /// </summary>
    /// <returns>The mapping of the device.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the value of <see cref="NovationLaunchpadRGBDeviceInfo.LedMapping"/> is not known.</exception>
    protected virtual Dictionary<LedId, (byte mode, byte id, int x, int y)> GetDeviceMapping()
        => DeviceInfo.LedMapping switch
        {
            LedIdMappings.Current => LaunchpadIdMapping.CURRENT,
            LedIdMappings.Legacy => LaunchpadIdMapping.LEGACY,
            _ => throw new ArgumentOutOfRangeException()
        };

    #endregion
}