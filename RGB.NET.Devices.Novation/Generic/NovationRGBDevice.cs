using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RGB.NET.Core;
using RGB.NET.Core.Layout;
using Sanford.Multimedia.Midi;

namespace RGB.NET.Devices.Novation
{
    /// <inheritdoc cref="AbstractRGBDevice{TDeviceInfo}" />
    /// <inheritdoc cref="INovationRGBDevice" />
    /// <summary>
    /// Represents a generic Novation-device. (launchpad).
    /// </summary>
    public abstract class NovationRGBDevice<TDeviceInfo> : AbstractRGBDevice<TDeviceInfo>, INovationRGBDevice
        where TDeviceInfo : NovationRGBDeviceInfo
    {
        #region Properties & Fields

        private readonly OutputDevice _outputDevice;
        private readonly TDeviceInfo _deviceInfo;

        /// <inheritdoc />
        /// <summary>
        /// Gets information about the <see cref="T:RGB.NET.Devices.Novation.NovationRGBDevice" />.
        /// </summary>
        public override TDeviceInfo DeviceInfo => _deviceInfo;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NovationRGBDevice{TDeviceInfo}"/> class.
        /// </summary>
        /// <param name="info">The generic information provided by Novation for the device.</param>
        protected NovationRGBDevice(TDeviceInfo info)
        {
            this._deviceInfo = info;

            _outputDevice = new OutputDevice(info.DeviceId);
        }

        #endregion

        #region Methods


        /// <summary>
        /// Initializes the device.
        /// </summary>
        public void Initialize()
        {
            InitializeLayout();

            if (Size == Size.Invalid)
            {
                Rectangle ledRectangle = new Rectangle(this.Select(x => x.LedRectangle));
                Size = ledRectangle.Size + new Size(ledRectangle.Location.X, ledRectangle.Location.Y);
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

                Size = new Size(layout.Width, layout.Height);

                if (layout.Leds != null)
                    foreach (LedLayout layoutLed in layout.Leds)
                    {
                        if (Enum.TryParse(layoutLed.Id, true, out NovationLedIds ledId))
                        {
                            if (LedMapping.TryGetValue(new NovationLedId(this, ledId), out Led led))
                            {
                                led.LedRectangle.Location = new Point(layoutLed.X, layoutLed.Y);
                                led.LedRectangle.Size = new Size(layoutLed.Width, layoutLed.Height);

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
                    NovationLedId ledId = led.Id as NovationLedId;
                    if (ledId == null) continue;

                    int color = ConvertColor(led.Color);
                    SendMessage(ledId.LedId.GetStatus(), ledId.LedId.GetId(), color);
                }
            }
        }

        /// <summary>
        /// Resets the <see cref="NovationRGBDevice{TDeviceInfo}"/> back top default.
        /// </summary>
        public virtual void Reset() => SendMessage(0xB0, 0, 0);

        /// <summary>
        /// Convert a <see cref="Color"/> to its novation-representation depending on the <see cref="NovationColorCapabilities"/> of the <see cref="NovationRGBDevice{TDeviceInfo}"/>.
        /// </summary>
        /// <param name="color">The <see cref="Color"/> to convert.</param>
        /// <returns>The novation-representation of the <see cref="Color"/>.</returns>
        protected virtual int ConvertColor(Color color)
        {
            switch (_deviceInfo.ColorCapabilities)
            {
                case NovationColorCapabilities.RGB:
                    return ConvertColorFull(color);
                case NovationColorCapabilities.LimitedRG:
                    return ConvertColorLimited(color);
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Convert a <see cref="Color"/> to its novation-representation depending on the <see cref="NovationColorCapabilities"/> of the <see cref="NovationRGBDevice{TDeviceInfo}"/>.
        /// The conversion uses the full rgb-range.
        /// </summary>
        /// <param name="color">The <see cref="Color"/> to convert.</param>
        /// <returns>The novation-representation of the <see cref="Color"/>.</returns>
        protected virtual int ConvertColorFull(Color color)
        {
            //TODO DarthAffe 16.08.2017: How are colors for full rgb devices encoded?
            return 0;
        }

        /// <summary>
        /// Convert a <see cref="Color"/> to its novation-representation depending on the <see cref="NovationColorCapabilities"/> of the <see cref="NovationRGBDevice{TDeviceInfo}"/>.
        /// The conversion uses only a limited amount of colors (3 red, 3 yellow, 3 green).
        /// </summary>
        /// <param name="color">The <see cref="Color"/> to convert.</param>
        /// <returns>The novation-representation of the <see cref="Color"/>.</returns>
        protected virtual int ConvertColorLimited(Color color)
        {
            if ((color.Hue >= 330) || (color.Hue < 30))
                return (int)Math.Ceiling(color.Value * 3); // red with brightness 1, 2 or 3

            if ((color.Hue >= 30) && (color.Hue < 90)) // yellow with brightness 17, 34 or 51
                return (int)Math.Ceiling(color.Value * 3) * 17;

            if ((color.Hue >= 90) && (color.Hue < 150)) // green with brightness 16, 32 or 48
                return (int)Math.Ceiling(color.Value * 3) * 16;

            return 0;
        }

        /// <summary>
        /// Sends a message to the <see cref="NovationRGBDevice{TDeviceInfo}"/>.
        /// </summary>
        /// <param name="status">The status-code of the message.</param>
        /// <param name="data1">The first data-package of the message.</param>
        /// <param name="data2">The second data-package of the message.</param>
        protected virtual void SendMessage(int status, int data1, int data2)
        {
            ShortMessage shortMessage = new ShortMessage(Convert.ToByte(status), Convert.ToByte(data1), Convert.ToByte(data2));
            _outputDevice.SendShort(shortMessage.Message);
        }

        /// <inheritdoc cref="IDisposable.Dispose" />
        /// <inheritdoc cref="AbstractRGBDevice{TDeviceInfo}.Dispose" />
        public override void Dispose()
        {
            Reset();
            _outputDevice.Dispose();

            base.Dispose();
        }

        #endregion
    }
}
