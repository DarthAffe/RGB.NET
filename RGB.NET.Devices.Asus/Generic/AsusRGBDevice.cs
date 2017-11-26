using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using RGB.NET.Core;
using RGB.NET.Core.Layout;

namespace RGB.NET.Devices.Asus
{
    /// <inheritdoc cref="AbstractRGBDevice{TDeviceInfo}" />
    /// <inheritdoc cref="IAsusRGBDevice" />
    /// <summary>
    /// Represents a generic Asus-device. (keyboard, mouse, headset, mousepad).
    /// </summary>
    public abstract class AsusRGBDevice<TDeviceInfo> : AbstractRGBDevice<TDeviceInfo>, IAsusRGBDevice
        where TDeviceInfo : AsusRGBDeviceInfo
    {
        #region Properties & Fields

        /// <summary>
        /// Gets or sets the internal color-data cache.
        /// </summary>
        protected byte[] ColorData { get; private set; }

        /// <inheritdoc />
        /// <summary>
        /// Gets information about the <see cref="T:RGB.NET.Devices.Asus.AsusRGBDevice" />.
        /// </summary>
        public override TDeviceInfo DeviceInfo { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AsusRGBDevice{TDeviceInfo}"/> class.
        /// </summary>
        /// <param name="info">The generic information provided by Asus for the device.</param>
        protected AsusRGBDevice(TDeviceInfo info)
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

            ColorData = new byte[LedMapping.Count * 3];
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
                        if (Enum.TryParse(layoutLed.Id, true, out AsusLedIds ledId))
                        {
                            if (LedMapping.TryGetValue(new AsusLedId(this, ledId), out Led led))
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

            if (leds.Count > 0)
            {
                foreach (Led led in leds)
                {
                    int index = (((AsusLedId)led.Id).Index) * 3;
                    ColorData[index] = led.Color.R;
                    ColorData[index + 1] = led.Color.G;
                    ColorData[index + 2] = led.Color.B;
                }

                ApplyColorData();
            }
        }

        /// <summary>
        /// Sends the color-data-cache to the device.
        /// </summary>
        protected abstract void ApplyColorData();

        /// <inheritdoc cref="IDisposable.Dispose" />
        /// <inheritdoc cref="AbstractRGBDevice{TDeviceInfo}.Dispose" />
        public override void Dispose()
        {
            if ((DeviceInfo is AsusRGBDeviceInfo deviceInfo) && (deviceInfo.Handle != IntPtr.Zero))
                Marshal.FreeHGlobal(deviceInfo.Handle);

            ColorData = null;

            base.Dispose();
        }

        #endregion
    }
}
