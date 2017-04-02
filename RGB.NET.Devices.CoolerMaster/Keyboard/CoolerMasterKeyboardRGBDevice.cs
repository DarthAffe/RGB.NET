using System;
using System.Collections.Generic;
using RGB.NET.Core;
using RGB.NET.Devices.CoolerMaster.Helper;

namespace RGB.NET.Devices.CoolerMaster
{
    /// <summary>
    /// Represents a CoolerMaster keyboard.
    /// </summary>
    public class CoolerMasterKeyboardRGBDevice : CoolerMasterRGBDevice
    {
        #region Properties & Fields

        /// <summary>
        /// Gets information about the <see cref="CoolerMasterKeyboardRGBDevice"/>.
        /// </summary>
        public CoolerMasterKeyboardRGBDeviceInfo KeyboardDeviceInfo { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CoolerMasterKeyboardRGBDevice"/> class.
        /// </summary>
        /// <param name="info">The specific information provided by CoolerMaster for the keyboard</param>
        internal CoolerMasterKeyboardRGBDevice(CoolerMasterKeyboardRGBDeviceInfo info)
            : base(info)
        {
            this.KeyboardDeviceInfo = info;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void InitializeLayout()
        {
            Dictionary<CoolerMasterLedIds, Tuple<int, int>> mapping = CoolerMasterKeyboardLedMappings.Mapping[KeyboardDeviceInfo.DeviceIndex][KeyboardDeviceInfo.PhysicalLayout];

            foreach (KeyValuePair<CoolerMasterLedIds, Tuple<int, int>> led in mapping)
                InitializeLed(new CoolerMasterLedId(this, led.Key, led.Value.Item1, led.Value.Item2),
                              new Rectangle(led.Value.Item2 * 19, led.Value.Item1 * 19, 19, 19));

            string model = KeyboardDeviceInfo.Model.Replace(" ", string.Empty).ToUpper();
            ApplyLayoutFromFile(PathHelper.GetAbsolutePath(
                $@"Layouts\CoolerMaster\Keyboards\{model}\{KeyboardDeviceInfo.PhysicalLayout.ToString().ToUpper()}.xml"),
                KeyboardDeviceInfo.LogicalLayout.ToString(), PathHelper.GetAbsolutePath($@"Images\CoolerMaster\Keyboards"));
        }

        #endregion
    }
}
