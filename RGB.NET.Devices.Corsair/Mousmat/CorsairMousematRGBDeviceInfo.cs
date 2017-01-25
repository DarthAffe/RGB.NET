using RGB.NET.Devices.Corsair.Native;

namespace RGB.NET.Devices.Corsair
{
    /// <summary>
    /// Represents a generic information for a <see cref="CorsairMousematRGBDevice"/>.
    /// </summary>
    public class CorsairMousematRGBDeviceInfo : CorsairRGBDeviceInfo
    {
        #region Constructors

        /// <summary>
        /// Internal constructor of managed <see cref="CorsairMousematRGBDeviceInfo"/>.
        /// </summary>
        /// <param name="deviceIndex">The index if the <see cref="CorsairMousematRGBDevice"/>.</param>
        /// <param name="nativeInfo">The native <see cref="_CorsairDeviceInfo" />-struct</param>
        internal CorsairMousematRGBDeviceInfo(int deviceIndex, _CorsairDeviceInfo nativeInfo)
            : base(deviceIndex, Core.RGBDeviceType.Mousemat, nativeInfo)
        { }

        #endregion
    }
}
