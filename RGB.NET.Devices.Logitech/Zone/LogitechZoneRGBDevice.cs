using System.Collections.Generic;
using System.Linq;
using RGB.NET.Core;

namespace RGB.NET.Devices.Logitech
{
    /// <inheritdoc cref="LogitechRGBDevice{TDeviceInfo}" />
    /// <summary>
    /// Represents a logitech zone-lightable device.
    /// </summary>
    public class LogitechZoneRGBDevice : LogitechRGBDevice<LogitechRGBDeviceInfo>, IUnknownDevice //TODO DarthAffe 18.04.2020: It's know which kind of device this is, but they would need to be separated
    {
        #region Constants

        private static readonly Dictionary<RGBDeviceType, LedId> BASE_LED_MAPPING = new()
        {
            { RGBDeviceType.Keyboard, LedId.Keyboard_Programmable1 },
            { RGBDeviceType.Mouse, LedId.Mouse1 },
            { RGBDeviceType.Headset, LedId.Headset1 },
            { RGBDeviceType.Mousepad, LedId.Mousepad1 },
            { RGBDeviceType.Speaker, LedId.Speaker1 }
        };

        #endregion

        #region Properties & Fields

        private LedId _baseLedId;

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Logitech.LogitechZoneRGBDevice" /> class.
        /// </summary>
        /// <param name="info">The specific information provided by logitech for the zone-lightable device</param>
        internal LogitechZoneRGBDevice(LogitechRGBDeviceInfo info)
            : base(info)
        {
            _baseLedId = BASE_LED_MAPPING.TryGetValue(info.DeviceType, out LedId id) ? id : LedId.Custom1;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public override void Initialize(UpdateQueue updateQueue)
        {
            base.Initialize(updateQueue);

            for (int i = 0; i < DeviceInfo.Zones; i++)
                AddLed(_baseLedId + i, new Point(i * 10, 0), new Size(10, 10));
        }

        /// <inheritdoc />
        protected override object? GetLedCustomData(LedId ledId) => (int)(ledId - _baseLedId);

        /// <inheritdoc />
        protected override void UpdateLeds(IEnumerable<Led> ledsToUpdate) => UpdateQueue?.SetData(ledsToUpdate.Where(x => x.Color.A > 0));

        #endregion
    }
}
