using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using RGB.NET.Core;
using RGB.NET.Devices.Corsair.Native;

namespace RGB.NET.Devices.Corsair
{
    /// <summary>
    /// Represents a generic CUE-device. (keyboard, mouse, headset, mousmat).
    /// </summary>
    public abstract class CorsairRGBDevice : AbstractRGBDevice
    {
        #region Properties & Fields

        /// <summary>
        /// Gets information about the <see cref="CorsairRGBDevice"/>.
        /// </summary>
        public override IRGBDeviceInfo DeviceInfo { get; }

        /// <inheritdoc />
        protected override Size InternalSize { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CorsairRGBDevice"/> class.
        /// </summary>
        /// <param name="info">The generic information provided by CUE for the device.</param>
        protected CorsairRGBDevice(IRGBDeviceInfo info)
        {
            this.DeviceInfo = info;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the device.
        /// </summary>
        internal void Initialize()
        {
            InitializeLeds();

            Rectangle ledRectangle = new Rectangle(this.Select(x => x.LedRectangle));
            InternalSize = ledRectangle.Size + new Size(ledRectangle.Location.X, ledRectangle.Location.Y);
        }

        /// <summary>
        /// Initializes the <see cref="Led"/> of the device.
        /// </summary>
        protected abstract void InitializeLeds();

        /// <inheritdoc />
        protected override void UpdateLeds(IEnumerable<Led> ledsToUpdate)
        {
            List<Led> leds = ledsToUpdate.Where(x => x.Color.A > 0).ToList();

            if (leds.Count > 0) // CUE seems to crash if 'CorsairSetLedsColors' is called with a zero length array
            {
                int structSize = Marshal.SizeOf(typeof(_CorsairLedColor));
                IntPtr ptr = Marshal.AllocHGlobal(structSize * leds.Count);
                IntPtr addPtr = new IntPtr(ptr.ToInt64());
                foreach (Led led in leds)
                {
                    _CorsairLedColor color = new _CorsairLedColor
                    {
                        ledId = (int)((CorsairLedId)led.Id).LedId,
                        r = led.Color.R,
                        g = led.Color.G,
                        b = led.Color.B
                    };

                    Marshal.StructureToPtr(color, addPtr, false);
                    addPtr = new IntPtr(addPtr.ToInt64() + structSize);
                }
                _CUESDK.CorsairSetLedsColors(leds.Count, ptr);
                Marshal.FreeHGlobal(ptr);
            }
        }

        #endregion
    }
}
