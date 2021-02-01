// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using RGB.NET.Core;

namespace RGB.NET.Devices.Corsair
{
    /// <inheritdoc cref="CorsairRGBDevice{TDeviceInfo}" />
    /// <summary>
    /// Represents a corsair mouse.
    /// </summary>
    public class CorsairMouseRGBDevice : CorsairRGBDevice<CorsairMouseRGBDeviceInfo>, IMouse
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Corsair.CorsairMouseRGBDevice" /> class.
        /// </summary>
        /// <param name="info">The specific information provided by CUE for the mouse</param>
        internal CorsairMouseRGBDevice(CorsairMouseRGBDeviceInfo info)
            : base(info)
        { }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void InitializeLayout()
        {
            switch (DeviceInfo.PhysicalLayout)
            {
                case CorsairPhysicalMouseLayout.Zones1:
                    AddLed(LedId.Mouse1, new Point(0, 0), new Size(10, 10));
                    break;
                case CorsairPhysicalMouseLayout.Zones2:
                    AddLed(LedId.Mouse1, new Point(0, 0), new Size(10, 10));
                    AddLed(LedId.Mouse2, new Point(10, 0), new Size(10, 10));
                    break;
                case CorsairPhysicalMouseLayout.Zones3:
                    AddLed(LedId.Mouse1, new Point(0, 0), new Size(10, 10));
                    AddLed(LedId.Mouse2, new Point(10, 0), new Size(10, 10));
                    AddLed(LedId.Mouse3, new Point(20, 0), new Size(10, 10));
                    break;
                case CorsairPhysicalMouseLayout.Zones4:
                    AddLed(LedId.Mouse1, new Point(0, 0), new Size(10, 10));
                    AddLed(LedId.Mouse2, new Point(10, 0), new Size(10, 10));
                    AddLed(LedId.Mouse3, new Point(20, 0), new Size(10, 10));
                    AddLed(LedId.Mouse4, new Point(30, 0), new Size(10, 10));
                    break;
                default:
                    throw new RGBDeviceException($"Can't initialize mouse with layout '{DeviceInfo.PhysicalLayout}'");
            }
        }

        protected override object? GetLedCustomData(LedId ledId)
        {
            if (string.Equals(DeviceInfo.Model, "GLAIVE RGB", StringComparison.OrdinalIgnoreCase))
                return MouseIdMapping.GLAIVE.TryGetValue(ledId, out CorsairLedId id) ? id : CorsairLedId.Invalid;
            else if (string.Equals(DeviceInfo.Model, "M65 RGB ELITE", StringComparison.OrdinalIgnoreCase))
                return MouseIdMapping.M65_RGB_ELITE.TryGetValue(ledId, out CorsairLedId id) ? id : CorsairLedId.Invalid;
            else
                return MouseIdMapping.DEFAULT.TryGetValue(ledId, out CorsairLedId id) ? id : CorsairLedId.Invalid;
        }

        #endregion
    }
}
