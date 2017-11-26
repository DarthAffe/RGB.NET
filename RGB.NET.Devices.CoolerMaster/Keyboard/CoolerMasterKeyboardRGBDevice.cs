using System;
using System.Collections.Generic;
using RGB.NET.Core;

namespace RGB.NET.Devices.CoolerMaster
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a CoolerMaster keyboard.
    /// </summary>
    public class CoolerMasterKeyboardRGBDevice : CoolerMasterRGBDevice<CoolerMasterKeyboardRGBDeviceInfo>
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.CoolerMaster.CoolerMasterKeyboardRGBDevice" /> class.
        /// </summary>
        /// <param name="info">The specific information provided by CoolerMaster for the keyboard</param>
        internal CoolerMasterKeyboardRGBDevice(CoolerMasterKeyboardRGBDeviceInfo info)
            : base(info)
        { }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void InitializeLayout()
        {
            Dictionary<CoolerMasterLedIds, Tuple<int, int>> mapping = CoolerMasterKeyboardLedMappings.Mapping[DeviceInfo.DeviceIndex][DeviceInfo.PhysicalLayout];

            foreach (KeyValuePair<CoolerMasterLedIds, Tuple<int, int>> led in mapping)
                InitializeLed(new CoolerMasterLedId(this, led.Key, led.Value.Item1, led.Value.Item2),
                              new Rectangle(led.Value.Item2 * 19, led.Value.Item1 * 19, 19, 19));

            string model = DeviceInfo.Model.Replace(" ", string.Empty).ToUpper();
            ApplyLayoutFromFile(PathHelper.GetAbsolutePath(
                $@"Layouts\CoolerMaster\Keyboards\{model}\{DeviceInfo.PhysicalLayout.ToString().ToUpper()}.xml"),
                DeviceInfo.LogicalLayout.ToString(), PathHelper.GetAbsolutePath(@"Images\CoolerMaster\Keyboards"));
        }

        #endregion
    }
}
