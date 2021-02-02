using System.Globalization;
using RGB.NET.Core;
using RGB.NET.Devices.Wooting.Enum;
using RGB.NET.Devices.Wooting.Generic;

namespace RGB.NET.Devices.Wooting.Keyboard
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a generic information for a <see cref="T:RGB.NET.Devices.Wooting.Keyboard.WootingKeyboardRGBDevice" />.
    /// </summary>
    public class WootingKeyboardRGBDeviceInfo : WootingRGBDeviceInfo
    {
        #region Properties & Fields

        /// <summary>
        /// Gets the <see cref="WootingPhysicalKeyboardLayout"/> of the <see cref="WootingKeyboardRGBDevice"/>.
        /// </summary>
        public WootingPhysicalKeyboardLayout PhysicalLayout { get; }

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Internal constructor of managed <see cref="T:RGB.NET.Devices.Wooting.WootingKeyboardRGBDeviceInfo" />.
        /// </summary>
        /// <param name="deviceIndex">The index of the <see cref="T:RGB.NET.Devices.Wooting.WootingKeyboardRGBDevice" />.</param>
        /// <param name="physicalKeyboardLayout">The <see cref="T:RGB.NET.Devices.Wooting.WootingPhysicalKeyboardLayout" /> of the <see cref="T:RGB.NET.Devices.Wooting.WootingKeyboardRGBDevice" />.</param>
        internal WootingKeyboardRGBDeviceInfo(WootingDevicesIndexes deviceIndex, WootingPhysicalKeyboardLayout physicalKeyboardLayout)
            : base(RGBDeviceType.Keyboard, deviceIndex)
        {
            this.PhysicalLayout = physicalKeyboardLayout;
        }

        #endregion
    }
}
