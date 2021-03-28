using System.Collections.Generic;
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

        private Dictionary<LedId, AsusLedId> _ledAsusLed = new();

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
                int unknownLed = (int) LedId.Unknown1;
                foreach (IAuraRgbKey key in ((IAuraSyncKeyboard)DeviceInfo.Device).Keys)
                {
                    if (AsusKeyboardLedMapping.MAPPING.TryGetValue((AsusLedId)key.Code, out LedId ledId))
                        AddAsusLed((AsusLedId)key.Code, ledId, new Point(pos++ * 19, 0), new Size(19, 19));
                    else
                    {
                        AddAsusLed((AsusLedId)key.Code, (LedId)unknownLed, new Point(pos++ * 19, 0), new Size(19, 19));
                        unknownLed++;
                    }
                }

                // UK Layout
                AddAsusLed(AsusLedId.KEY_OEM_102, AsusKeyboardLedMapping.MAPPING[AsusLedId.KEY_OEM_102], new Point(pos++ * 19, 0), new Size(19, 19));
                AddAsusLed(AsusLedId.KEY_OEM_102, AsusKeyboardLedMapping.MAPPING[AsusLedId.UNDOCUMENTED_1], new Point(pos * 19, 0), new Size(19, 19));
            }
            else
            {
                int ledCount = DeviceInfo.Device.Lights.Count;
                for (int i = 0; i < ledCount; i++)
                    AddLed(LedId.Keyboard_Custom1 + i, new Point(i * 19, 0), new Size(19, 19));
            }
        }

        private void AddAsusLed(AsusLedId asusLedId, LedId ledId, Point position, Size size)
        {
            if (this._ledAsusLed.TryGetValue(ledId, out AsusLedId firstAsusLed))
                throw new RGBDeviceException($"Got LED '{ledId}' twice, first ASUS LED '{firstAsusLed}' second ASUS LED '{asusLedId}' on device '{DeviceInfo.DeviceName}'");

            this._ledAsusLed.Add(ledId, asusLedId);
            AddLed(ledId, position, size);
        }

        /// <inheritdoc />
        protected override object? GetLedCustomData(LedId ledId)
        {
            if (this._ledAsusLed.TryGetValue(ledId, out AsusLedId asusLedId))
                return asusLedId;
            return null;
        }

        #endregion
    }
}
