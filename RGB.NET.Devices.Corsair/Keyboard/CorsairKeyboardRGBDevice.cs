// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Runtime.InteropServices;
using RGB.NET.Core;
using RGB.NET.Devices.Corsair.Native;

namespace RGB.NET.Devices.Corsair
{
    /// <summary>
    /// Represents a corsair keyboard.
    /// </summary>
    public class CorsairKeyboardRGBDevice : CorsairRGBDevice
    {
        #region Properties & Fields

        /// <summary>
        /// Gets information about the <see cref="CorsairKeyboardRGBDevice"/>.
        /// </summary>
        public CorsairKeyboardRGBDeviceInfo KeyboardDeviceInfo { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CorsairKeyboardRGBDevice"/> class.
        /// </summary>
        /// <param name="info">The specific information provided by CUE for the keyboard</param>
        internal CorsairKeyboardRGBDevice(CorsairKeyboardRGBDeviceInfo info)
            : base(info)
        {
            this.KeyboardDeviceInfo = info;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the <see cref="Led"/> of the keyboard.
        /// </summary>
        protected override void InitializeLeds()
        {
            _CorsairLedPositions nativeLedPositions =
                (_CorsairLedPositions)Marshal.PtrToStructure(_CUESDK.CorsairGetLedPositions(), typeof(_CorsairLedPositions));

            int structSize = Marshal.SizeOf(typeof(_CorsairLedPosition));
            IntPtr ptr = nativeLedPositions.pLedPosition;

            for (int i = 0; i < nativeLedPositions.numberOfLed; i++)
            {
                _CorsairLedPosition ledPosition = (_CorsairLedPosition)Marshal.PtrToStructure(ptr, typeof(_CorsairLedPosition));
                InitializeLed(new CorsairLedId(this, ledPosition.ledId),
                              new Rectangle(ledPosition.left, ledPosition.top, ledPosition.width, ledPosition.height));

                ptr = new IntPtr(ptr.ToInt64() + structSize);
            }
        }

        #endregion
    }
}
