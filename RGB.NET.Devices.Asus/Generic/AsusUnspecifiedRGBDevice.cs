using RGB.NET.Core;

namespace RGB.NET.Devices.Asus
{
    /// <inheritdoc cref="AsusRGBDevice{TDeviceInfo}" />
    /// <summary>
    /// Represents a Asus headset.
    /// </summary>
    public class AsusUnspecifiedRGBDevice : AsusRGBDevice<AsusRGBDeviceInfo>, IUnknownDevice
    {
        #region Properties & Fields

        private LedId _baseLedId;

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Asus.AsusHeadsetRGBDevice" /> class.
        /// </summary>
        /// <param name="info">The specific information provided by Asus for the headset.</param>
        internal AsusUnspecifiedRGBDevice(AsusRGBDeviceInfo info, LedId baseLedId)
            : base(info)
        {
            this._baseLedId = baseLedId;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void InitializeLayout()
        {
            int ledCount = DeviceInfo.Device.Lights.Count;
            for (int i = 0; i < ledCount; i++)
                InitializeLed(_baseLedId + i, new Rectangle(i * 10, 0, 10, 10));

            //TODO DarthAffe 19.05.2019: Add a way to define a layout for this kind of devies
        }

        /// <inheritdoc />
        protected override object CreateLedCustomData(LedId ledId) => (int)ledId - (int)_baseLedId;

        #endregion
    }
}
