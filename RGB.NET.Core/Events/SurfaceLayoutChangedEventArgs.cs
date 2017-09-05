// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

using System;
using System.Collections.Generic;

namespace RGB.NET.Core
{
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
        public IEnumerable<IRGBDevice> Devices { get; }

        /// <summary>
        /// Gets a value indicating if the event is caused by the addition of a new <see cref="IRGBDevice"/> to the <see cref="RGBSurface"/>.
        /// </summary>
        public bool DeviceAdded { get; }

        /// <summary>
        /// Gets a value indicating if the event is caused by a changed location of one of the devices on the <see cref="RGBSurface"/>.
        /// </summary>
        public bool DeviceLocationChanged { get; }

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Core.SurfaceLayoutChangedEventArgs" /> class.
        /// </summary>
        /// <param name="devices">The <see cref="T:RGB.NET.Core.IRGBDevice" /> that caused the change.</param>
        /// <param name="deviceAdded">A value indicating if the event is caused by the addition of a new <see cref="T:RGB.NET.Core.IRGBDevice" /> to the <see cref="T:RGB.NET.Core.RGBSurface" />.</param>
        /// <param name="deviceLocationChanged">A value indicating if the event is caused by a changed location of one of the devices on the <see cref="T:RGB.NET.Core.RGBSurface" />.</param>
        public SurfaceLayoutChangedEventArgs(IEnumerable<IRGBDevice> devices, bool deviceAdded, bool deviceLocationChanged)
        {
            this.Devices = devices;
            this.DeviceAdded = deviceAdded;
            this.DeviceLocationChanged = deviceLocationChanged;
        }

        #endregion
    }
}
