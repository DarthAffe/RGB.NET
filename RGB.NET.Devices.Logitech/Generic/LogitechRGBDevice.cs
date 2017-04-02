using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RGB.NET.Core;
using RGB.NET.Core.Layout;
using RGB.NET.Devices.Logitech.Native;

namespace RGB.NET.Devices.Logitech
{
    /// <summary>
    /// Represents a generic Logitech-device. (keyboard, mouse, headset, mousmat).
    /// </summary>
    public abstract class LogitechRGBDevice : AbstractRGBDevice
    {
        #region Properties & Fields

        /// <summary>
        /// Gets information about the <see cref="LogitechRGBDevice"/>.
        /// </summary>
        public override IRGBDeviceInfo DeviceInfo { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LogitechRGBDevice"/> class.
        /// </summary>
        /// <param name="info">The generic information provided by Logitech for the device.</param>
        protected LogitechRGBDevice(IRGBDeviceInfo info)
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
                        LogitechLedIds ledId;
                        if (Enum.TryParse(layoutLed.Id, true, out ledId))
                        {
                            LogitechLedId id = new LogitechLedId(this, ledId);
                            Led led;
                            if (!LedMapping.TryGetValue(id, out led))
                                led = InitializeLed(id, new Rectangle());

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

        /// <inheritdoc />
        protected override void UpdateLeds(IEnumerable<Led> ledsToUpdate)
        {
            _LogitechGSDK.LogiLedSetTargetDevice(LogitechDeviceCaps.PerKeyRGB);

            List<Led> leds = ledsToUpdate.Where(x => x.Color.A > 0).ToList();

            byte[] bitmap = null;
            foreach (Led led in leds)
            {
                //TODO DarthAffe 26.03.2017: This is only needed since update by name doesn't work as expected for all keys ...
                int bitmapOffset;
                if (BitmapMapping.BitmapOffset.TryGetValue(((LogitechLedId)led.Id).LedId, out bitmapOffset))
                {
                    if (bitmap == null)
                        bitmap = BitmapMapping.CreateBitmap();

                    BitmapMapping.SetColor(ref bitmap, bitmapOffset, led.Color);
                }
                else
                    _LogitechGSDK.LogiLedSetLightingForKeyWithKeyName((int)((LogitechLedId)led.Id).LedId,
                                                                      (int)Math.Round(led.Color.RPercent * 100),
                                                                      (int)Math.Round(led.Color.GPercent * 100),
                                                                      (int)Math.Round(led.Color.BPercent * 100));
            }

            if (bitmap != null)
                _LogitechGSDK.LogiLedSetLightingFromBitmap(bitmap);
        }

        #endregion
    }
}
