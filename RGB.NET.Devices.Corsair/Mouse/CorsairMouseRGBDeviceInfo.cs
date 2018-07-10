using System.Collections.Generic;
using RGB.NET.Core;
using RGB.NET.Devices.Corsair.Native;

namespace RGB.NET.Devices.Corsair
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a generic information for a <see cref="T:RGB.NET.Devices.Corsair.CorsairMouseRGBDevice" />.
    /// </summary>
    public class CorsairMouseRGBDeviceInfo : CorsairRGBDeviceInfo
    {
        #region Properties & Fields

        /// <summary>
        /// Gets the physical layout of the mouse.
        /// </summary>
        public CorsairPhysicalMouseLayout PhysicalLayout { get; }

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Internal constructor of managed <see cref="T:RGB.NET.Devices.Corsair.CorsairMouseRGBDeviceInfo" />.
        /// </summary>
        /// <param name="deviceIndex">The index of the <see cref="T:RGB.NET.Devices.Corsair.CorsairMouseRGBDevice" />.</param>
        /// <param name="nativeInfo">The native <see cref="T:RGB.NET.Devices.Corsair.Native._CorsairDeviceInfo" />-struct</param>
        /// <param name="modelCounter">A dictionary containing counters to create unique names for equal devices models.</param>
        internal CorsairMouseRGBDeviceInfo(int deviceIndex, _CorsairDeviceInfo nativeInfo, Dictionary<string, int> modelCounter)
            : base(deviceIndex, RGBDeviceType.Mouse, nativeInfo, modelCounter)
        {
            this.PhysicalLayout = (CorsairPhysicalMouseLayout)nativeInfo.physicalLayout;
        }

        #endregion
    }
}
