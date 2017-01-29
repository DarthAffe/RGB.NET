// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using RGB.NET.Devices.Corsair.Native;

namespace RGB.NET.Devices.Corsair
{
    /// <summary>
    /// Represents a generic information for a <see cref="CorsairKeyboardRGBDevice"/>.
    /// </summary>
    public class CorsairKeyboardRGBDeviceInfo : CorsairRGBDeviceInfo
    {
        #region Properties & Fields

        /// <summary>
        /// Gets the physical layout of the keyboard.
        /// </summary>
        public CorsairPhysicalKeyboardLayout PhysicalLayout { get; }

        /// <summary>
        /// Gets the logical layout of the keyboard as set in CUE settings.
        /// </summary>
        public CorsairLogicalKeyboardLayout LogicalLayout { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Internal constructor of managed <see cref="CorsairKeyboardRGBDeviceInfo"/>.
        /// </summary>
        /// <param name="deviceIndex">The index of the <see cref="CorsairKeyboardRGBDevice"/>.</param>
        /// <param name="nativeInfo">The native <see cref="_CorsairDeviceInfo" />-struct</param>
        internal CorsairKeyboardRGBDeviceInfo(int deviceIndex, _CorsairDeviceInfo nativeInfo)
            : base(deviceIndex, Core.RGBDeviceType.Keyboard, nativeInfo)
        {
            this.PhysicalLayout = (CorsairPhysicalKeyboardLayout)nativeInfo.physicalLayout;
            this.LogicalLayout = (CorsairLogicalKeyboardLayout)nativeInfo.logicalLayout;

            Image = new Uri($"pack://application:,,,/RGB.NET.Devices.Corsair;component/Images/Keyboards/{Model.Replace(" ", string.Empty).ToUpper()}/{LogicalLayout.ToString().ToUpper()}.png", UriKind.Absolute);
        }

        #endregion
    }
}
