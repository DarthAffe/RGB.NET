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

        /// <inheritdoc />
        protected override Size InternalSize { get; set; }

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

            Rectangle ledRectangle = new Rectangle(this.Select(x => x.LedRectangle));
            InternalSize = ledRectangle.Size + new Size(ledRectangle.Location.X, ledRectangle.Location.Y);
        }

        /// <summary>
        /// Initializes the <see cref="Led"/> of the device.
        /// </summary>
        protected abstract void InitializeLeds();

        #endregion
    }
}
