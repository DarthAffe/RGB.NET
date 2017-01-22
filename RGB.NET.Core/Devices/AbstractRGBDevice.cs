using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RGB.NET.Core
{
    /// <summary>
    /// Represents a generic RGB-device
    /// </summary>
    public abstract class AbstractRGBDevice : IRGBDevice
    {
        #region Properties & Fields

        /// <summary>
        /// Gets generic information about the <see cref="IRGBDevice"/>.
        /// </summary>
        public IRGBDeviceInfo DeviceInfo { get; }

        /// <summary>
        /// Gets the <see cref="Rectangle"/> representing the whole <see cref="IRGBDevice"/>.
        /// </summary>
        public Rectangle DeviceRectangle { get; }

        /// <summary>
        /// Gets a dictionary containing all <see cref="Led"/> of the <see cref="IRGBDevice"/>.
        /// </summary>
        protected Dictionary<ILedId, Led> LedMapping { get; } = new Dictionary<ILedId, Led>();

        #region Indexer

        Led IRGBDevice.this[ILedId ledId]
        {
            get
            {
                Led led;
                return LedMapping.TryGetValue(ledId, out led) ? led : null;
            }
        }

        Led IRGBDevice.this[Point location] => LedMapping.Values.FirstOrDefault(x => x.LedRectangle.Contains(location));

        IEnumerable<Led> IRGBDevice.this[Rectangle referenceRect, float minOverlayPercentage]
            => LedMapping.Values.Where(x => referenceRect.CalculateIntersectPercentage(x.LedRectangle) >= minOverlayPercentage)
        ;

        #endregion

        #endregion

        #region Constructors

        #endregion

        #region Methods

        public void Initialize()
        {
            throw new System.NotImplementedException();
        }

        public void Update(bool flushLeds = false)
        {
            throw new System.NotImplementedException();
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
