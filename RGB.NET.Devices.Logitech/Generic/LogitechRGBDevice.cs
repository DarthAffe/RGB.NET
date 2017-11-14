using System;
using System.IO;
using System.Linq;
using RGB.NET.Core;
using RGB.NET.Core.Layout;

namespace RGB.NET.Devices.Logitech
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a generic Logitech-device. (keyboard, mouse, headset, mousepad).
    /// </summary>
    public abstract class LogitechRGBDevice : AbstractRGBDevice
    {
        #region Properties & Fields

        /// <inheritdoc />
        /// <summary>
        /// Gets information about the <see cref="T:RGB.NET.Devices.Logitech.LogitechRGBDevice" />.
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
        protected virtual void InitializeLayout()
        {
            if (!(DeviceInfo is LogitechRGBDeviceInfo info)) return;
            string basePath = info.ImageBasePath;
            string layout = info.ImageLayout;
            string layoutPath = info.LayoutPath;
            ApplyLayoutFromFile(PathHelper.GetAbsolutePath($@"Layouts\Logitech\{basePath}\{layoutPath}.xml"), layout, PathHelper.GetAbsolutePath($@"Images\Logitech\{basePath}"));
        }

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
                        if (Enum.TryParse(layoutLed.Id, true, out LogitechLedIds ledId))
                        {
                            LogitechLedId id = new LogitechLedId(this, ledId);
                            if (!LedMapping.TryGetValue(id, out Led led))
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

        #endregion
    }
}
