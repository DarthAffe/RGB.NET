using System;
using System.Runtime.InteropServices;
using RGB.NET.Core;
using RGB.NET.Devices.Corsair.Native;

namespace RGB.NET.Devices.Corsair
{
    /// <summary>
    /// Represents a generic information for a Corsair-<see cref="IRGBDevice"/>.
    /// </summary>
    public class CorsairRGBDeviceInfo : IRGBDeviceInfo
    {
        #region Properties & Fields

        /// <summary>
        /// Gets the corsair specific device type.
        /// </summary>
        public CorsairDeviceType CorsairDeviceType { get; }

        /// <summary>
        /// Gets the index of the <see cref="CorsairRGBDevice"/>.
        /// </summary>
        public int CorsairDeviceIndex { get; }

        /// <inheritdoc />
        public RGBDeviceType DeviceType { get; }

        /// <inheritdoc />
        public string Manufacturer => "Corsair";

        /// <inheritdoc />
        public string Model { get; }

        /// <summary>
        /// Gets a flag that describes device capabilities. (<see cref="CorsairDeviceCaps" />)
        /// </summary>
        public CorsairDeviceCaps CapsMask { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Internal constructor of managed <see cref="CorsairRGBDeviceInfo"/>.
        /// </summary>
        /// <param name="deviceIndex">The index of the <see cref="CorsairRGBDevice"/>.</param>
        /// <param name="deviceType">The type of the <see cref="IRGBDevice"/>.</param>
        /// <param name="nativeInfo">The native <see cref="_CorsairDeviceInfo" />-struct</param>
        internal CorsairRGBDeviceInfo(int deviceIndex, RGBDeviceType deviceType, _CorsairDeviceInfo nativeInfo)
        {
            this.CorsairDeviceIndex = deviceIndex;
            this.DeviceType = deviceType;
            this.CorsairDeviceType = nativeInfo.type;
            this.Model = nativeInfo.model == IntPtr.Zero ? null : Marshal.PtrToStringAnsi(nativeInfo.model);
            this.CapsMask = (CorsairDeviceCaps)nativeInfo.capsMask;
        }

        #endregion
    }
}
