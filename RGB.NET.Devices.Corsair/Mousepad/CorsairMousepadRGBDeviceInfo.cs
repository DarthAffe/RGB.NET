using System.Collections.Generic;
using RGB.NET.Core;
using RGB.NET.Devices.Corsair.Native;

namespace RGB.NET.Devices.Corsair
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a generic information for a <see cref="T:RGB.NET.Devices.Corsair.CorsairMousepadRGBDevice" />.
    /// </summary>
    public class CorsairMousepadRGBDeviceInfo : CorsairRGBDeviceInfo
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Internal constructor of managed <see cref="T:RGB.NET.Devices.Corsair.CorsairMousepadRGBDeviceInfo" />.
        /// </summary>
        /// <param name="deviceIndex">The index if the <see cref="T:RGB.NET.Devices.Corsair.CorsairMousepadRGBDevice" />.</param>
        /// <param name="nativeInfo">The native <see cref="T:RGB.NET.Devices.Corsair.Native._CorsairDeviceInfo" />-struct</param>
        /// <param name="modelCounter">A dictionary containing counters to create unique names for equal devices models.</param>
        internal CorsairMousepadRGBDeviceInfo(int deviceIndex, _CorsairDeviceInfo nativeInfo, Dictionary<string, int> modelCounter)
            : base(deviceIndex, RGBDeviceType.Mousepad, nativeInfo, modelCounter)
        { }

        #endregion
    }
}
