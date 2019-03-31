using System.Collections.Generic;
using System.Linq;
using RGB.NET.Core;
using RGB.NET.Devices.Msi.Native;

namespace RGB.NET.Devices.Msi
{
    /// <inheritdoc cref="AbstractRGBDevice{TDeviceInfo}" />
    /// <inheritdoc cref="IMsiRGBDevice" />
    /// <summary>
    /// Represents a generic Msi-device. (keyboard, mouse, headset, mousepad).
    /// </summary>
    public abstract class MsiRGBDevice<TDeviceInfo> : AbstractRGBDevice<TDeviceInfo>, IMsiRGBDevice
        where TDeviceInfo : MsiRGBDeviceInfo
    {
        #region Properties & Fields

        /// <inheritdoc />
        /// <summary>
        /// Gets information about the <see cref="T:RGB.NET.Devices.Msi.MsiRGBDevice" />.
        /// </summary>
        public override TDeviceInfo DeviceInfo { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MsiRGBDevice{TDeviceInfo}"/> class.
        /// </summary>
        /// <param name="info">The generic information provided by Msi for the device.</param>
        protected MsiRGBDevice(TDeviceInfo info)
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

        /// <inheritdoc />
        protected override void UpdateLeds(IEnumerable<Led> ledsToUpdate)
        {
            List<Led> leds = ledsToUpdate.Where(x => x.Color.A > 0).ToList();

            if (leds.Count > 0)
            {
                string deviceType = DeviceInfo.MsiDeviceType;
                foreach (Led led in leds)
                    _MsiSDK.SetLedColor(deviceType, (int)led.CustomData, led.Color.GetR(), led.Color.GetG(), led.Color.GetB());
            }
        }

        #endregion
    }
}
