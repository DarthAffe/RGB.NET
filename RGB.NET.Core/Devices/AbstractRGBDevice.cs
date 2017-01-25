// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RGB.NET.Core
{
    /// <summary>
    /// Represents a generic RGB-device
    /// </summary>
    public abstract class AbstractRGBDevice : AbstractBindable, IRGBDevice
    {
        #region Properties & Fields

        /// <inheritdoc />
        public abstract IRGBDeviceInfo DeviceInfo { get; }

        /// <inheritdoc />
        public Size Size => new Size(InternalSize?.Width ?? 0, InternalSize?.Height ?? 0);

        /// <summary>
        /// Gets the <see cref="Size"/> of the whole <see cref="IRGBDevice"/>.
        /// </summary>
        protected abstract Size InternalSize { get; set; }

        private Point _location = new Point();
        /// <inheritdoc />
        public Point Location
        {
            get { return _location; }
            set { SetProperty(ref _location, value ?? new Point()); }
        }

        /// <summary>
        /// Gets a dictionary containing all <see cref="Led"/> of the <see cref="IRGBDevice"/>.
        /// </summary>
        protected Dictionary<ILedId, Led> LedMapping { get; } = new Dictionary<ILedId, Led>();

        #region Indexer

        /// <inheritdoc />
        Led IRGBDevice.this[ILedId ledId]
        {
            get
            {
                Led led;
                return LedMapping.TryGetValue(ledId, out led) ? led : null;
            }
        }

        /// <inheritdoc />
        Led IRGBDevice.this[Point location] => LedMapping.Values.FirstOrDefault(x => x.LedRectangle.Contains(location));

        /// <inheritdoc />
        IEnumerable<Led> IRGBDevice.this[Rectangle referenceRect, double minOverlayPercentage]
            => LedMapping.Values.Where(x => referenceRect.CalculateIntersectPercentage(x.LedRectangle) >= minOverlayPercentage);

        #endregion

        #endregion

        #region Methods

        /// <inheritdoc />
        public virtual void Update(bool flushLeds = false)
        {
            // Device-specific updates
            DeviceUpdate();

            // Send LEDs to SDK
            IEnumerable<Led> ledsToUpdate = flushLeds ? LedMapping.Values : LedMapping.Values.Where(x => x.IsDirty);
            foreach (Led ledToUpdate in ledsToUpdate)
                ledToUpdate.Update();
        }

        /// <summary>
        /// Performs device specific updates.
        /// </summary>
        protected virtual void DeviceUpdate()
        { }

        /// <summary>
        /// Initializes the <see cref="Led"/> with the specified id.
        /// </summary>
        /// <param name="ledId">The <see cref="ILedId"/> to initialize.</param>
        /// <param name="ledRectangle">The <see cref="Rectangle"/> representing the position of the <see cref="Led"/> to initialize.</param>
        /// <returns></returns>
        protected virtual Led InitializeLed(ILedId ledId, Rectangle ledRectangle)
        {
            if (LedMapping.ContainsKey(ledId)) return null;

            Led led = new Led(this, ledId, ledRectangle);
            LedMapping.Add(ledId, led);
            return led;
        }

        #region Enumerator

        /// <summary>
        /// Returns an enumerator that iterates over all <see cref="Led"/> of the <see cref="IRGBDevice"/>.
        /// </summary>
        /// <returns>An enumerator for all <see cref="Led"/> of the <see cref="IRGBDevice"/>.</returns>
        public IEnumerator<Led> GetEnumerator()
        {
            return LedMapping.Values.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates over all <see cref="Led"/> of the <see cref="IRGBDevice"/>.
        /// </summary>
        /// <returns>An enumerator for all <see cref="Led"/> of the <see cref="IRGBDevice"/>.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #endregion
    }
}
