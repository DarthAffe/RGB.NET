using System;
using RGB.NET.Core;

namespace RGB.NET.Devices.Novation
{
    /// <summary>
    /// Represents a generic information for a <see cref="NovationLaunchpadRGBDevice"/>.
    /// </summary>
    public class NovationLaunchpadRGBDeviceInfo : NovationRGBDeviceInfo
    {
        #region Constructors

        /// <summary>
        /// Internal constructor of managed <see cref="NovationLaunchpadRGBDeviceInfo"/>.
        /// </summary>
        /// <param name="model">The represented device model.</param>
        internal NovationLaunchpadRGBDeviceInfo(string model)
            : base(RGBDeviceType.LedMatrix, model)
        {
            Image = new Uri(PathHelper.GetAbsolutePath($@"Images\Novation\Launchpads\{Model.Replace(" ", string.Empty).ToUpper()}.png"), UriKind.Absolute);
        }

        #endregion
    }
}
