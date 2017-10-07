using System;
using RGB.NET.Core;

namespace RGB.NET.Devices.Aura
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a generic information for a <see cref="T:RGB.NET.Devices.Aura.AuraGraphicsCardRGBDevice" />.
    /// </summary>
    public class AuraGraphicsCardRGBDeviceInfo : AuraRGBDeviceInfo
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Internal constructor of managed <see cref="T:RGB.NET.Devices.Aura.AuraGraphicsCardRGBDeviceInfo" />.
        /// </summary>
        /// <param name="deviceType">The type of the <see cref="IRGBDevice"/>.</param>
        /// <param name="handle">The handle of the <see cref="IRGBDevice"/>.</param>
        internal AuraGraphicsCardRGBDeviceInfo(RGBDeviceType deviceType, IntPtr handle)
            : base(deviceType, handle)
        {
            Image = new Uri(PathHelper.GetAbsolutePath($@"Images\Aura\GraphicsCards\{Model.Replace(" ", string.Empty).ToUpper()}.png"), UriKind.Absolute);
        }

        #endregion
    }
}
