using System;
using System.Collections.Generic;
using RGB.NET.Core;

namespace RGB.NET.Devices.Razer;

/// <inheritdoc cref="AbstractRGBDevice{RazerRGBDeviceInfo}" />
/// <inheritdoc cref="IRazerRGBDevice" />
/// <summary>
/// Represents a generic razer-device. (keyboard, mouse, headset, mousepad).
/// </summary>
public abstract class RazerRGBDevice : AbstractRGBDevice<RazerRGBDeviceInfo>, IRazerRGBDevice
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="RazerRGBDevice"/> class.
    /// </summary>
    /// <param name="info">The generic information provided by razer for the device.</param>
    /// <param name="updateQueue">The queue used to update this device.</param>
    protected RazerRGBDevice(RazerRGBDeviceInfo info, IUpdateQueue updateQueue)
        : base(info, updateQueue)
    {
        RequiresFlush = true;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override void UpdateLeds(IEnumerable<Led> ledsToUpdate) => UpdateQueue.SetData(GetUpdateData(ledsToUpdate));

    /// <inheritdoc />
    public override void Dispose()
    {
        try { UpdateQueue.Dispose(); }
        catch { /* at least we tried */ }

        base.Dispose();

        GC.SuppressFinalize(this);
    }

    #endregion
}