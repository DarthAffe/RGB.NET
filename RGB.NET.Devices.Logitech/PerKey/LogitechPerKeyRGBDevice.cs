using System.Collections.Generic;
using System.Linq;
using RGB.NET.Core;

namespace RGB.NET.Devices.Logitech
{
    /// <inheritdoc cref="LogitechRGBDevice{TDeviceInfo}" />
    /// <summary>
    /// Represents a logitech per-key-lightable device.
    /// </summary>
    public class LogitechPerKeyRGBDevice : LogitechRGBDevice<LogitechRGBDeviceInfo>, IUnknownDevice //TODO DarthAffe 18.04.2020: It's know which kind of device this is, but they would need to be separated
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Logitech.LogitechPerKeyRGBDevice" /> class.
        /// </summary>
        /// <param name="info">The specific information provided by logitech for the per-key-lightable device</param>
        internal LogitechPerKeyRGBDevice(LogitechRGBDeviceInfo info)
            : base(info)
        { }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override object? GetLedCustomData(LedId ledId) => (ledId, PerKeyIdMapping.DEFAULT.TryGetValue(ledId, out LogitechLedId logitechLedId) ? logitechLedId : LogitechLedId.Invalid);

        /// <inheritdoc />
        protected override void UpdateLeds(IEnumerable<Led> ledsToUpdate) => UpdateQueue?.SetData(ledsToUpdate.Where(x => x.Color.A > 0));

        #endregion
    }
}
