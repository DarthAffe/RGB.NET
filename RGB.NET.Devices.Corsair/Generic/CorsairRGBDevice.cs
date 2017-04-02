using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using RGB.NET.Core;
using RGB.NET.Core.Layout;
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
            InitializeLayout();

            if (InternalSize == null)
            {
                Rectangle ledRectangle = new Rectangle(this.Select(x => x.LedRectangle));
                InternalSize = ledRectangle.Size + new Size(ledRectangle.Location.X, ledRectangle.Location.Y);
            }
        }

        /// <summary>
        /// Initializes the <see cref="Led"/> and <see cref="Size"/> of the device.
        /// </summary>
        protected abstract void InitializeLayout();

        /// <summary>
        /// Applies the given layout.
        /// </summary>
        /// <param name="layoutPath">The file containing the layout.</param>
        /// <param name="imageLayout">The name of the layout used to get the images of the leds.</param>
        /// <param name="imageBasePath">The path images for this device are collected in.</param>
        protected void ApplyLayoutFromFile(string layoutPath, string imageLayout, string imageBasePath)
        {
            DeviceLayout layout = DeviceLayout.Load(layoutPath);
            if (layout != null)
            {
                LedImageLayout ledImageLayout = layout.LedImageLayouts.FirstOrDefault(x => string.Equals(x.Layout, imageLayout, StringComparison.OrdinalIgnoreCase));

                InternalSize = new Size(layout.Width, layout.Height);

                if (layout.Leds != null)
                    foreach (LedLayout layoutLed in layout.Leds)
                    {
                        CorsairLedIds ledId;
                        if (Enum.TryParse(layoutLed.Id, true, out ledId))
                        {
                            Led led;
                            if (LedMapping.TryGetValue(new CorsairLedId(this, ledId), out led))
                            {
                                led.LedRectangle.Location.X = layoutLed.X;
                                led.LedRectangle.Location.Y = layoutLed.Y;
                                led.LedRectangle.Size.Width = layoutLed.Width;
                                led.LedRectangle.Size.Height = layoutLed.Height;

                                led.Shape = layoutLed.Shape;
                                led.ShapeData = layoutLed.ShapeData;

                                LedImage image = ledImageLayout?.LedImages.FirstOrDefault(x => x.Id == layoutLed.Id);
                                led.Image = (!string.IsNullOrEmpty(image?.Image))
                                    ? new Uri(Path.Combine(imageBasePath, image.Image), UriKind.Absolute)
                                    : new Uri(Path.Combine(imageBasePath, "Missing.png"), UriKind.Absolute);
                            }
                        }
                    }
            }
        }

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
