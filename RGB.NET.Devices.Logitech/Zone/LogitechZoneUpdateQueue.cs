using System;
using System.Collections.Generic;
using RGB.NET.Core;
using RGB.NET.Devices.Logitech.Native;

namespace RGB.NET.Devices.Logitech
{
    /// <summary>
    /// Represents the update-queue performing updates for logitech zone devices.
    /// </summary>
    public class LogitechZoneUpdateQueue : UpdateQueue
    {
        #region Constants

        private static readonly Dictionary<RGBDeviceType, LogitechDeviceType> DEVICE_TYPE_MAPPING = new Dictionary<RGBDeviceType, LogitechDeviceType>
        {
            {RGBDeviceType.Keyboard, LogitechDeviceType.Keyboard},
            {RGBDeviceType.Mouse, LogitechDeviceType.Mouse},
            {RGBDeviceType.Headset, LogitechDeviceType.Headset},
            {RGBDeviceType.Mousepad, LogitechDeviceType.Mousemat},
            {RGBDeviceType.Speaker, LogitechDeviceType.Speaker}
        };

        #endregion

        #region Properties & Fields

        private readonly LogitechDeviceType _deviceType;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LogitechZoneUpdateQueue"/> class.
        /// </summary>
        /// <param name="updateTrigger">The update trigger used by this queue.</param>
        /// <param name="deviceType">The tpye of the device this queue is updating.</param>
        public LogitechZoneUpdateQueue(IDeviceUpdateTrigger updateTrigger, RGBDeviceType deviceType)
            : base(updateTrigger)
        {
            if (!DEVICE_TYPE_MAPPING.TryGetValue(deviceType, out _deviceType))
                throw new ArgumentException($"Invalid type '{deviceType.ToString()}'", nameof(deviceType));
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void Update(Dictionary<object, Color> dataSet)
        {
            _LogitechGSDK.LogiLedSetTargetDevice(LogitechDeviceCaps.All);

            foreach (KeyValuePair<object, Color> data in dataSet)
            {
                int zone = (int)data.Key;
                _LogitechGSDK.LogiLedSetLightingForTargetZone(_deviceType, zone,
                                                              (int)Math.Round(data.Value.RPercent * 100),
                                                              (int)Math.Round(data.Value.GPercent * 100),
                                                              (int)Math.Round(data.Value.BPercent * 100));
            }
        }

        #endregion
    }
}
