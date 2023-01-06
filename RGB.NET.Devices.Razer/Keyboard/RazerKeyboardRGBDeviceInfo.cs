// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;

namespace RGB.NET.Devices.Razer;

/// <summary>
/// Represents a generic information for a <see cref="T:RGB.NET.Devices.Razer.RazerKeyboardRGBDevice" />.
/// </summary>
public class RazerKeyboardRGBDeviceInfo : RazerRGBDeviceInfo, IKeyboardDeviceInfo
{
    #region Properties & Fields

    /// <inheritdoc />
    public KeyboardLayoutType Layout => KeyboardLayoutType.Unknown;

    #endregion

    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Internal constructor of managed <see cref="T:RGB.NET.Devices.Razer.RazerKeyboardRGBDeviceInfo" />.
    /// </summary>
    /// <param name="model">The model of the <see cref="IRGBDevice"/>.</param>
    /// <param name="endpointType">The Razer SDK endpoint type the <see cref="IRGBDevice"/> is addressed through.</param>
    internal RazerKeyboardRGBDeviceInfo(string model, RazerEndpointType endpointType)
        : base(RGBDeviceType.Keyboard, endpointType, model)
    {
    }

    #endregion
}