using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using RGB.NET.Core;
using RGB.NET.Devices.Corsair.Native;

namespace RGB.NET.Devices.Corsair
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a generic information for a Corsair-<see cref="T:RGB.NET.Core.IRGBDevice" />.
    /// </summary>
    public class CorsairRGBDeviceInfo : IRGBDeviceInfo
    {
        #region Properties & Fields

        /// <summary>
        /// Gets the corsair specific device type.
        /// </summary>
        public CorsairDeviceType CorsairDeviceType { get; }

        /// <summary>
        /// Gets the index of the <see cref="CorsairRGBDevice{TDeviceInfo}"/>.
        /// </summary>
        public int CorsairDeviceIndex { get; }

        /// <inheritdoc />
        public RGBDeviceType DeviceType { get; }

        /// <inheritdoc />
        public string DeviceName { get; }

        /// <inheritdoc />
        public string Manufacturer => "Corsair";

        /// <inheritdoc />
        public string Model { get; }

        public object? LayoutMetadata { get; set; }

        /// <summary>
        /// Gets a flag that describes device capabilities. (<see cref="CorsairDeviceCaps" />)
        /// </summary>
        public CorsairDeviceCaps CapsMask { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Internal constructor of managed <see cref="CorsairRGBDeviceInfo"/>.
        /// </summary>
        /// <param name="deviceIndex">The index of the <see cref="CorsairRGBDevice{TDeviceInfo}"/>.</param>
        /// <param name="deviceType">The type of the <see cref="IRGBDevice"/>.</param>
        /// <param name="nativeInfo">The native <see cref="_CorsairDeviceInfo" />-struct</param>
        /// <param name="modelCounter">A dictionary containing counters to create unique names for equal devices models.</param>
        internal CorsairRGBDeviceInfo(int deviceIndex, RGBDeviceType deviceType, _CorsairDeviceInfo nativeInfo, Dictionary<string, int> modelCounter)
        {
            this.CorsairDeviceIndex = deviceIndex;
            this.DeviceType = deviceType;
            this.CorsairDeviceType = nativeInfo.type;
            this.Model = nativeInfo.model == IntPtr.Zero ? string.Empty : Regex.Replace(Marshal.PtrToStringAnsi(nativeInfo.model) ?? string.Empty, " ?DEMO", string.Empty, RegexOptions.IgnoreCase);
            this.CapsMask = (CorsairDeviceCaps)nativeInfo.capsMask;

            DeviceName = GetUniqueModelName(modelCounter);
        }

        /// <summary>
        /// Internal constructor of managed <see cref="CorsairRGBDeviceInfo"/>.
        /// </summary>
        /// <param name="deviceIndex">The index of the <see cref="CorsairRGBDevice{TDeviceInfo}"/>.</param>
        /// <param name="deviceType">The type of the <see cref="IRGBDevice"/>.</param>
        /// <param name="nativeInfo">The native <see cref="_CorsairDeviceInfo" />-struct</param>
        /// <param name="modelName">The name of the device-model (overwrites the one provided with the device info).</param>
        /// <param name="modelCounter">A dictionary containing counters to create unique names for equal devices models.</param>
        internal CorsairRGBDeviceInfo(int deviceIndex, RGBDeviceType deviceType, _CorsairDeviceInfo nativeInfo, string modelName, Dictionary<string, int> modelCounter)
        {
            this.CorsairDeviceIndex = deviceIndex;
            this.DeviceType = deviceType;
            this.CorsairDeviceType = nativeInfo.type;
            this.Model = modelName;
            this.CapsMask = (CorsairDeviceCaps)nativeInfo.capsMask;

            DeviceName = GetUniqueModelName(modelCounter);
        }

        #endregion

        #region Methods

        private string GetUniqueModelName(Dictionary<string, int> modelCounter)
        {
            if (modelCounter.TryGetValue(Model, out int _))
            {
                int counter = ++modelCounter[Model];
                return $"{Manufacturer} {Model} {counter}";
            }
            else
            {
                modelCounter.Add(Model, 1);
                return $"{Manufacturer} {Model}";
            }
        }

        #endregion
    }
}
