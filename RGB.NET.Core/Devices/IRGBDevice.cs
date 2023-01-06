using System;
using System.Collections.Generic;

namespace RGB.NET.Core;

/// <inheritdoc cref="IEnumerable{Led}" />
/// <inheritdoc cref="IBindable" />
/// <inheritdoc cref="IDisposable" />
/// <summary>
/// Represents a generic RGB-device.
/// </summary>
public interface IRGBDevice : IEnumerable<Led>, IPlaceable, IBindable, IDisposable
{
    #region Properties

    /// <summary>
    /// Gets the surface this device is attached to.
    /// </summary>
    RGBSurface? Surface { get; internal set; }

    /// <summary>
    /// Gets generic information about the <see cref="IRGBDevice"/>.
    /// </summary>
    IRGBDeviceInfo DeviceInfo { get; }

    /// <summary>
    /// Gets a list of color corrections applied to this device.
    /// </summary>
    IList<IColorCorrection> ColorCorrections { get; }

    #endregion

    #region Indexer

    /// <summary>
    /// Gets the <see cref="Led"/> with the specified <see cref="LedId"/>.
    /// </summary>
    /// <param name="ledId">The <see cref="LedId"/> of the <see cref="Led"/> to get.</param>
    /// <returns>The <see cref="Led"/> with the specified <see cref="LedId"/> or null if no <see cref="Led"/> is found.</returns>
    Led? this[LedId ledId] { get; }

    /// <summary>
    /// Gets the <see cref="Led" /> at the specified physical location.
    /// </summary>
    /// <param name="location">The <see cref="Point"/> to get the location from.</param>
    /// <returns>The <see cref="Led"/> at the specified <see cref="Point"/> or null if no location is found.</returns>
    Led? this[Point location] { get; }

    /// <summary>
    /// Gets a list of <see cref="Led" /> inside the specified <see cref="Rectangle"/>.
    /// </summary>
    /// <param name="referenceRect">The <see cref="Rectangle"/> to check.</param>
    /// <param name="minOverlayPercentage">The minimal percentage overlay a <see cref="Led"/> must have with the <see cref="Rectangle" /> to be taken into the list.</param>
    /// <returns>A enumerable of leds inside the specified rectangle.</returns>
    IEnumerable<Led> this[Rectangle referenceRect, double minOverlayPercentage = 0.5] { get; }

    #endregion

    #region Methods

    /// <summary>
    /// Perform an update for all dirty <see cref="Led"/>, or all <see cref="Led"/> if flushLeds is set to true.
    /// </summary>
    /// <param name="flushLeds">Specifies whether all <see cref="Led"/> (including clean ones) should be updated.</param>
    void Update(bool flushLeds = false);

    /// <summary>
    /// Adds a led to the device.
    /// </summary>
    /// <param name="ledId">The id of the led.</param>
    /// <param name="location">The location of the led on the device.</param>
    /// <param name="size">The size of the led.</param>
    /// <param name="customData">Custom data saved on the led.</param>
    /// <returns>The newly added led or <c>null</c> if a led with this id is already added.</returns>
    Led? AddLed(LedId ledId, in Point location, in Size size, object? customData = null);

    /// <summary>
    /// Removes the led with the specified id from the device.
    /// </summary>
    /// <param name="ledId">The id of the led to remove.</param>
    /// <returns>The removed led or <c>null</c> if there was no led with the specified id.</returns>
    Led? RemoveLed(LedId ledId);

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