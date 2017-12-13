using System;
using System.Collections.Generic;
using System.Linq;
using RGB.NET.Core;
using RGB.NET.Devices.CoolerMaster.Native;

namespace RGB.NET.Devices.CoolerMaster
{
    /// <inheritdoc cref="AbstractRGBDevice{TDeviceInfo}" />
    /// <inheritdoc cref="ICoolerMasterRGBDevice" />
    /// <summary>
    /// Represents a generic CoolerMaster-device. (keyboard, mouse, headset, mousepad).
    /// </summary>
    public abstract class CoolerMasterRGBDevice<TDeviceInfo> : AbstractRGBDevice<TDeviceInfo>, ICoolerMasterRGBDevice
        where TDeviceInfo : CoolerMasterRGBDeviceInfo
    {
        #region Properties & Fields
        /// <inheritdoc />
        /// <summary>
        /// Gets information about the <see cref="T:RGB.NET.Devices.CoolerMaster.CoolerMasterRGBDevice" />.
        /// </summary>
        public override TDeviceInfo DeviceInfo { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CoolerMasterRGBDevice{TDeviceInfo}"/> class.
        /// </summary>
        /// <param name="info">The generic information provided by CoolerMaster for the device.</param>
        protected CoolerMasterRGBDevice(TDeviceInfo info)
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
                _CoolerMasterSDK.SetControlDevice(DeviceInfo.DeviceIndex);

                foreach (Led led in leds)
                {
                    (int row, int column) = ((int, int))led.CustomData;
                    _CoolerMasterSDK.SetLedColor(row, column, led.Color.R, led.Color.G, led.Color.B);
                }

                _CoolerMasterSDK.RefreshLed(false);
            }
        }

        /// <inheritdoc cref="IDisposable.Dispose" />
        /// <inheritdoc cref="AbstractRGBDevice{TDeviceInfo}.Dispose" />
        public override void Dispose()
        {
            _CoolerMasterSDK.SetControlDevice(DeviceInfo.DeviceIndex);
            _CoolerMasterSDK.EnableLedControl(false);

            base.Dispose();
        }

        #endregion
    }
}
