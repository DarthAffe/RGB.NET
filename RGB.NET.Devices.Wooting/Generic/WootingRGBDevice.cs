using RGB.NET.Core;

namespace RGB.NET.Devices.Wooting.Generic
{
    /// <inheritdoc cref="AbstractRGBDevice{TDeviceInfo}" />
    /// <inheritdoc cref="IWootingRGBDevice" />
    /// <summary>
    /// Represents a Wooting-device
    /// </summary>
    public abstract class WootingRGBDevice<TDeviceInfo> : AbstractRGBDevice<TDeviceInfo>, IWootingRGBDevice
        where TDeviceInfo : WootingRGBDeviceInfo
    {
        #region Properties & Fields

        /// <inheritdoc />
        /// <summary>
        /// Gets information about the <see cref="T:RGB.NET.Devices.Wooting.WootingRGBDevice" />.
        /// </summary>
        public override TDeviceInfo DeviceInfo { get; }

        /// <summary>
        /// Gets or sets the update queue performing updates for this device.
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        protected UpdateQueue? UpdateQueue { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WootingRGBDevice{TDeviceInfo}"/> class.
        /// </summary>
        /// <param name="info">The generic information provided by Wooting for the device.</param>
        protected WootingRGBDevice(TDeviceInfo info)
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
            
            UpdateQueue = new WootingUpdateQueue(updateTrigger);
        }

        /// <summary>
        /// Initializes the <see cref="Led"/> and <see cref="Size"/> of the device.
        /// </summary>
        protected abstract void InitializeLayout();

        /// <inheritdoc />
        public override void Dispose()
        {
            try { UpdateQueue?.Dispose(); }
            catch { /* at least we tried */ }

            base.Dispose();
        }

        #endregion
    }
}
