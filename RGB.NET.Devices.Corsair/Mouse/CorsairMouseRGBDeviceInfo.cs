using System;
using RGB.NET.Devices.Corsair.Native;

namespace RGB.NET.Devices.Corsair
{
    /// <summary>
    /// Represents a generic information for a <see cref="CorsairMouseRGBDevice"/>.
    /// </summary>
    public class CorsairMouseRGBDeviceInfo : CorsairRGBDeviceInfo
    {
        #region Properties & Fields

        /// <summary>
        /// Gets the physical layout of the mouse.
        /// </summary>
        public CorsairPhysicalMouseLayout PhysicalLayout { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Internal constructor of managed <see cref="CorsairMouseRGBDeviceInfo"/>.
        /// </summary>
        /// <param name="deviceIndex">The index of the <see cref="CorsairMouseRGBDevice"/>.</param>
        /// <param name="nativeInfo">The native <see cref="_CorsairDeviceInfo" />-struct</param>
        internal CorsairMouseRGBDeviceInfo(int deviceIndex, _CorsairDeviceInfo nativeInfo)
            : base(deviceIndex, Core.RGBDeviceType.Mouse, nativeInfo)
        {
            this.PhysicalLayout = (CorsairPhysicalMouseLayout)nativeInfo.physicalLayout;

            Image = new Uri($"pack://application:,,,/RGB.NET.Devices.Corsair;component/Images/Mice/{Model.Replace(" ", string.Empty).ToUpper()}.png", UriKind.Absolute);
        }

        #endregion
    }
}
