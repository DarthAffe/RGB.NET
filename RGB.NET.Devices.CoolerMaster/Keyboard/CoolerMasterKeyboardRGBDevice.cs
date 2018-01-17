using System.Collections.Generic;
using System.Linq;
using RGB.NET.Core;
using RGB.NET.Devices.CoolerMaster.Native;

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
        protected override void UpdateLeds(IEnumerable<Led> ledsToUpdate)
        {
            List<Led> leds = ledsToUpdate.Where(x => x.Color.A > 0).ToList();

            if (leds.Count > 0)
            {
                // 6 by 22 seems hard-coded but it's what the CM SDK expects regardless of keyboard size
                _CoolerMasterColorMatrix colorMatrix = new _CoolerMasterColorMatrix { KeyColor = new _CoolerMasterKeyColor[_CoolerMasterColorMatrix.ROWS, _CoolerMasterColorMatrix.COLUMNS] };
                foreach (Led led in leds)
                {
                    (int row, int column) = ((int, int))led.CustomData;
                    colorMatrix.KeyColor[row, column] = new _CoolerMasterKeyColor(led.Color.R, led.Color.G, led.Color.B);
                }

                _CoolerMasterSDK.SetControlDevice(DeviceInfo.DeviceIndex);
                _CoolerMasterSDK.SetAllLedColor(colorMatrix);
                _CoolerMasterSDK.RefreshLed(false);
            }
        }

        /// <inheritdoc />
        protected override void InitializeLayout()
        {
            Dictionary<LedId, (int row, int column)> mapping = CoolerMasterKeyboardLedMappings.Mapping[DeviceInfo.DeviceIndex][DeviceInfo.PhysicalLayout];

            foreach (KeyValuePair<LedId, (int row, int column)> led in mapping)
                InitializeLed(led.Key, new Rectangle(led.Value.column * 19, led.Value.row * 19, 19, 19));

            string model = DeviceInfo.Model.Replace(" ", string.Empty).ToUpper();
            ApplyLayoutFromFile(PathHelper.GetAbsolutePath(
                $@"Layouts\CoolerMaster\Keyboards\{model}\{DeviceInfo.PhysicalLayout.ToString().ToUpper()}.xml"), DeviceInfo.LogicalLayout.ToString());
        }

        /// <inheritdoc />
        protected override object CreateLedCustomData(LedId ledId) => CoolerMasterKeyboardLedMappings.Mapping[DeviceInfo.DeviceIndex][DeviceInfo.PhysicalLayout][ledId];

        #endregion
    }
}
