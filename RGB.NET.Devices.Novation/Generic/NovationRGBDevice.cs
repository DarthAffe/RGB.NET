using System;
using System.Collections.Generic;
using System.Linq;
using RGB.NET.Core;

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

        /// <inheritdoc />
        /// <summary>
        /// Gets information about the <see cref="T:RGB.NET.Devices.Novation.NovationRGBDevice" />.
        /// </summary>
        public override TDeviceInfo DeviceInfo { get; }

        /// <summary>
        /// The <see cref="MidiUpdateQueue"/> used to update this <see cref="NovationRGBDevice{TDeviceInfo}"/>.
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        protected MidiUpdateQueue? UpdateQueue { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NovationRGBDevice{TDeviceInfo}"/> class.
        /// </summary>
        /// <param name="info">The generic information provided by Novation for the device.</param>
        protected NovationRGBDevice(TDeviceInfo info)
        {
            this.DeviceInfo = info;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the device.
        /// </summary>
        public void Initialize(IDeviceUpdateTrigger updateTrigger)
        {
            InitializeLayout();
            
            UpdateQueue = DeviceInfo.ColorCapabilities switch
            {
                NovationColorCapabilities.LimitedRG => new LimitedColorUpdateQueue(updateTrigger, DeviceInfo.DeviceId),
                NovationColorCapabilities.RGB => new RGBColorUpdateQueue(updateTrigger, DeviceInfo.DeviceId),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        /// <summary>
        /// Initializes the <see cref="Led"/> and <see cref="Size"/> of the device.
        /// </summary>
        protected abstract void InitializeLayout();

        /// <inheritdoc />
        protected override void UpdateLeds(IEnumerable<Led> ledsToUpdate) => UpdateQueue?.SetData(ledsToUpdate.Where(x => x.Color.A > 0));

        /// <summary>
        /// Resets the <see cref="NovationRGBDevice{TDeviceInfo}"/> back to default.
        /// </summary>
        public virtual void Reset() => UpdateQueue?.Reset();

        /// <inheritdoc cref="IDisposable.Dispose" />
        /// <inheritdoc cref="AbstractRGBDevice{TDeviceInfo}.Dispose" />
        public override void Dispose()
        {
            Reset();

            try { UpdateQueue?.Dispose(); }
            catch { /* at least we tried */ }

            base.Dispose();
        }

        #endregion
    }
}
