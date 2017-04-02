using System;
using System.Globalization;
using RGB.NET.Core;
using RGB.NET.Devices.CoolerMaster.Helper;

namespace RGB.NET.Devices.CoolerMaster
{
    /// <summary>
    /// Represents a generic information for a <see cref="CoolerMasterKeyboardRGBDevice"/>.
    /// </summary>
    public class CoolerMasterKeyboardRGBDeviceInfo : CoolerMasterRGBDeviceInfo
    {
        #region Properties & Fields

        /// <summary>
        /// Gets the <see cref="CoolerMasterPhysicalKeyboardLayout"/> of the <see cref="CoolerMasterKeyboardRGBDevice"/>.
        /// </summary>
        public CoolerMasterPhysicalKeyboardLayout PhysicalLayout { get; private set; }

        /// <summary>
        /// Gets the <see cref="CoolerMasterLogicalKeyboardLayout"/> of the <see cref="CoolerMasterKeyboardRGBDevice"/>.
        /// </summary>
        public CoolerMasterLogicalKeyboardLayout LogicalLayout { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Internal constructor of managed <see cref="CoolerMasterKeyboardRGBDeviceInfo"/>.
        /// </summary>
        /// <param name="deviceIndex">The index of the <see cref="CoolerMasterKeyboardRGBDevice"/>.</param>
        /// <param name="physicalKeyboardLayout">The <see cref="CoolerMasterPhysicalKeyboardLayout" /> of the <see cref="CoolerMasterKeyboardRGBDevice"/>.</param>
        /// <param name="culture">The <see cref="CultureInfo"/> of the layout this keyboard is using</param>
        internal CoolerMasterKeyboardRGBDeviceInfo(CoolerMasterDevicesIndexes deviceIndex, CoolerMasterPhysicalKeyboardLayout physicalKeyboardLayout, CultureInfo culture)
                : base(RGBDeviceType.Keyboard, deviceIndex)
        {
            this.PhysicalLayout = physicalKeyboardLayout;

            SetLayouts(culture.KeyboardLayoutId);

            Image = new Uri(PathHelper.GetAbsolutePath($@"Images\Logitech\Keyboards\{Model.Replace(" ", string.Empty).ToUpper()}.png"), UriKind.Absolute);
        }

        private void SetLayouts(int keyboardLayoutId)
        {
            switch (keyboardLayoutId)
            {
                //TODO DarthAffe 02.04.2017: Check all available keyboards and there layout-ids
                default:
                    LogicalLayout = CoolerMasterLogicalKeyboardLayout.DE;
                    break;
            }
        }

        #endregion
    }
}
