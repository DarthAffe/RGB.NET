// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RGB.NET.Core;

namespace RGB.NET.Devices.Corsair.SpecialParts
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a lightbar attached to a <see cref="T:RGB.NET.Core.IRGBDevice" />
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
            _leds = device.Where(led => ((CorsairLedId)led.CustomData >= CorsairLedId.Lightbar1) && ((CorsairLedId)led.CustomData <= CorsairLedId.Lightbar19)).ToList();
            _left = _leds.Where(led => (CorsairLedId)led.CustomData < CorsairLedId.Lightbar10).ToList();
            _right = _leds.Where(led => (CorsairLedId)led.CustomData > CorsairLedId.Lightbar10).ToList();
            Center = _leds.FirstOrDefault(led => (CorsairLedId)led.CustomData == CorsairLedId.Lightbar10);
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        /// <summary>
        /// Returns an enumerator that iterates over all <see cref="T:RGB.NET.Core.Led" /> of the <see cref="T:RGB.NET.Core.IRGBDeviceSpecialPart" />.
        /// </summary>
        /// <returns>An enumerator for all <see cref="T:RGB.NET.Core.Led" /> of the <see cref="T:RGB.NET.Core.IRGBDeviceSpecialPart" />.</returns>
        public IEnumerator<Led> GetEnumerator() => _leds.GetEnumerator();

        /// <inheritdoc />
        /// <summary>
        /// Returns an enumerator that iterates over all <see cref="T:RGB.NET.Core.Led" /> of the <see cref="T:RGB.NET.Core.IRGBDeviceSpecialPart" />.
        /// </summary>
        /// <returns>An enumerator for all <see cref="T:RGB.NET.Core.Led" /> of the <see cref="T:RGB.NET.Core.IRGBDeviceSpecialPart" />.</returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }
}
