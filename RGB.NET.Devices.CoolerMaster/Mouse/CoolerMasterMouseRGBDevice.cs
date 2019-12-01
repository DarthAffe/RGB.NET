using System.Collections.Generic;
using RGB.NET.Core;

namespace RGB.NET.Devices.CoolerMaster
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a CoolerMaster mouse.
    /// </summary>
    public class CoolerMasterMouseRGBDevice : CoolerMasterRGBDevice<CoolerMasterMouseRGBDeviceInfo>
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.CoolerMaster.CoolerMasterMouseRGBDevice" /> class.
        /// </summary>
        /// <param name="info">The specific information provided by CoolerMaster for the mouse</param>
        internal CoolerMasterMouseRGBDevice(CoolerMasterMouseRGBDeviceInfo info)
            : base(info)
        { }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void InitializeLayout()
        {
            Dictionary<LedId, (int row, int column)> mapping = CoolerMasterMouseLedMappings.Mapping[DeviceInfo.DeviceIndex];

            foreach (KeyValuePair<LedId, (int row, int column)> led in mapping)
                InitializeLed(led.Key, new Rectangle(led.Value.column * 19, led.Value.row * 19, 19, 19));

            string model = DeviceInfo.Model.Replace(" ", string.Empty).ToUpper();
            ApplyLayoutFromFile(PathHelper.GetAbsolutePath(this, @"Layouts\CoolerMaster\Mice", $"{model}.xml"), null);
        }

        /// <inheritdoc />
        protected override object CreateLedCustomData(LedId ledId) => CoolerMasterMouseLedMappings.Mapping[DeviceInfo.DeviceIndex][ledId];

        #endregion
    }
}
