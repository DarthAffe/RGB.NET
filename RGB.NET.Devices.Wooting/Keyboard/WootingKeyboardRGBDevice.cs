using System.Collections.Generic;
using System.Linq;
using RGB.NET.Core;
using RGB.NET.Devices.Wooting.Generic;

namespace RGB.NET.Devices.Wooting.Keyboard
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a Wooting keyboard.
    /// </summary>
    public class WootingKeyboardRGBDevice : WootingRGBDevice<WootingKeyboardRGBDeviceInfo>
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Wooting.Keyboard.WootingKeyboardRGBDevice" /> class.
        /// </summary>
        /// <param name="info">The specific information provided by Wooting for the keyboard</param>
        internal WootingKeyboardRGBDevice(WootingKeyboardRGBDeviceInfo info)
            : base(info)
        { }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void InitializeLayout()
        {
            
            Dictionary<LedId, (int row, int column)> mapping = WootingKeyboardLedMappings.Mapping[DeviceInfo.DeviceIndex][DeviceInfo.PhysicalLayout];

            foreach (KeyValuePair<LedId, (int row, int column)> led in mapping)
            {
                InitializeLed(led.Key, new Point(led.Value.column * 19, led.Value.row * 19), new Size(19,19));
            }

            string model = DeviceInfo.Model.Replace(" ", string.Empty).ToUpper();
            ApplyLayoutFromFile(PathHelper.GetAbsolutePath(this, $@"Layouts\Wooting\Keyboards\{model}", $"{DeviceInfo.PhysicalLayout.ToString().ToUpper()}.xml"),
                                DeviceInfo.LogicalLayout.ToString());
        }

        /// <inheritdoc />
        protected override void UpdateLeds(IEnumerable<Led> ledsToUpdate) => UpdateQueue.SetData(ledsToUpdate.Where(x => x.Color.A > 0));

        /// <inheritdoc />
        protected override object CreateLedCustomData(LedId ledId) => WootingKeyboardLedMappings.Mapping[DeviceInfo.DeviceIndex][DeviceInfo.PhysicalLayout][ledId];

        #endregion
    }
}
