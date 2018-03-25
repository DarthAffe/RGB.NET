using System.Collections.Generic;
using System.Linq;
using RGB.NET.Core;

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
                InitializeLed(LedId.Custom1, new Rectangle(0, 0, 10, 10));
        }

        /// <inheritdoc />
        protected override object CreateLedCustomData(LedId ledId) => (ledId, LogitechLedId.DEVICE);

        /// <inheritdoc />
        protected override void UpdateLeds(IEnumerable<Led> ledsToUpdate) => UpdateQueue.SetData(ledsToUpdate.Where(x => x.Color.A > 0).Take(1));

        #endregion
    }
}
