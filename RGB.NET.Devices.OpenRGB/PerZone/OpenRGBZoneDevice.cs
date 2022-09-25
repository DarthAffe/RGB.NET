using OpenRGB.NET.Enums;
using OpenRGB.NET.Models;
using RGB.NET.Core;

namespace RGB.NET.Devices.OpenRGB
{
    /// <inheritdoc />
    public class OpenRGBZoneDevice : AbstractOpenRGBDevice<OpenRGBZoneDeviceInfo>
    {
        private readonly int _initialLed;
        private readonly Zone _zone;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenRGBZoneDevice"/> class.
        /// </summary>
        /// <param name="info">The information provided by OpenRGB</param>
        /// <param name="initialLed">The ledId of the first led in the device that belongs to this zone.</param>
        /// <param name="zone">The Zone information provided by OpenRGB.</param>
        /// <param name="updateQueue">The queu used to update this zone.</param>
        public OpenRGBZoneDevice(OpenRGBZoneDeviceInfo info, int initialLed, Zone zone, IUpdateQueue updateQueue)
            : base(info, updateQueue)
        {
            _initialLed = initialLed;
            _zone = zone;

            InitializeLayout();
        }

        private void InitializeLayout()
        {
            Size ledSize = new Size(19);
            const int ledSpacing = 20;
            LedId initial = Helper.GetInitialLedIdForDeviceType(DeviceInfo.DeviceType) + _initialLed;

            if (_zone.Type == ZoneType.Matrix)
            {
                for (int row = 0; row < _zone.MatrixMap.Height; row++)
                {
                    for (int column = 0; column < _zone.MatrixMap.Width; column++)
                    {
                        uint index = _zone.MatrixMap.Matrix[row, column];

                        //will be max value if the position does not have an associated key
                        if (index == uint.MaxValue)
                            continue;

                        LedId ledId = StandardKeyNames.Default.TryGetValue(DeviceInfo.OpenRGBDevice.Leds[_initialLed + index].Name, out LedId l)
                            ? l
                            : initial++;

                        while (AddLed(ledId, new Point(ledSpacing * column, ledSpacing * row), ledSize, _initialLed + (int)index) == null)
                            ledId = initial++;
                    }
                }
            }
            else
            {
                for (int i = 0; i < _zone.LedCount; i++)
                {
                    LedId ledId = initial++;

                    while (AddLed(ledId, new Point(ledSpacing * i, 0), ledSize, _initialLed + i) == null)
                        ledId = initial++;
                }
            }
        }
    }
}