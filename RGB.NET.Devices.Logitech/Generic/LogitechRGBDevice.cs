using System;
using System.Collections.Generic;
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
        protected void ApplyLayoutFromFile(string layoutPath)
        {
            DeviceLayout layout = DeviceLayout.Load(layoutPath);
            if (layout != null)
            {
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
                        }
                    }
            }
        }

        /// <inheritdoc />
        protected override void UpdateLeds(IEnumerable<Led> ledsToUpdate)
        {
            List<Led> leds = ledsToUpdate.Where(x => x.Color.A > 0).ToList();

            foreach (Led led in leds)
                _LogitechGSDK.LogiLedSetLightingForKeyWithKeyName((int)((LogitechLedId)led.Id).LedId,
                                                                  (int)Math.Round(led.Color.RPercent * 100),
                                                                  (int)Math.Round(led.Color.GPercent * 100),
                                                                  (int)Math.Round(led.Color.BPercent * 100));
        }

        #endregion
    }
}
