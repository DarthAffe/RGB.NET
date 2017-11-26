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
    /// <inheritdoc />
    /// <summary>
    /// Represents a generic CUE-device. (keyboard, mouse, headset, mousepad).
    /// </summary>
    public abstract class CorsairRGBDevice<TDeviceInfo> : AbstractRGBDevice<TDeviceInfo>, ICorsairRGBDevice
        where TDeviceInfo : CorsairRGBDeviceInfo
    {
        #region Properties & Fields

        /// <inheritdoc />
        /// <summary>
        /// Gets information about the <see cref="T:RGB.NET.Devices.Corsair.CorsairRGBDevice" />.
        /// </summary>
        public override TDeviceInfo DeviceInfo { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CorsairRGBDevice{TDeviceInfo}"/> class.
        /// </summary>
        /// <param name="info">The generic information provided by CUE for the device.</param>
        protected CorsairRGBDevice(TDeviceInfo info)
        {
            this.DeviceInfo = info;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the device.
        /// </summary>
        public void Initialize()
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
                        if (Enum.TryParse(layoutLed.Id, true, out CorsairLedIds ledId))
                        {
                            if (LedMapping.TryGetValue(new CorsairLedId(this, ledId), out Led led))
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

        /// <summary>
        /// Reads the current color-data from the device
        /// </summary>
        /// <returns>A dictionary mapping the <see cref="CorsairLedIds"/> to the current <see cref="Color"/>.</returns>
        protected Dictionary<CorsairLedIds, Color> GetColors()
        {
            int structSize = Marshal.SizeOf(typeof(_CorsairLedColor));
            IntPtr ptr = Marshal.AllocHGlobal(structSize * LedMapping.Count);
            IntPtr addPtr = new IntPtr(ptr.ToInt64());
            foreach (Led led in this)
            {
                _CorsairLedColor color = new _CorsairLedColor { ledId = (int)((CorsairLedId)led.Id).LedId };
                Marshal.StructureToPtr(color, addPtr, false);
                addPtr = new IntPtr(addPtr.ToInt64() + structSize);
            }
            _CUESDK.CorsairGetLedsColors(LedMapping.Count, ptr);

            IntPtr readPtr = ptr;
            Dictionary<CorsairLedIds, Color> colorData = new Dictionary<CorsairLedIds, Color>();
            for (int i = 0; i < LedMapping.Count; i++)
            {
                _CorsairLedColor ledColor = (_CorsairLedColor)Marshal.PtrToStructure(readPtr, typeof(_CorsairLedColor));
                colorData.Add((CorsairLedIds)ledColor.ledId, new Color((byte)ledColor.r, (byte)ledColor.g, (byte)ledColor.b));

                readPtr = new IntPtr(readPtr.ToInt64() + structSize);
            }

            Marshal.FreeHGlobal(ptr);

            return colorData;
        }

        #endregion
    }
}
