using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RGB.NET.Core;
using RGB.NET.Core.Layout;
using Sanford.Multimedia.Midi;

namespace RGB.NET.Devices.Novation
{
    /// <summary>
    /// Represents a generic Novation-device. (keyboard, mouse, headset, mousepad).
    /// </summary>
    public abstract class NovationRGBDevice : AbstractRGBDevice
    {
        #region Properties & Fields

        private readonly OutputDevice _outputDevice;

        /// <summary>
        /// Gets information about the <see cref="NovationRGBDevice"/>.
        /// </summary>
        public override IRGBDeviceInfo DeviceInfo { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NovationRGBDevice"/> class.
        /// </summary>
        /// <param name="info">The generic information provided by Novation for the device.</param>
        protected NovationRGBDevice(IRGBDeviceInfo info)
        {
            this.DeviceInfo = info;
            _outputDevice = new OutputDevice(((NovationRGBDeviceInfo)DeviceInfo).DeviceId);
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
                        NovationLedIds ledId;
                        if (Enum.TryParse(layoutLed.Id, true, out ledId))
                        {
                            Led led;
                            if (LedMapping.TryGetValue(new NovationLedId(this, ledId), out led))
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
                    NovationLedId ledId = (NovationLedId)led.Id;

                    int color = 0;

                    if (led.Color.R > 0)
                    {
                        color = 1;

                        if (led.Color.R > 127)
                            color = 2;
                        if (led.Color.R == 255)
                            color = 3;
                    }

                    if (led.Color.G > 0)
                    {
                        color = 16;

                        if (led.Color.G > 127)
                            color = 32;
                        if (led.Color.G == 255)
                            color = 48;
                    }

                    if ((led.Color.R > 0) && (led.Color.G > 0))
                    {
                        color = 17;

                        if(((led.Color.R > 127) && (led.Color.G < 127)) || ((led.Color.R < 127) && (led.Color.G > 127)))
                            color = 34;
                        if((led.Color.R > 127) && (led.Color.G > 127))
                            color = 51;
                    }

                    SendMessage(ledId.LedId.GetStatus(), ledId.LedId.GetId(), color);
                }
            }
        }

        private void SendMessage(int status, int data1, int data2)
        {
            ShortMessage shortMessage = new ShortMessage(Convert.ToByte(status), Convert.ToByte(data1), Convert.ToByte(data2));
            _outputDevice.SendShort(shortMessage.Message);
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            _outputDevice.Dispose();

            base.Dispose();
        }

        #endregion
    }
}
