// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

using System;

namespace RGB.NET.Core
{
    /// <summary>
    /// Represents the information supplied with an <see cref="RGBSurface.SurfaceLayoutChanged"/>-event.
    /// </summary>
    public class SurfaceLayoutChangedEventArgs : EventArgs
    {
        #region Properties & Fields

        /// <summary>
        /// Gets the <see cref="IRGBDevice"/> that caused the change. Returns null if the change isn't caused by a <see cref="IRGBDevice"/>.
        /// </summary>
        public IRGBDevice Device { get; }

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

        /// <summary>
        /// Initializes a new instance of the <see cref="SurfaceLayoutChangedEventArgs"/> class.
        /// </summary>
        /// <param name="device">The <see cref="IRGBDevice"/> that caused the change</param>
        /// <param name="deviceAdded">A value indicating if the event is caused by the addition of a new <see cref="IRGBDevice"/> to the <see cref="RGBSurface"/>.</param>
        /// <param name="deviceLocationChanged">A value indicating if the event is caused by a changed location of one of the devices on the <see cref="RGBSurface"/>.</param>
        public SurfaceLayoutChangedEventArgs(IRGBDevice device, bool deviceAdded, bool deviceLocationChanged)
        {
            this.Device = device;
            this.DeviceAdded = deviceAdded;
            this.DeviceLocationChanged = deviceLocationChanged;
        }

        #endregion
    }
}
