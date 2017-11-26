// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Runtime.InteropServices;
using RGB.NET.Core;
using RGB.NET.Devices.Corsair.Native;

namespace RGB.NET.Devices.Corsair
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a corsair keyboard.
    /// </summary>
    public class CorsairKeyboardRGBDevice : CorsairRGBDevice<CorsairKeyboardRGBDeviceInfo>
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Corsair.CorsairKeyboardRGBDevice" /> class.
        /// </summary>
        /// <param name="info">The specific information provided by CUE for the keyboard</param>
        internal CorsairKeyboardRGBDevice(CorsairKeyboardRGBDeviceInfo info)
            : base(info)
        { }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void InitializeLayout()
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

            string model = DeviceInfo.Model.Replace(" ", string.Empty).ToUpper();
            ApplyLayoutFromFile(PathHelper.GetAbsolutePath(
                $@"Layouts\Corsair\Keyboards\{model}\{DeviceInfo.PhysicalLayout.ToString().ToUpper()}.xml"),
                DeviceInfo.LogicalLayout.ToString(), PathHelper.GetAbsolutePath(@"Images\Corsair\Keyboards"));
        }

        #endregion
    }
}
