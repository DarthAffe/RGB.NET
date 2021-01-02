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

        RGBSurface? Surface { get; internal set; }

        /// <summary>
        /// Gets generic information about the <see cref="IRGBDevice"/>.
        /// </summary>
        IRGBDeviceInfo DeviceInfo { get; }

        /// <summary>
        /// Gets or sets the location of the <see cref="IRGBDevice"/>.
        /// </summary>
        Point Location { get; set; }

        /// <summary>
        /// Gets the <see cref="Size"/> of the <see cref="IRGBDevice"/>.
        /// </summary>
        Size Size { get; }

        /// <summary>
        /// Gets the actual <see cref="Size"/> of the <see cref="IRGBDevice"/>.
        /// This includes the <see cref="Scale"/>.
        /// </summary>
        Size ActualSize { get; }

        /// <summary>
        /// Gets a <see cref="Rectangle"/> representing the logical location of the <see cref="DeviceRectangle"/> relative to the <see cref="RGBSurface"/>.
        /// </summary>
        Rectangle DeviceRectangle { get; }

        /// <summary>
        /// Gets or sets the scale of the <see cref="IRGBDevice"/>.
        /// </summary>
        Scale Scale { get; set; }

        /// <summary>
        /// Gets or sets the rotation of the <see cref="IRGBDevice"/>.
        /// </summary>
        Rotation Rotation { get; set; }

        #endregion

        #region Indexer

        /// <summary>
        /// Gets the <see cref="Led"/> with the specified <see cref="LedId"/>.
        /// </summary>
        /// <param name="ledId">The <see cref="LedId"/> of the <see cref="Led"/> to get.</param>
        /// <returns>The <see cref="Led"/> with the specified <see cref="LedId"/> or null if no <see cref="Led"/> is found.</returns>
        Led? this[LedId ledId] { get; }

        /// <summary>
        /// Gets the <see cref="Led" /> at the given physical location.
        /// </summary>
        /// <param name="location">The <see cref="Point"/> to get the location from.</param>
        /// <returns>The <see cref="Led"/> at the given <see cref="Point"/> or null if no location is found.</returns>
        Led? this[Point location] { get; }

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
