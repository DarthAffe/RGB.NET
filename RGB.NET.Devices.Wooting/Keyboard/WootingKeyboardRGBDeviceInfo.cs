using System;
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

        /// <summary>
        /// Gets the <see cref="WootingLogicalKeyboardLayout"/> of the <see cref="WootingKeyboardRGBDevice"/>.
        /// </summary>
        public WootingLogicalKeyboardLayout LogicalLayout { get; private set; }

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Internal constructor of managed <see cref="T:RGB.NET.Devices.Wooting.WootingKeyboardRGBDeviceInfo" />.
        /// </summary>
        /// <param name="deviceIndex">The index of the <see cref="T:RGB.NET.Devices.Wooting.WootingKeyboardRGBDevice" />.</param>
        /// <param name="physicalKeyboardLayout">The <see cref="T:RGB.NET.Devices.Wooting.WootingPhysicalKeyboardLayout" /> of the <see cref="T:RGB.NET.Devices.Wooting.WootingKeyboardRGBDevice" />.</param>
        /// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> of the layout this keyboard is using</param>
        internal WootingKeyboardRGBDeviceInfo(WootingDevicesIndexes deviceIndex, WootingPhysicalKeyboardLayout physicalKeyboardLayout, CultureInfo culture)
                : base(RGBDeviceType.Keyboard, deviceIndex)
        {
            this.PhysicalLayout = physicalKeyboardLayout;

            // For now just go for this
            switch (physicalKeyboardLayout)
            {
                case WootingPhysicalKeyboardLayout.US:
                    this.LogicalLayout = WootingLogicalKeyboardLayout.US;
                    break;
                case WootingPhysicalKeyboardLayout.EU:
                    this.LogicalLayout = WootingLogicalKeyboardLayout.EU;
                    break;
            }
        }
        
        #endregion
    }
}
