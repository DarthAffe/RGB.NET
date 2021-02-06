using RGB.NET.Core;

namespace RGB.NET.Devices.CoolerMaster
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a generic information for a <see cref="T:RGB.NET.Devices.CoolerMaster.CoolerMasterKeyboardRGBDevice" />.
    /// </summary>
    public class CoolerMasterKeyboardRGBDeviceInfo : CoolerMasterRGBDeviceInfo
    {
        #region Properties & Fields

        /// <summary>
        /// Gets the <see cref="CoolerMasterPhysicalKeyboardLayout"/> of the <see cref="CoolerMasterKeyboardRGBDevice"/>.
        /// </summary>
        public CoolerMasterPhysicalKeyboardLayout PhysicalLayout { get; }

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Internal constructor of managed <see cref="T:RGB.NET.Devices.CoolerMaster.CoolerMasterKeyboardRGBDeviceInfo" />.
        /// </summary>
        /// <param name="deviceIndex">The index of the <see cref="T:RGB.NET.Devices.CoolerMaster.CoolerMasterKeyboardRGBDevice" />.</param>
        /// <param name="physicalKeyboardLayout">The <see cref="T:RGB.NET.Devices.CoolerMaster.CoolerMasterPhysicalKeyboardLayout" /> of the <see cref="T:RGB.NET.Devices.CoolerMaster.CoolerMasterKeyboardRGBDevice" />.</param>
        internal CoolerMasterKeyboardRGBDeviceInfo(CoolerMasterDevicesIndexes deviceIndex, CoolerMasterPhysicalKeyboardLayout physicalKeyboardLayout)
                : base(RGBDeviceType.Keyboard, deviceIndex)
        {
            this.PhysicalLayout = physicalKeyboardLayout;
        }

        #endregion
    }
}
