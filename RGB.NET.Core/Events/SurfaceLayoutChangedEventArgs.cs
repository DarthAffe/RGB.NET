// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

using System;

namespace RGB.NET.Core;

/// <inheritdoc />
/// <summary>
/// Represents the information supplied with an <see cref="E:RGB.NET.Core.RGBSurface.SurfaceLayoutChanged" />-event.
/// </summary>
public class SurfaceLayoutChangedEventArgs : EventArgs
{
    #region Properties & Fields

    /// <summary>
    /// Gets the <see cref="IRGBDevice"/> that caused the change. Returns null if the change isn't caused by a <see cref="IRGBDevice"/>.
    /// </summary>
    public IRGBDevice? Devices { get; }

    /// <summary>
    /// Gets a value indicating if the event is caused by the addition of a new <see cref="IRGBDevice"/> to the <see cref="RGBSurface"/>.
    /// </summary>
    public bool DeviceAdded { get; }

    /// <summary>
    /// Gets a value indicating if the event is caused by the removal of a <see cref="IRGBDevice"/> to the <see cref="RGBSurface"/>.
    /// </summary>
    public bool DeviceRemoved { get; }

    /// <summary>
    /// Gets a value indicating if the event is caused by a changed location or size of one of the <see cref="IRGBDevice"/> on the <see cref="RGBSurface"/>.
    /// </summary>
    public bool DeviceChanged { get; }

    #endregion

    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Core.SurfaceLayoutChangedEventArgs" /> class.
    /// </summary>
    /// <param name="devices">The <see cref="T:RGB.NET.Core.IRGBDevice" /> that caused the change.</param>
    /// <param name="deviceAdded">A value indicating if the event is caused by the addition of a new <see cref="T:RGB.NET.Core.IRGBDevice" /> to the <see cref="T:RGB.NET.Core.RGBSurface" />.</param>
    /// <param name="deviceRemoved">A value indicating if the event is caused by the removal of a <see cref="T:RGB.NET.Core.IRGBDevice" /> from the <see cref="T:RGB.NET.Core.RGBSurface" />.</param>
    /// <param name="deviceChanged">A value indicating if the event is caused by a change to a <see cref="T:RGB.NET.Core.IRGBDevice" /> on the <see cref="T:RGB.NET.Core.RGBSurface" />.</param>
    private SurfaceLayoutChangedEventArgs(IRGBDevice? devices, bool deviceAdded, bool deviceRemoved, bool deviceChanged)
    {
        this.Devices = devices;
        this.DeviceAdded = deviceAdded;
        this.DeviceRemoved = deviceRemoved;
        this.DeviceChanged = deviceChanged;
    }

    #endregion

    #region Factory

    internal static SurfaceLayoutChangedEventArgs FromAddedDevice(IRGBDevice device) => new(device, true, false, false);
    internal static SurfaceLayoutChangedEventArgs FromRemovedDevice(IRGBDevice device) => new(device, false, true, false);
    internal static SurfaceLayoutChangedEventArgs FromChangedDevice(IRGBDevice device) => new(device, false, false, true);
    internal static SurfaceLayoutChangedEventArgs Misc() => new(null, false, false, false);

    #endregion
}