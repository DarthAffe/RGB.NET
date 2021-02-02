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
            if (!CoolerMasterKeyboardLedMappings.Mapping.TryGetValue(DeviceInfo.DeviceIndex, out Dictionary<CoolerMasterPhysicalKeyboardLayout, Dictionary<LedId, (int row, int column)>>? deviceMappings))
                throw new RGBDeviceException($"Failed to find a CoolerMasterKeyboardLedMapping for device index {DeviceInfo.DeviceIndex}");
            if (!deviceMappings.TryGetValue(DeviceInfo.PhysicalLayout, out Dictionary<LedId, (int row, int column)>? mapping))
                throw new RGBDeviceException($"Failed to find a CoolerMasterKeyboardLedMapping for device index {DeviceInfo.DeviceIndex} with physical layout {DeviceInfo.PhysicalLayout}");

            foreach ((LedId ledId, (int row, int column)) in mapping)
                AddLed(ledId, new Point(column * 19, row * 19), new Size(19, 19));
        }

        /// <inheritdoc />
        protected override object GetLedCustomData(LedId ledId) => CoolerMasterKeyboardLedMappings.Mapping[DeviceInfo.DeviceIndex][DeviceInfo.PhysicalLayout][ledId];
        
        #endregion
    }
}
