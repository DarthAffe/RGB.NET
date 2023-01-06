// ReSharper disable UnusedMember.Global

using System.Collections.Generic;
using System.Linq;

namespace RGB.NET.Core;

/// <summary>
/// Offers some extensions and helper-methods for the work with the surface.
/// </summary>
public static class SurfaceExtensions
{
    #region Methods

    /// <summary>
    /// Initializes the specifiec device provider and attaches all devices.
    /// </summary>
    /// <param name="surface">The surface to attach the devices to.</param>
    /// <param name="deviceProvider">The device provider to load.</param>
    /// <param name="loadFilter"><see cref="RGBDeviceType"/>-flags to filter the devices to load.</param>
    /// <param name="throwExceptions">Specifies if exceptions should be thrown or silently be ignored.</param>
    public static void Load(this RGBSurface surface, IRGBDeviceProvider deviceProvider, RGBDeviceType loadFilter = RGBDeviceType.All, bool throwExceptions = false)
    {
        if (!deviceProvider.IsInitialized)
            deviceProvider.Initialize(loadFilter, throwExceptions);

        surface.Attach(deviceProvider.Devices);
    }

    /// <summary>
    /// Attaches the specified devices to the surface.
    /// </summary>
    /// <param name="surface">The surface the devices are attached to.</param>
    /// <param name="devices">The devices to attach.</param>
    public static void Attach(this RGBSurface surface, IEnumerable<IRGBDevice> devices)
    {
        foreach (IRGBDevice device in devices)
            surface.Attach(device);
    }

    /// <summary>
    /// Detaches the specified devices from the surface.
    /// </summary>
    /// <param name="surface">The surface the devices are detached from.</param>
    /// <param name="devices">The devices to detach.</param>
    public static void Detach(this RGBSurface surface, IEnumerable<IRGBDevice> devices)
    {
        foreach (IRGBDevice device in devices)
            surface.Detach(device);
    }

    /// <summary>
    /// Gets all devices of a specific type.
    /// </summary>
    /// <typeparam name="T">The type of devices to get.</typeparam>
    /// <returns>A collection of devices with the specified type.</returns>
    public static IEnumerable<T> GetDevices<T>(this RGBSurface surface)
        where T : class
        => surface.Devices.Where(x => x is T).Cast<T>();

    /// <summary>
    /// Gets all devices of the specified <see cref="RGBDeviceType"/>.
    /// </summary>
    /// <param name="surface">The surface to get the devices from.</param>
    /// <param name="deviceType">The <see cref="RGBDeviceType"/> of the devices to get.</param>
    /// <returns>A collection of devices matching the specified <see cref="RGBDeviceType"/>.</returns>
    public static IEnumerable<IRGBDevice> GetDevices(this RGBSurface surface, RGBDeviceType deviceType)
        => surface.Devices.Where(d => deviceType.HasFlag(d.DeviceInfo.DeviceType));

    /// <summary>
    /// Automatically aligns all devices to prevent overlaps.
    /// </summary>
    public static void AlignDevices(this RGBSurface surface)
    {
        float posX = 0;
        foreach (IRGBDevice device in surface.Devices)
        {
            device.Location += new Point(posX, 0);
            posX += device.ActualSize.Width + 1;
        }
    }

    #endregion
}