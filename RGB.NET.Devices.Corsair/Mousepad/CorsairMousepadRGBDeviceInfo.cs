using System;
using RGB.NET.Core;
using RGB.NET.Devices.Corsair.Native;

namespace RGB.NET.Devices.Corsair
{
    /// <summary>
    /// Represents a generic information for a <see cref="CorsairMousepadRGBDevice"/>.
    /// </summary>
    public class CorsairMousepadRGBDeviceInfo : CorsairRGBDeviceInfo
    {
        #region Constructors

        /// <summary>
        /// Internal constructor of managed <see cref="CorsairMousepadRGBDeviceInfo"/>.
        /// </summary>
        /// <param name="deviceIndex">The index if the <see cref="CorsairMousepadRGBDevice"/>.</param>
        /// <param name="nativeInfo">The native <see cref="_CorsairDeviceInfo" />-struct</param>
        internal CorsairMousepadRGBDeviceInfo(int deviceIndex, _CorsairDeviceInfo nativeInfo)
            : base(deviceIndex, RGBDeviceType.Mousepad, nativeInfo)
        {
            Image = new Uri(PathHelper.GetAbsolutePath($@"Images\Corsair\Mousepad\{Model.Replace(" ", string.Empty).ToUpper()}.png"), UriKind.Absolute);
        }

        #endregion
    }
}
