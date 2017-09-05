// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using RGB.NET.Core;
using RGB.NET.Devices.Corsair.Native;

namespace RGB.NET.Devices.Corsair
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a corsair mousepad.
    /// </summary>
    public class CorsairMousepadRGBDevice : CorsairRGBDevice
    {
        #region Properties & Fields

        /// <summary>
        /// Gets information about the <see cref="CorsairMousepadRGBDevice"/>.
        /// </summary>
        public CorsairMousepadRGBDeviceInfo MousepadDeviceInfo { get; }

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Corsair.CorsairMousepadRGBDevice" /> class.
        /// </summary>
        /// <param name="info">The specific information provided by CUE for the mousepad</param>
        internal CorsairMousepadRGBDevice(CorsairMousepadRGBDeviceInfo info)
            : base(info)
        {
            this.MousepadDeviceInfo = info;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void InitializeLayout()
        {
            _CorsairLedPositions nativeLedPositions =
                (_CorsairLedPositions)
                Marshal.PtrToStructure(_CUESDK.CorsairGetLedPositionsByDeviceIndex(MousepadDeviceInfo.CorsairDeviceIndex),
                                       typeof(_CorsairLedPositions));

            int structSize = Marshal.SizeOf(typeof(_CorsairLedPosition));
            IntPtr ptr = nativeLedPositions.pLedPosition;

            List<_CorsairLedPosition> positions = new List<_CorsairLedPosition>();
            for (int i = 0; i < nativeLedPositions.numberOfLed; i++)
            {
                _CorsairLedPosition ledPosition = (_CorsairLedPosition)Marshal.PtrToStructure(ptr, typeof(_CorsairLedPosition));
                ptr = new IntPtr(ptr.ToInt64() + structSize);
                positions.Add(ledPosition);
            }

            foreach (_CorsairLedPosition ledPosition in positions.OrderBy(p => p.ledId))
                InitializeLed(new CorsairLedId(this, ledPosition.ledId),
                              new Rectangle(ledPosition.left, ledPosition.top, ledPosition.width, ledPosition.height));

            ApplyLayoutFromFile(PathHelper.GetAbsolutePath($@"Layouts\Corsair\Mousepads\{MousepadDeviceInfo.Model.Replace(" ", string.Empty).ToUpper()}.xml"),
                null, PathHelper.GetAbsolutePath(@"Images\Corsair\Mousepads"));
        }

        #endregion
    }
}
