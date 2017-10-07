using System;
using RGB.NET.Core;

namespace RGB.NET.Devices.Aura
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a generic information for a <see cref="T:RGB.NET.Devices.Aura.AuraMouseRGBDevice" />.
    /// </summary>
    public class AuraMouseRGBDeviceInfo : AuraRGBDeviceInfo
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Internal constructor of managed <see cref="T:RGB.NET.Devices.Aura.AuraMouseRGBDeviceInfo" />.
        /// </summary>
        /// <param name="deviceType">The type of the <see cref="IRGBDevice"/>.</param>
        /// <param name="handle">The handle of the <see cref="IRGBDevice"/>.</param>
        internal AuraMouseRGBDeviceInfo(RGBDeviceType deviceType, IntPtr handle)
            : base(deviceType, handle, "Asus", "Rog")
        {
            Image = new Uri(PathHelper.GetAbsolutePath($@"Images\Aura\Mouses\{Model.Replace(" ", string.Empty).ToUpper()}.png"), UriKind.Absolute);
        }

        #endregion
    }
}
