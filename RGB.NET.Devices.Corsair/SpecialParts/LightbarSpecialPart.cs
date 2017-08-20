// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RGB.NET.Core;

namespace RGB.NET.Devices.Corsair.SpecialParts
{
    /// <summary>
    /// Represents a lightbar attached to a <see cref="IRGBDevice"/>
    /// </summary>
    public class LightbarSpecialPart : IRGBDeviceSpecialPart
    {
        #region Properties & Fields

        private List<Led> _leds;
        /// <summary>
        /// Gets a readonly collection of all <see cref="Led"/> of this <see cref="LightbarSpecialPart"/>.
        /// </summary>
        public IEnumerable<Led> Leds => new ReadOnlyCollection<Led>(_leds);

        private List<Led> _left;
        /// <summary>
        /// Gets a readonly collection of all <see cref="Led"/> in the left half of this <see cref="LightbarSpecialPart"/>.
        /// </summary>
        public IEnumerable<Led> Left => new ReadOnlyCollection<Led>(_left);

        private List<Led> _right;
        /// <summary>
        /// Gets a readonly collection of all <see cref="Led"/> in the right half of this <see cref="LightbarSpecialPart"/>.
        /// </summary>
        public IEnumerable<Led> Right => new ReadOnlyCollection<Led>(_right);

        /// <summary>
        /// Gets the Center <see cref="Led"/> of this <see cref="LightbarSpecialPart"/>.
        /// </summary>
        public Led Center { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LightbarSpecialPart"/> class.
        /// </summary>
        /// <param name="device">The device associated with this <see cref="IRGBDeviceSpecialPart"/>.</param>
        public LightbarSpecialPart(IRGBDevice device)
        {
            _leds = device.Where(led => (((CorsairLedId)led.Id).LedId >= CorsairLedIds.Lightbar1) && (((CorsairLedId)led.Id).LedId <= CorsairLedIds.Lightbar19)).ToList();
            _left = _leds.Where(led => ((CorsairLedId)led.Id).LedId < CorsairLedIds.Lightbar10).ToList();
            _right = _leds.Where(led => ((CorsairLedId)led.Id).LedId > CorsairLedIds.Lightbar10).ToList();
            Center = _leds.FirstOrDefault(led => ((CorsairLedId)led.Id).LedId == CorsairLedIds.Lightbar10);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns an enumerator that iterates over all <see cref="Led"/> of the <see cref="IRGBDeviceSpecialPart"/>.
        /// </summary>
        /// <returns>An enumerator for all <see cref="Led"/> of the <see cref="IRGBDeviceSpecialPart"/>.</returns>
        public IEnumerator<Led> GetEnumerator() => _leds.GetEnumerator();

        /// <summary>
        /// Returns an enumerator that iterates over all <see cref="Led"/> of the <see cref="IRGBDeviceSpecialPart"/>.
        /// </summary>
        /// <returns>An enumerator for all <see cref="Led"/> of the <see cref="IRGBDeviceSpecialPart"/>.</returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }
}
