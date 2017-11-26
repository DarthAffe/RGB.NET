using System;
using System.Collections.Generic;
using System.Linq;
using RGB.NET.Core;
using RGB.NET.Devices.Logitech.Native;

namespace RGB.NET.Devices.Logitech
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a logitech per-device-lightable device.
    /// </summary>
    public class LogitechPerDeviceRGBDevice : LogitechRGBDevice<LogitechRGBDeviceInfo>
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Logitech.LogitechPerDeviceRGBDevice" /> class.
        /// </summary>
        /// <param name="info">The specific information provided by logitech for the per-device-lightable device</param>
        internal LogitechPerDeviceRGBDevice(LogitechRGBDeviceInfo info)
            : base(info)
        { }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void InitializeLayout()
        {
            base.InitializeLayout();

            if (LedMapping.Count == 0)
                InitializeLed(new LogitechLedId(this, LogitechLedIds.DEVICE), new Rectangle(0, 0, 10, 10));
        }

        /// <inheritdoc />
        protected override void UpdateLeds(IEnumerable<Led> ledsToUpdate)
        {
            Led led = ledsToUpdate.FirstOrDefault(x => x.Color.A > 0);
            if (led == null) return;

            _LogitechGSDK.LogiLedSetTargetDevice(LogitechDeviceCaps.DeviceRGB);
            _LogitechGSDK.LogiLedSetLighting((int)Math.Round(led.Color.RPercent * 100),
                                             (int)Math.Round(led.Color.GPercent * 100),
                                             (int)Math.Round(led.Color.BPercent * 100));
        }

        #endregion
    }
}
