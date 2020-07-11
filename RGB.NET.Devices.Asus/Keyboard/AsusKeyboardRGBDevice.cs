using System.Collections.Generic;
using System.Linq;
using AuraServiceLib;
using RGB.NET.Core;

namespace RGB.NET.Devices.Asus
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a Asus keyboard.
    /// </summary>
    public class AsusKeyboardRGBDevice : AsusRGBDevice<AsusKeyboardRGBDeviceInfo>
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Asus.AsusKeyboardRGBDevice" /> class.
        /// </summary>
        /// <param name="info">The specific information provided by Asus for the keyboard.</param>
        internal AsusKeyboardRGBDevice(AsusKeyboardRGBDeviceInfo info)
            : base(info)
        { }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void InitializeLayout()
        {
            Dictionary<AsusLedId, LedId> reversedMapping = AsusKeyboardLedMapping.MAPPING.ToDictionary(x => x.Value, x => x.Key);

            if (DeviceInfo.Device.Type != (uint)AsusDeviceType.NB_KB_4ZONE_RGB)
            {
                int pos = 0;
                foreach (IAuraRgbKey key in ((IAuraSyncKeyboard)DeviceInfo.Device).Keys)
                    InitializeLed(reversedMapping[(AsusLedId)key.Code], new Point(pos++ * 19, 0), new Size(19, 19));

                //UK Layout
                InitializeLed(reversedMapping[AsusLedId.KEY_OEM_102], new Point(pos++ * 19, 0), new Size(19, 19));

                InitializeLed(reversedMapping[AsusLedId.UNDOCUMENTED_1], new Point(pos * 19, 0), new Size(19, 19));
            }
            else
            {
                int ledCount = DeviceInfo.Device.Lights.Count;
                for (int i = 0; i < ledCount; i++)
                    InitializeLed(LedId.Keyboard_Custom1 + i, new Point(i * 19, 0), new Size(19, 19));
            }

            string model = DeviceInfo.Model.Replace(" ", string.Empty).ToUpper();
            ApplyLayoutFromFile(PathHelper.GetAbsolutePath(this, $@"Layouts\Asus\Keyboards\{model}", $"{DeviceInfo.PhysicalLayout.ToString().ToUpper()}.xml"), DeviceInfo.LogicalLayout.ToString());
        }

        /// <inheritdoc />
        protected override object CreateLedCustomData(LedId ledId)
        {
            if (DeviceInfo.Device.Type == (uint)AsusDeviceType.NB_KB_4ZONE_RGB)
                return ledId - LedId.Keyboard_Custom1;

            return AsusKeyboardLedMapping.MAPPING[ledId];
        }

        #endregion
    }
}
