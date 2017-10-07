using System;
using System.Globalization;
using RGB.NET.Core;

namespace RGB.NET.Devices.Aura
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a generic information for a <see cref="T:RGB.NET.Devices.Aura.AuraKeyboardRGBDevice" />.
    /// </summary>
    public class AuraKeyboardRGBDeviceInfo : AuraRGBDeviceInfo
    {
        #region Properties & Fields
        
        /// <summary>
        /// Gets the physical layout of the keyboard.
        /// </summary>
        public AuraPhysicalKeyboardLayout PhysicalLayout { get; private set; }

        /// <summary>
        /// Gets the logical layout of the keyboard.
        /// </summary>
        public AuraLogicalKeyboardLayout LogicalLayout { get; private set; }

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Internal constructor of managed <see cref="T:RGB.NET.Devices.Aura.AuraKeyboardRGBDeviceInfo" />.
        /// </summary>
        /// <param name="deviceType">The type of the <see cref="IRGBDevice"/>.</param>
        /// <param name="handle">The handle of the <see cref="IRGBDevice"/>.</param>
        /// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> of the layout this keyboard is using</param>
        internal AuraKeyboardRGBDeviceInfo(RGBDeviceType deviceType, IntPtr handle, CultureInfo culture)
            : base(deviceType, handle, "Asus", "Claymore")
        {
            SetLayouts(culture.KeyboardLayoutId);

            Image = new Uri(PathHelper.GetAbsolutePath($@"Images\Aura\Keyboards\{Model.Replace(" ", string.Empty).ToUpper()}.png"), UriKind.Absolute);
        }

        #endregion

        #region Methods
        
        private void SetLayouts(int keyboardLayoutId)
        {
            switch (keyboardLayoutId)
            {
                //TODO DarthAffe 07.10.2017: Implement
                default:
                    PhysicalLayout = AuraPhysicalKeyboardLayout.TODO;
                    LogicalLayout = AuraLogicalKeyboardLayout.TODO;
                    break;
            }
        }

        #endregion
    }
}
