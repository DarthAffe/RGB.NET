using System.Collections.Generic;
using System.Linq;
using AuraServiceLib;
using RGB.NET.Core;

namespace RGB.NET.Devices.Asus
{
    /// <inheritdoc cref="AsusRGBDevice{TDeviceInfo}" />
    /// <summary>
    /// Represents a Asus keyboard.
    /// </summary>
    public class AsusKeyboardRGBDevice : AsusRGBDevice<AsusKeyboardRGBDeviceInfo>, IKeyboard
    {
        #region Properties & Fields

        IKeyboardDeviceInfo IKeyboard.DeviceInfo => DeviceInfo;

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Asus.AsusKeyboardRGBDevice" /> class.
        /// </summary>
        /// <param name="info">The specific information provided by Asus for the keyboard.</param>
        internal AsusKeyboardRGBDevice(AsusKeyboardRGBDeviceInfo info, IDeviceUpdateTrigger updateTrigger)
            : base(info, updateTrigger)
        {
            InitializeLayout();
        }

        #endregion

        #region Methods

        private void InitializeLayout()
        {
            if (DeviceInfo.Device.Type != (uint)AsusDeviceType.NB_KB_4ZONE_RGB)
            {
                int pos = 0;
                foreach (IAuraRgbKey key in ((IAuraSyncKeyboard)DeviceInfo.Device).Keys)
                {
                    if (AsusKeyboardLedMapping.MAPPING.TryGetValue((AsusLedId)key.Code, out LedId ledId))
                        AddLed(ledId, new Point(pos++ * 19, 0), new Size(19, 19));
                    else
                        throw new RGBDeviceException($"Couldn't find a LED mapping for key {key.Code:X} named '{key.Name}' on device '{DeviceInfo.DeviceName}'");
                }

                //UK Layout
                AddLed(AsusKeyboardLedMapping.MAPPING[AsusLedId.KEY_OEM_102], new Point(pos++ * 19, 0), new Size(19, 19));

                AddLed(AsusKeyboardLedMapping.MAPPING[AsusLedId.UNDOCUMENTED_1], new Point(pos * 19, 0), new Size(19, 19));
            }
            else
            {
                int ledCount = DeviceInfo.Device.Lights.Count;
                for (int i = 0; i < ledCount; i++)
                    AddLed(LedId.Keyboard_Custom1 + i, new Point(i * 19, 0), new Size(19, 19));
            }
        }

        /// <inheritdoc />
        protected override object? GetLedCustomData(LedId ledId)
        {
            if (DeviceInfo.Device.Type == (uint)AsusDeviceType.NB_KB_4ZONE_RGB)
                return ledId - LedId.Keyboard_Custom1;

            return AsusKeyboardLedMapping.MAPPING.FirstOrDefault(m => m.Value == ledId).Key;
        }

        #endregion
    }
}
