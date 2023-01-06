using System;
using RGB.NET.Core;
using RGB.NET.Devices.Logitech.Native;

namespace RGB.NET.Devices.Logitech;

/// <summary>
/// Represents the update-queue performing updates for logitech zone devices.
/// </summary>
public class LogitechZoneUpdateQueue : UpdateQueue
{
    #region Properties & Fields

    private readonly LogitechDeviceType _deviceType;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="LogitechZoneUpdateQueue"/> class.
    /// </summary>
    /// <param name="updateTrigger">The update trigger used by this queue.</param>
    /// <param name="deviceType">The tpye of the device this queue is updating.</param>
    public LogitechZoneUpdateQueue(IDeviceUpdateTrigger updateTrigger, LogitechDeviceType deviceType)
        : base(updateTrigger)
    {
        this._deviceType = deviceType;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override void Update(in ReadOnlySpan<(object key, Color color)> dataSet)
    {
        _LogitechGSDK.LogiLedSetTargetDevice(LogitechDeviceCaps.All);

        foreach ((object key, Color color) in dataSet)
        {
            int zone = (int)key;
            _LogitechGSDK.LogiLedSetLightingForTargetZone(_deviceType, zone,
                                                          (int)MathF.Round(color.R * 100),
                                                          (int)MathF.Round(color.G * 100),
                                                          (int)MathF.Round(color.B * 100));
        }
    }

    #endregion
}