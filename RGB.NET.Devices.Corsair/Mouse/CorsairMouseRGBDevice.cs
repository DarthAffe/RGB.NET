// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;
using RGB.NET.Core.Exceptions;

namespace RGB.NET.Devices.Corsair
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a corsair mouse.
    /// </summary>
    public class CorsairMouseRGBDevice : CorsairRGBDevice<CorsairMouseRGBDeviceInfo>
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
                    InitializeLed(new CorsairLedId(this, CorsairLedIds.B1), new Rectangle(0, 0, 10, 10));
                    break;
                case CorsairPhysicalMouseLayout.Zones2:
                    InitializeLed(new CorsairLedId(this, CorsairLedIds.B1), new Rectangle(0, 0, 10, 10));
                    InitializeLed(new CorsairLedId(this, CorsairLedIds.B2), new Rectangle(10, 0, 10, 10));
                    break;
                case CorsairPhysicalMouseLayout.Zones3:
                    InitializeLed(new CorsairLedId(this, CorsairLedIds.B1), new Rectangle(0, 0, 10, 10));
                    InitializeLed(new CorsairLedId(this, CorsairLedIds.B2), new Rectangle(10, 0, 10, 10));
                    InitializeLed(new CorsairLedId(this, CorsairLedIds.B3), new Rectangle(20, 0, 10, 10));
                    break;
                case CorsairPhysicalMouseLayout.Zones4:
                    InitializeLed(new CorsairLedId(this, CorsairLedIds.B1), new Rectangle(0, 0, 10, 10));
                    InitializeLed(new CorsairLedId(this, CorsairLedIds.B2), new Rectangle(10, 0, 10, 10));
                    InitializeLed(new CorsairLedId(this, CorsairLedIds.B3), new Rectangle(20, 0, 10, 10));
                    InitializeLed(new CorsairLedId(this, CorsairLedIds.B4), new Rectangle(30, 0, 10, 10));
                    break;
                default:
                    throw new RGBDeviceException($"Can't initial mouse with layout '{DeviceInfo.PhysicalLayout}'");
            }

            ApplyLayoutFromFile(PathHelper.GetAbsolutePath($@"Layouts\Corsair\Mice\{DeviceInfo.Model.Replace(" ", string.Empty).ToUpper()}.xml"),
                null, PathHelper.GetAbsolutePath(@"Images\Corsair\Mice"));
        }

        #endregion
    }
}
