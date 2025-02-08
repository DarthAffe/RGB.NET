﻿using System;
using RGB.NET.Core;
using RGB.NET.Devices.Logitech.Native;

namespace RGB.NET.Devices.Logitech;

/// <inheritdoc />
/// <summary>
/// Represents the update-queue performing updates for logitech per-device devices.
/// </summary>
public sealed class LogitechPerDeviceUpdateQueue : UpdateQueue
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="LogitechPerDeviceUpdateQueue"/> class.
    /// </summary>
    /// <param name="updateTrigger">The update trigger used by this queue.</param>
    public LogitechPerDeviceUpdateQueue(IDeviceUpdateTrigger updateTrigger)
        : base(updateTrigger)
    { }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override bool Update(ReadOnlySpan<(object key, Color color)> dataSet)
    {
        try
        {
            Color color = dataSet[0].color;

            _LogitechGSDK.LogiLedSetTargetDevice(LogitechDeviceCaps.DeviceRGB);
            _LogitechGSDK.LogiLedSetLighting((int)Math.Round(color.R * 100),
                                             (int)Math.Round(color.G * 100),
                                             (int)Math.Round(color.B * 100));

            return true;
        }
        catch (Exception ex)
        {
            LogitechDeviceProvider.Instance.Throw(ex);
        }

        return false;
    }

    #endregion
}