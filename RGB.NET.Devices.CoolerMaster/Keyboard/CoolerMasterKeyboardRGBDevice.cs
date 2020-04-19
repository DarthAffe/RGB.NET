using System.Collections.Generic;
using RGB.NET.Core;

namespace RGB.NET.Devices.CoolerMaster
{
    /// <inheritdoc cref="CoolerMasterRGBDevice{TDeviceInfo}" />
    /// <summary>
    /// Represents a CoolerMaster keyboard.
    /// </summary>
    public class CoolerMasterKeyboardRGBDevice : CoolerMasterRGBDevice<CoolerMasterKeyboardRGBDeviceInfo>, IKeyboard
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
            Dictionary<CoolerMasterPhysicalKeyboardLayout, Dictionary<LedId, (int row, int column)>> deviceMappings;
            Dictionary<LedId, (int row, int column)> mapping;
            
            if (!CoolerMasterKeyboardLedMappings.Mapping.TryGetValue(DeviceInfo.DeviceIndex, out deviceMappings))
                throw new RGBDeviceException($"Failed to find a CoolerMasterKeyboardLedMapping for device index {DeviceInfo.DeviceIndex}");
            if (!deviceMappings.TryGetValue(DeviceInfo.PhysicalLayout, out mapping))
                throw new RGBDeviceException($"Failed to find a CoolerMasterKeyboardLedMapping for device index {DeviceInfo.DeviceIndex} with physical layout {DeviceInfo.PhysicalLayout}");
            
            foreach (KeyValuePair<LedId, (int row, int column)> led in mapping)
                InitializeLed(led.Key, new Rectangle(led.Value.column * 19, led.Value.row * 19, 19, 19));

            string model = DeviceInfo.Model.Replace(" ", string.Empty).ToUpper();
            ApplyLayoutFromFile(PathHelper.GetAbsolutePath(this, $@"Layouts\CoolerMaster\Keyboards\{model}", $"{DeviceInfo.PhysicalLayout.ToString().ToUpper()}.xml"),
                                DeviceInfo.LogicalLayout.ToString());
        }

        /// <inheritdoc />
        protected override object CreateLedCustomData(LedId ledId) => CoolerMasterKeyboardLedMappings.Mapping[DeviceInfo.DeviceIndex][DeviceInfo.PhysicalLayout][ledId];

        #endregion
    }
}
