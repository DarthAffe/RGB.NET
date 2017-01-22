using System.Linq;
using RGB.NET.Core;

namespace RGB.NET.Devices.Corsair
{
    /// <summary>
    /// Represents a generic CUE-device. (keyboard, mouse, headset, mousmat).
    /// </summary>
    public abstract class CorsairRGBDevice : AbstractRGBDevice
    {
        #region Properties & Fields

        /// <summary>
        /// Gets information about the <see cref="CorsairRGBDevice"/>.
        /// </summary>
        public override IRGBDeviceInfo DeviceInfo { get; }

        private Rectangle _deviceRectangle;
        /// <inheritdoc />
        public override Rectangle DeviceRectangle => _deviceRectangle;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CorsairRGBDevice"/> class.
        /// </summary>
        /// <param name="info">The generic information provided by CUE for the device.</param>
        protected CorsairRGBDevice(IRGBDeviceInfo info)
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
            InitializeLeds();

            _deviceRectangle = new Rectangle(this.Select(x => x.LedRectangle));
        }

        /// <summary>
        /// Initializes the <see cref="Led"/> of the device.
        /// </summary>
        protected abstract void InitializeLeds();

        #endregion
    }
}
