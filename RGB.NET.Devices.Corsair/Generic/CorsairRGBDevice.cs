using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using RGB.NET.Core;
using RGB.NET.Devices.Corsair.Native;

namespace RGB.NET.Devices.Corsair
{
    /// <inheritdoc cref="AbstractRGBDevice{TDeviceInfo}" />
    /// <inheritdoc cref="ICorsairRGBDevice" />
    /// <summary>
    /// Represents a generic CUE-device. (keyboard, mouse, headset, mousepad).
    /// </summary>
    public abstract class CorsairRGBDevice<TDeviceInfo> : AbstractRGBDevice<TDeviceInfo>, ICorsairRGBDevice
        where TDeviceInfo : CorsairRGBDeviceInfo
    {
        #region Properties & Fields

        /// <inheritdoc />
        /// <summary>
        /// Gets information about the <see cref="T:RGB.NET.Devices.Corsair.CorsairRGBDevice" />.
        /// </summary>
        public override TDeviceInfo DeviceInfo { get; }

        /// <summary>
        /// Gets a dictionary containing all <see cref="Led"/> of the <see cref="CorsairRGBDevice{TDeviceInfo}"/>.
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        protected Dictionary<CorsairLedId, Led> InternalLedMapping { get; } = new Dictionary<CorsairLedId, Led>();

        /// <summary>
        /// Gets or sets the update queue performing updates for this device.
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        protected CorsairDeviceUpdateQueue DeviceUpdateQueue { get; set; }

        #endregion

        #region Indexer

        /// <summary>
        /// Gets the <see cref="Led"/> with the specified <see cref="CorsairLedId"/>.
        /// </summary>
        /// <param name="ledId">The <see cref="CorsairLedId"/> of the <see cref="Led"/> to get.</param>
        /// <returns>The <see cref="Led"/> with the specified <see cref="CorsairLedId"/> or null if no <see cref="Led"/> is found.</returns>
        // ReSharper disable once MemberCanBePrivate.Global
        public Led this[CorsairLedId ledId] => InternalLedMapping.TryGetValue(ledId, out Led led) ? led : null;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CorsairRGBDevice{TDeviceInfo}"/> class.
        /// </summary>
        /// <param name="info">The generic information provided by CUE for the device.</param>
        protected CorsairRGBDevice(TDeviceInfo info)
        {
            this.DeviceInfo = info;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the device.
        /// </summary>
        public void Initialize(CorsairDeviceUpdateQueue deviceUpdateQueue)
        {
            DeviceUpdateQueue = deviceUpdateQueue;

            InitializeLayout();

            foreach (Led led in LedMapping.Values)
            {
                CorsairLedId ledId = (CorsairLedId)led.CustomData;
                if (ledId != CorsairLedId.Invalid)
                    InternalLedMapping.Add(ledId, led);
            }

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
            => DeviceUpdateQueue.SetData(ledsToUpdate.Where(x => (x.Color.A > 0) && (x.CustomData is CorsairLedId ledId && (ledId != CorsairLedId.Invalid))));

        /// <inheritdoc />
        public override void Dispose()
        {
            try { DeviceUpdateQueue?.Dispose(); }
            catch { /* at least we tried */ }

            base.Dispose();
        }

        #endregion
    }
}
