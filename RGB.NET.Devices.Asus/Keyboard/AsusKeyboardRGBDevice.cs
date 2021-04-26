using System.Collections.Generic;
using System.Threading;
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

                // A device can have more lights than keys, a space bar with 4 lights per example but only the middle light is represented as a key
                // This means we want all lights but keys contain more information (a LED ID) so first pick up all keys and 'tag' them by giving them a color of 0x000001

                // Clear tags to make sure no device is already at 0x000001
                ClearTags();
                foreach (IAuraRgbKey key in ((IAuraSyncKeyboard)DeviceInfo.Device).Keys)
                {
                    if (AsusKeyboardLedMapping.MAPPING.TryGetValue((AsusLedId)key.Code, out LedId ledId))
                        AddAsusLed((AsusLedId)key.Code, ledId, new Point(pos++ * 19, 0), new Size(19, 19));
                    else
                    {
                        AddAsusLed((AsusLedId)key.Code, (LedId)unknownLed, new Point(pos++ * 19, 0), new Size(19, 19));
                        unknownLed++;
                    }

                    TagAsusLed(key);
                }

                // Give the ASUS SDK some time to catch up
                Thread.Sleep(100);

                // With keys iterated, add any light that was not tagged, these are lights that aren't represented by keys
                // Because there's no way to tell which light is which, they're all added as Unknown LEDs
                for (int index = 0; index < ((IAuraSyncKeyboard)DeviceInfo.Device).Lights.Count; index++)
                {
                    IAuraRgbLight light = ((IAuraSyncKeyboard)DeviceInfo.Device).Lights[index];
                    if (IsAsusLedTagged(light))
                        continue;

                    AddAsusLed(index, (LedId)unknownLed, new Point(pos++ * 19, 0), new Size(19, 19));
                    unknownLed++;
                }

                // Clear tags when done, the info is no longer relevant
                ClearTags();
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
            if (this._ledAsusLed.TryGetValue(ledId, out AsusLedId asusLedId))
                return (true, (int)asusLedId);
            if (this._ledAsusLights.TryGetValue(ledId, out int lightIndex))
                return (false, lightIndex);
            return null;
        }


        /// <summary>
        /// Add an ASUS LED by its LED ID
        /// </summary>
        private void AddAsusLed(AsusLedId asusLedId, LedId ledId, Point position, Size size)
        {
            if (this._ledAsusLed.TryGetValue(ledId, out AsusLedId firstAsusLed))
                throw new RGBDeviceException($"Got LED '{ledId}' twice, first ASUS LED '{firstAsusLed}' "
                                             + $"second ASUS LED '{asusLedId}' on device '{DeviceInfo.DeviceName}'");

            this._ledAsusLed.Add(ledId, asusLedId);
            AddLed(ledId, position, size);
        }

        /// <summary>
        /// Add an asus LED by its light index
        /// </summary>
        private void AddAsusLed(int index, LedId ledId, Point position, Size size)
        {
            this._ledAsusLights.Add(ledId, index);
            AddLed(ledId, position, size);
        }

        /// <summary>
        /// Clears the tags off all keys by setting their color to 0x000000
        /// </summary>
        private void ClearTags()
        {
            foreach (IAuraRgbLight light in ((IAuraSyncKeyboard)DeviceInfo.Device).Lights)
                light.Color = 0x000000;
        }

        /// <summary>
        /// Tags a LED by its key by setting its color to 0x000001
        /// </summary>
        /// <param name="key"></param>
        private void TagAsusLed(IAuraRgbKey key)
        {
            key.Color = 0x000001;
        }

        /// <summary>
        /// Determines whether a LED is tagged by its light
        /// </summary>
        /// <param name="light"></param>
        /// <returns></returns>
        private bool IsAsusLedTagged(IAuraRgbLight light)
        {
            return light.Color == 0x000001;
        }

        #endregion
    }
}
