using RGB.NET.Core;

namespace RGB.NET.Devices.Novation
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a generic information for a <see cref="T:RGB.NET.Devices.Novation.NovationLaunchpadRGBDevice" />.
    /// </summary>
    public class NovationLaunchpadRGBDeviceInfo : NovationRGBDeviceInfo
    {
        #region Properties & Fields

        internal LedIdMappings LedIdMapping { get; }

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Internal constructor of managed <see cref="T:RGB.NET.Devices.Novation.NovationLaunchpadRGBDeviceInfo" />.
        /// </summary>
        /// <param name="model">The represented device model.</param>
        /// <param name="deviceId"></param>
        /// <param name="colorCapabilities">The <see cref="T:RGB.NET.Devices.Novation.NovationColorCapabilities" /> of the <see cref="T:RGB.NET.Core.IRGBDevice" />.</param>
        internal NovationLaunchpadRGBDeviceInfo(string model, int deviceId, NovationColorCapabilities colorCapabilities,
                                                LedIdMappings ledIdMapping)
            : base(RGBDeviceType.LedMatrix, model, deviceId, colorCapabilities)
        {
            this.LedIdMapping = ledIdMapping;
        }

        #endregion
    }
}
