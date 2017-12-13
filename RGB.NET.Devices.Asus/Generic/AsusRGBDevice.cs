using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using RGB.NET.Core;

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

            if (Size == Size.Invalid)
            {
                Rectangle ledRectangle = new Rectangle(this.Select(x => x.LedRectangle));
                Size = ledRectangle.Size + new Size(ledRectangle.Location.X, ledRectangle.Location.Y);
            }

            ColorData = new byte[LedMapping.Count * 3];
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
                foreach (Led led in leds)
                {
                    int index = ((int)led.CustomData) * 3;
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
