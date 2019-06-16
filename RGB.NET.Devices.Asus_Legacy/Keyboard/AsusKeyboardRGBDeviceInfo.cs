using System;
using System.Globalization;
using RGB.NET.Core;

namespace RGB.NET.Devices.Asus
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a generic information for a <see cref="T:RGB.NET.Devices.Asus.AsusKeyboardRGBDevice" />.
    /// </summary>
    public class AsusKeyboardRGBDeviceInfo : AsusRGBDeviceInfo
    {
        #region Properties & Fields

        /// <inheritdoc />
        public override bool SupportsSyncBack => false;

        /// <summary>
        /// Gets the physical layout of the keyboard.
        /// </summary>
        public AsusPhysicalKeyboardLayout PhysicalLayout { get; private set; }

        /// <summary>
        /// Gets the logical layout of the keyboard.
        /// </summary>
        public AsusLogicalKeyboardLayout LogicalLayout { get; private set; }

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Internal constructor of managed <see cref="T:RGB.NET.Devices.Asus.AsusKeyboardRGBDeviceInfo" />.
        /// </summary>
        /// <param name="deviceType">The type of the <see cref="IRGBDevice"/>.</param>
        /// <param name="handle">The handle of the <see cref="IRGBDevice"/>.</param>
        /// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> of the layout this keyboard is using.</param>
        internal AsusKeyboardRGBDeviceInfo(RGBDeviceType deviceType, IntPtr handle, CultureInfo culture)
            : base(deviceType, handle, "Claymore")
        {
            SetLayouts(culture.KeyboardLayoutId);
        }

        #endregion

        #region Methods

        private void SetLayouts(int keyboardLayoutId)
        {
            switch (keyboardLayoutId)
            {
                //TODO DarthAffe 07.10.2017: Implement
                default:
                    PhysicalLayout = AsusPhysicalKeyboardLayout.TODO;
                    LogicalLayout = AsusLogicalKeyboardLayout.TODO;
                    break;
            }
        }

        #endregion
    }
}
