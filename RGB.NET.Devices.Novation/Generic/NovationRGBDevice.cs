using System;
using System.Collections.Generic;
using RGB.NET.Core;

namespace RGB.NET.Devices.Novation;

/// <inheritdoc cref="AbstractRGBDevice{TDeviceInfo}" />
/// <summary>
/// Represents a generic Novation-device. (launchpad).
/// </summary>
public abstract class NovationRGBDevice<TDeviceInfo> : AbstractRGBDevice<TDeviceInfo>, INovationRGBDevice
    where TDeviceInfo : NovationRGBDeviceInfo
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="NovationRGBDevice{TDeviceInfo}"/> class.
    /// </summary>
    /// <param name="info">The generic information provided by Novation for the device.</param>
    /// <param name="updateTrigger">The update trigger used to update this device.</param>
    protected NovationRGBDevice(TDeviceInfo info, IDeviceUpdateTrigger updateTrigger)
        : base(info, GetUpdateQueue(updateTrigger, info))
    { }

    #endregion

    #region Methods

    private static UpdateQueue GetUpdateQueue(IDeviceUpdateTrigger updateTrigger, TDeviceInfo info) =>
        info.ColorCapabilities switch
        {
            NovationColorCapabilities.LimitedRG => new LimitedColorUpdateQueue(updateTrigger, info.DeviceId),
            NovationColorCapabilities.RGB => new RGBColorUpdateQueue(updateTrigger, info.DeviceId),
            _ => throw new ArgumentOutOfRangeException()
        };

    /// <inheritdoc />
    protected override void UpdateLeds(IEnumerable<Led> ledsToUpdate) => UpdateQueue.SetData(GetUpdateData(ledsToUpdate));

    /// <summary>
    /// Resets the <see cref="NovationRGBDevice{TDeviceInfo}"/> back to default.
    /// </summary>
    public virtual void Reset() => UpdateQueue.Reset();

    /// <inheritdoc cref="IDisposable.Dispose" />
    /// <inheritdoc cref="AbstractRGBDevice{TDeviceInfo}.Dispose" />
    public override void Dispose()
    {
        Reset();

        base.Dispose();
    }

    #endregion
}