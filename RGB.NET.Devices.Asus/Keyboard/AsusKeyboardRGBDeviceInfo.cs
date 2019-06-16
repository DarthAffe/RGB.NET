using System.Globalization;
using AuraServiceLib;
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
        /// <param name="device">The <see cref="IAuraSyncDevice"/> backing this RGB.NET device.</param>
        /// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> of the layout this keyboard is using.</param>
        internal AsusKeyboardRGBDeviceInfo(IAuraSyncDevice device, CultureInfo culture)
            : base(RGBDeviceType.Keyboard, device, "Claymore")
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
