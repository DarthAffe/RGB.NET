using System.Collections.Generic;
using System.Linq;
using RGB.NET.Core;

namespace RGB.NET.Devices.Msi
{
    /// <inheritdoc cref="AbstractRGBDevice{TDeviceInfo}" />
    /// <inheritdoc cref="IMsiRGBDevice" />
    /// <summary>
    /// Represents a generic MSI-device. (keyboard, mouse, headset, mousepad).
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

        /// <summary>
        /// Gets or sets the update queue performing updates for this device.
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        protected MsiDeviceUpdateQueue DeviceUpdateQueue { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MsiRGBDevice{TDeviceInfo}"/> class.
        /// </summary>
        /// <param name="info">The generic information provided by MSI for the device.</param>
        protected MsiRGBDevice(TDeviceInfo info)
        {
            this.DeviceInfo = info;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the device.
        /// </summary>
        public void Initialize(MsiDeviceUpdateQueue updateQueue, int ledCount)
        {
            DeviceUpdateQueue = updateQueue;

            InitializeLayout(ledCount);

            if (Size == Size.Invalid)
            {
                Rectangle ledRectangle = new Rectangle(this.Select(x => x.LedRectangle));
                Size = ledRectangle.Size + new Size(ledRectangle.Location.X, ledRectangle.Location.Y);
            }
        }

        /// <summary>
        /// Initializes the <see cref="Led"/> and <see cref="Size"/> of the device.
        /// </summary>
        protected abstract void InitializeLayout(int ledCount);

        /// <inheritdoc />
        protected override void UpdateLeds(IEnumerable<Led> ledsToUpdate)
            => DeviceUpdateQueue.SetData(ledsToUpdate.Where(x => (x.Color.A > 0) && (x.CustomData is int)));

        #endregion
    }
}
