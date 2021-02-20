using AuraServiceLib;
using RGB.NET.Core;

namespace RGB.NET.Devices.Asus
{
    /// <summary>
    /// Represents a generic information for a <see cref="T:RGB.NET.Devices.Asus.AsusKeyboardRGBDevice" />.
    /// </summary>
    public class AsusKeyboardRGBDeviceInfo : AsusRGBDeviceInfo, IKeyboardDeviceInfo
    {
        #region Properties & Fields

        /// <inheritdoc />
        public KeyboardLayoutType Layout => KeyboardLayoutType.Unknown;

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Internal constructor of managed <see cref="T:RGB.NET.Devices.Asus.AsusKeyboardRGBDeviceInfo" />.
        /// </summary>
        /// <param name="device">The <see cref="IAuraSyncDevice"/> backing this RGB.NET device.</param>
        internal AsusKeyboardRGBDeviceInfo(IAuraSyncDevice device)
            : base(RGBDeviceType.Keyboard, device, device.Name)
        { }

        #endregion
    }
}
