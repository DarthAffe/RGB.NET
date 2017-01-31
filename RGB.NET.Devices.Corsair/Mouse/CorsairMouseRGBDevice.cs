// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System.IO;
using System.Reflection;
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

        /// <inheritdoc />
        protected override void InitializeLayout()
        {
            switch (MouseDeviceInfo.PhysicalLayout)
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
                    throw new RGBDeviceException($"Can't initial mouse with layout '{MouseDeviceInfo.PhysicalLayout}'");
            }

            ApplyLayoutFromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
                $@"Layouts\Corsair\String\{MouseDeviceInfo.Model.Replace(" ", string.Empty).ToUpper()}.xml"));
        }

        #endregion
    }
}
