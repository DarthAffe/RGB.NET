using System;
using System.Collections.Generic;

namespace RGB.NET.Core
{
    /// <inheritdoc cref="IEnumerable{T}" />
    /// <inheritdoc cref="IBindable" />
    /// <inheritdoc cref="IDisposable" />
    /// <summary>
    /// Represents a generic RGB-device.
    /// </summary>
    public interface IRGBDevice : IEnumerable<Led>, IBindable, IDisposable
    {
        #region Properties

        /// <summary>
        /// Gets generic information about the <see cref="IRGBDevice"/>.
        /// </summary>
        IRGBDeviceInfo DeviceInfo { get; }

        /// <summary>
        /// Gets or sets the location of the <see cref="IRGBDevice"/>.
        /// </summary>
        Point Location { get; set; }

        /// <summary>
        /// Gets a copy of the <see cref="Size"/> of the whole <see cref="IRGBDevice"/>.
        /// </summary>
        Size Size { get; }

        /// <summary>
        /// Gets or sets the <see cref="DeviceUpdateMode"/> of the <see cref="IRGBDevice"/>.
        /// </summary>
        DeviceUpdateMode UpdateMode { get; set; }

        #endregion

        #region Indexer

        /// <summary>
        /// Gets the <see cref="Led"/> with the specified <see cref="LedId"/>.
        /// </summary>
        /// <param name="ledId">The <see cref="LedId"/> of the <see cref="Led"/> to get.</param>
        /// <returns>The <see cref="Led"/> with the specified <see cref="LedId"/> or null if no <see cref="Led"/> is found.</returns>
        Led this[LedId ledId] { get; }

        /// <summary>
        /// Gets the <see cref="Led" /> at the given physical location.
        /// </summary>
        /// <param name="location">The <see cref="Point"/> to get the location from.</param>
        /// <returns>The <see cref="Led"/> at the given <see cref="Point"/> or null if no location is found.</returns>
        Led this[Point location] { get; }

        /// <summary>
        /// Gets a list of <see cref="Led" /> inside the given <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="referenceRect">The <see cref="Rectangle"/> to check.</param>
        /// <param name="minOverlayPercentage">The minimal percentage overlay a <see cref="Led"/> must have with the <see cref="Rectangle" /> to be taken into the list.</param>
        /// <returns></returns>
        IEnumerable<Led> this[Rectangle referenceRect, double minOverlayPercentage = 0.5] { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Perform an update for all dirty <see cref="Led"/>, or all <see cref="Led"/> if flushLeds is set to true.
        /// </summary>
        /// <param name="flushLeds">Specifies whether all <see cref="Led"/> (including clean ones) should be updated.</param>
        void Update(bool flushLeds = false);

        /// <summary>
        /// Synchronizes the internal state of the device to the real (physical) state.
        /// This isn't supported by all devices! Check <see cref="IRGBDeviceInfo.SupportsSyncBack"/> to see if it's supported or not.
        /// </summary>
        void SyncBack();

        /// <summary>
        /// Adds the given <see cref="IRGBDeviceSpecialPart"/> to the device.
        /// This will override existing <see cref="IRGBDeviceSpecialPart"/> of the same type.
        /// </summary>
        /// <param name="specialDevicePart">The <see cref="IRGBDeviceSpecialPart"/> to add.</param>
        /// <typeparam name="T">The generic typeof of the <see cref="IRGBDeviceSpecialPart"/> to add.</typeparam>
        void AddSpecialDevicePart<T>(T specialDevicePart) where T : class, IRGBDeviceSpecialPart;

        /// <summary>
        /// Gets the requested <see cref="IRGBDeviceSpecialPart"/> if available on this <see cref="IRGBDevice"/>.
        /// </summary>
        /// <typeparam name="T">The generic type of the requested <see cref="IRGBDeviceSpecialPart"/>.</typeparam>
        /// <returns>The requested <see cref="IRGBDeviceSpecialPart"/> or null if not available in this <see cref="IRGBDevice"/>.</returns>
        T GetSpecialDevicePart<T>() where T : class, IRGBDeviceSpecialPart;

        #endregion
    }

    /// <inheritdoc />
    /// <summary>
    /// Represents a generic RGB-device with an known device-info type.
    /// </summary>
    public interface IRGBDevice<out TDeviceInfo> : IRGBDevice
        where TDeviceInfo : IRGBDeviceInfo
    {
        /// <summary>
        /// Gets generic information about the <see cref="IRGBDevice"/>.
        /// </summary>
        new TDeviceInfo DeviceInfo { get; }
    }
}
