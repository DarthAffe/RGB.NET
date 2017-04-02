// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Globalization;
using RGB.NET.Core;

namespace RGB.NET.Devices.Logitech
{
    /// <summary>
    /// Represents a generic information for a <see cref="LogitechKeyboardRGBDevice"/>.
    /// </summary>
    public class LogitechKeyboardRGBDeviceInfo : LogitechRGBDeviceInfo
    {
        #region Properties & Fields

        /// <summary>
        /// Gets the physical layout of the keyboard.
        /// </summary>
        public LogitechPhysicalKeyboardLayout PhysicalLayout { get; private set; }

        /// <summary>
        /// Gets the logical layout of the keyboard.
        /// </summary>
        public LogitechLogicalKeyboardLayout LogicalLayout { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Internal constructor of managed <see cref="LogitechKeyboardRGBDeviceInfo"/>.
        /// </summary>
        /// <param name="model">The represented device model.</param>
        /// <param name="deviceCaps">The lighting-capabilities of the device.</param>
        /// <param name="culture">The <see cref="CultureInfo"/> of the layout this keyboard is using</param>
        internal LogitechKeyboardRGBDeviceInfo(string model, LogitechDeviceCaps deviceCaps, CultureInfo culture)
            : base(RGBDeviceType.Keyboard, model, deviceCaps)
        {
            SetLayouts(culture.KeyboardLayoutId);

            Image = new Uri(PathHelper.GetAbsolutePath($@"Images\Logitech\Keyboards\{Model.Replace(" ", string.Empty).ToUpper()}.png"), UriKind.Absolute);
        }

        private void SetLayouts(int keyboardLayoutId)
        {
            switch (keyboardLayoutId)
            {
                //TODO DarthAffe 04.02.2017: Check all available keyboards and there layout-ids
                default:
                    PhysicalLayout = LogitechPhysicalKeyboardLayout.UK;
                    LogicalLayout = LogitechLogicalKeyboardLayout.DE;
                    break;
            }
        }

        #endregion
    }
}
