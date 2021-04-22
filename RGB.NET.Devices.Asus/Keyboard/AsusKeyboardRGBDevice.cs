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

        private Dictionary<LedId, AsusLedId> _ledAsusLed = new();
        private Dictionary<LedId, int> _ledAsusLights = new();

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
                int unknownLed = (int)LedId.Unknown1;

                List<IAuraRgbKey> keys = ((IAuraSyncKeyboard)DeviceInfo.Device).Keys.Cast<IAuraRgbKey>().ToList();
                foreach (IAuraRgbKey key in keys)
                {
                    if (AsusKeyboardLedMapping.MAPPING.TryGetValue((AsusLedId)key.Code, out LedId ledId))
                        AddAsusLed((AsusLedId)key.Code, ledId, new Point(pos++ * 19, 0), new Size(19, 19));
                    else
                    {
                        AddAsusLed((AsusLedId)key.Code, (LedId)unknownLed, new Point(pos++ * 19, 0), new Size(19, 19));
                        unknownLed++;
                    }
                }

                for (int index = 0; index < ((IAuraSyncKeyboard)DeviceInfo.Device).Lights.Count; index++)
                {
                    IAuraRgbLight light = ((IAuraSyncKeyboard)DeviceInfo.Device).Lights[index];
                    if (keys.Contains(light))
                        continue;

                    AddAsusLed(index, (LedId)unknownLed, new Point(pos++ * 19, 0), new Size(19, 19));
                    unknownLed++;
                }
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
                throw new
                    RGBDeviceException($"Got LED '{ledId}' twice, first ASUS LED '{firstAsusLed}' second ASUS LED '{asusLedId}' on device '{DeviceInfo.DeviceName}'");

            this._ledAsusLed.Add(ledId, asusLedId);
            AddLed(ledId, position, size);
        }

        private void AddAsusLed(int index, LedId ledId, Point position, Size size)
        {
            this._ledAsusLights.Add(ledId, index);
            AddLed(ledId, position, size);
        }

        /// <inheritdoc />
        protected override object? GetLedCustomData(LedId ledId)
        {
            if (this._ledAsusLed.TryGetValue(ledId, out AsusLedId asusLedId))
                return (true, (int)asusLedId);
            if (this._ledAsusLights.TryGetValue(ledId, out int lightIndex))
                return (false, lightIndex);
            return null;
        }

        #endregion
    }
}
