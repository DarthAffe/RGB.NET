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
    /// <summary>
    /// Represents a corsair mousemat.
    /// </summary>
    public class CorsairMousematRGBDevice : CorsairRGBDevice
    {
        #region Properties & Fields

        /// <summary>
        /// Gets information about the <see cref="CorsairMousematRGBDevice"/>.
        /// </summary>
        public CorsairMousematRGBDeviceInfo MousematDeviceInfo { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CorsairMousematRGBDevice"/> class.
        /// </summary>
        /// <param name="info">The specific information provided by CUE for the mousemat</param>
        internal CorsairMousematRGBDevice(CorsairMousematRGBDeviceInfo info)
            : base(info)
        {
            this.MousematDeviceInfo = info;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the <see cref="Led"/> of the mousemat.
        /// </summary>
        protected override void InitializeLeds()
        {
            _CorsairLedPositions nativeLedPositions =
                (_CorsairLedPositions)
                Marshal.PtrToStructure(_CUESDK.CorsairGetLedPositionsByDeviceIndex(MousematDeviceInfo.CorsairDeviceIndex),
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
        }

        #endregion
    }
}
