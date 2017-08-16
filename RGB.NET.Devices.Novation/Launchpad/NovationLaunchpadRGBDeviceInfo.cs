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
        /// <param name="deviceId"></param>
        internal NovationLaunchpadRGBDeviceInfo(string model, int deviceId)
            : base(RGBDeviceType.LedMatrix, model, deviceId)
        {
            Image = new Uri(PathHelper.GetAbsolutePath($@"Images\Novation\Launchpads\{Model.Replace(" ", string.Empty).ToUpper()}.png"), UriKind.Absolute);
        }

        #endregion
    }
}
