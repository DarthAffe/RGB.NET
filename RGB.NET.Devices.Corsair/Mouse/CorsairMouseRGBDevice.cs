// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;
using RGB.NET.Core.Exceptions;

namespace RGB.NET.Devices.Corsair
{
    /// <summary>
    /// Represents a corsair mouse.
    /// </summary>
    public class CorsairMouseRGBDevice : CorsairRGBDevice
    {
        #region Properties & Fields

        /// <summary>
        /// Gets information about the <see cref="CorsairMouseRGBDevice"/>.
        /// </summary>
        public CorsairMouseRGBDeviceInfo MouseDeviceInfo { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CorsairMouseRGBDevice"/> class.
        /// </summary>
        /// <param name="info">The specific information provided by CUE for the mouse</param>
        internal CorsairMouseRGBDevice(CorsairMouseRGBDeviceInfo info)
            : base(info)
        {
            this.MouseDeviceInfo = info;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the <see cref="Led"/> of the mouse.
        /// </summary>
        protected override void InitializeLeds()
        {
            switch (MouseDeviceInfo.PhysicalLayout)
            {
                case CorsairPhysicalMouseLayout.Zones1:
                    InitializeLed(new CorsairLedId(CorsairLedIds.B1), new Rectangle(0, 0, 10, 10));
                    break;
                case CorsairPhysicalMouseLayout.Zones2:
                    InitializeLed(new CorsairLedId(CorsairLedIds.B1), new Rectangle(0, 0, 10, 10));
                    InitializeLed(new CorsairLedId(CorsairLedIds.B2), new Rectangle(10, 0, 10, 10));
                    break;
                case CorsairPhysicalMouseLayout.Zones3:
                    InitializeLed(new CorsairLedId(CorsairLedIds.B1), new Rectangle(0, 0, 10, 10));
                    InitializeLed(new CorsairLedId(CorsairLedIds.B2), new Rectangle(10, 0, 10, 10));
                    InitializeLed(new CorsairLedId(CorsairLedIds.B3), new Rectangle(20, 0, 10, 10));
                    break;
                case CorsairPhysicalMouseLayout.Zones4:
                    InitializeLed(new CorsairLedId(CorsairLedIds.B1), new Rectangle(0, 0, 10, 10));
                    InitializeLed(new CorsairLedId(CorsairLedIds.B2), new Rectangle(10, 0, 10, 10));
                    InitializeLed(new CorsairLedId(CorsairLedIds.B3), new Rectangle(20, 0, 10, 10));
                    InitializeLed(new CorsairLedId(CorsairLedIds.B4), new Rectangle(30, 0, 10, 10));
                    break;
                default:
                    throw new RGBDeviceException($"Can't initial mouse with layout '{MouseDeviceInfo.PhysicalLayout}'");
            }
        }

        #endregion
    }
}
