using System;
using RGB.NET.Core;
using RGB.NET.Devices.Logitech.Native;

namespace RGB.NET.Devices.Logitech;

/// <summary>
/// Represents the update-queue performing updates for logitech per-key devices.
/// </summary>
public class LogitechPerKeyUpdateQueue : UpdateQueue
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="LogitechPerKeyUpdateQueue"/> class.
    /// </summary>
    /// <param name="updateTrigger">The update trigger used by this queue.</param>
    public LogitechPerKeyUpdateQueue(IDeviceUpdateTrigger updateTrigger)
        : base(updateTrigger)
    {
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override void Update(in ReadOnlySpan<(object key, Color color)> dataSet)
    {
        _LogitechGSDK.LogiLedSetTargetDevice(LogitechDeviceCaps.PerKeyRGB);

        foreach ((object key, Color color) in dataSet)
        {
            // These will be LogitechLedId but the SDK expects an int and doesn't care about invalid values
            int keyName = (int)key;
            _LogitechGSDK.LogiLedSetLightingForKeyWithKeyName(keyName,
                                                              (int)MathF.Round(color.R * 100),
                                                              (int)MathF.Round(color.G * 100),
                                                              (int)MathF.Round(color.B * 100));
        }
    }

    #endregion
}