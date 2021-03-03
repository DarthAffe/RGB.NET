// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RGB.NET.Core
{
    /// <inheritdoc cref="AbstractBindable" />
    /// <inheritdoc cref="IRGBDevice{TDeviceInfo}" />
    /// <summary>
    /// Represents a generic RGB-device.
    /// </summary>
    public abstract class AbstractRGBDevice<TDeviceInfo> : Placeable, IRGBDevice<TDeviceInfo>
        where TDeviceInfo : class, IRGBDeviceInfo
    {
        private RGBSurface? _surface;

        #region Properties & Fields

        RGBSurface? IRGBDevice.Surface
        {
            get => _surface;
            set
            {
                if (SetProperty(ref _surface, value))
                {
                    if (value == null) OnDetached();
                    else OnAttached();
                }
            }
        }

        /// <inheritdoc />
        public abstract TDeviceInfo DeviceInfo { get; }

        /// <inheritdoc />
        IRGBDeviceInfo IRGBDevice.DeviceInfo => DeviceInfo;

        /// <inheritdoc />
        public IList<IColorCorrection> ColorCorrections { get; } = new List<IColorCorrection>();

        /// <summary>
        /// Gets or sets if the device needs to be flushed on every update.
        /// </summary>
        protected bool RequiresFlush { get; set; } = false;

        /// <summary>
        /// Gets a dictionary containing all <see cref="Led"/> of the <see cref="IRGBDevice"/>.
        /// </summary>
        protected Dictionary<LedId, Led> LedMapping { get; } = new();

        #region Indexer

        /// <inheritdoc />
        Led? IRGBDevice.this[LedId ledId] => LedMapping.TryGetValue(ledId, out Led? led) ? led : null;

        /// <inheritdoc />
        Led? IRGBDevice.this[Point location] => LedMapping.Values.FirstOrDefault(x => x.Boundary.Contains(location));

        /// <inheritdoc />
        IEnumerable<Led> IRGBDevice.this[Rectangle referenceRect, double minOverlayPercentage]
            => LedMapping.Values.Where(x => referenceRect.CalculateIntersectPercentage(x.Boundary) >= minOverlayPercentage);

        #endregion

        #endregion

        #region Methods

        /// <inheritdoc />
        public virtual void Update(bool flushLeds = false)
        {
            // Device-specific updates
            DeviceUpdate();

            // Send LEDs to SDK
            List<Led> ledsToUpdate = GetLedsToUpdate(flushLeds).ToList();
            foreach (Led ledToUpdate in ledsToUpdate)
                ledToUpdate.Update();

            UpdateLeds(ledsToUpdate);
        }

        protected virtual IEnumerable<Led> GetLedsToUpdate(bool flushLeds) => ((RequiresFlush || flushLeds) ? LedMapping.Values : LedMapping.Values.Where(x => x.IsDirty)).Where(led => led.Color.A > 0);
        protected virtual IEnumerable<(object key, Color color)> GetUpdateData(IEnumerable<Led> leds)
        {
            if (ColorCorrections.Count > 0)
            {
                foreach (Led led in leds)
                {
                    Color color = led.Color;
                    object key = led.CustomData ?? led.Id;

                    foreach (IColorCorrection colorCorrection in ColorCorrections)
                        colorCorrection.ApplyTo(ref color);

                    yield return (key, color);
                }
            }
            else
            {
                foreach (Led led in leds)
                {
                    Color color = led.Color;
                    object key = led.CustomData ?? led.Id;

                    yield return (key, color);
                }
            }
        }

        /// <inheritdoc />
        public virtual void Dispose()
        {
            try
            {
                LedMapping.Clear();
            }
            catch { /* this really shouldn't happen */ }
        }

        /// <summary>
        /// Performs device specific updates.
        /// </summary>
        protected virtual void DeviceUpdate()
        { }

        /// <summary>
        /// Sends all the updated <see cref="Led"/> to the device.
        /// </summary>
        protected abstract void UpdateLeds(IEnumerable<Led> ledsToUpdate);

        /// <summary>
        /// Initializes the <see cref="Led"/> with the specified id.
        /// </summary>
        /// <param name="ledId">The <see cref="LedId"/> to initialize.</param>
        /// <param name="location">The location of the <see cref="Led"/> to initialize.</param>
        /// <param name="size">The size of the <see cref="Led"/> to initialize.</param>
        /// <returns>The initialized led.</returns>
        public virtual Led? AddLed(LedId ledId, in Point location, in Size size, object? customData = null)
        {
            if ((ledId == LedId.Invalid) || LedMapping.ContainsKey(ledId)) return null;

            Led led = new(this, ledId, location, size, customData ?? GetLedCustomData(ledId));
            LedMapping.Add(ledId, led);
            return led;
        }

        public virtual Led? RemoveLed(LedId ledId)
        {
            if (ledId == LedId.Invalid) return null;
            if (!LedMapping.TryGetValue(ledId, out Led? led)) return null;

            LedMapping.Remove(ledId);
            return led;
        }

        protected virtual object? GetLedCustomData(LedId ledId) => null;

        protected virtual void OnAttached()
        {
            if (Location == Point.Invalid) Location = new Point(0, 0);
            if (Size == Size.Invalid)
            {
                Rectangle ledRectangle = new(this.Select(x => x.Boundary));
                Size = ledRectangle.Size + new Size(ledRectangle.Location.X, ledRectangle.Location.Y);
            }
        }

        protected virtual void OnDetached() { }

        #region Enumerator

        /// <inheritdoc />
        /// <summary>
        /// Returns an enumerator that iterates over all <see cref="T:RGB.NET.Core.Led" /> of the <see cref="T:RGB.NET.Core.IRGBDevice" />.
        /// </summary>
        /// <returns>An enumerator for all <see cref="T:RGB.NET.Core.Led" /> of the <see cref="T:RGB.NET.Core.IRGBDevice" />.</returns>
        public IEnumerator<Led> GetEnumerator() => LedMapping.Values.GetEnumerator();

        /// <inheritdoc />
        /// <summary>
        /// Returns an enumerator that iterates over all <see cref="T:RGB.NET.Core.Led" /> of the <see cref="T:RGB.NET.Core.IRGBDevice" />.
        /// </summary>
        /// <returns>An enumerator for all <see cref="T:RGB.NET.Core.Led" /> of the <see cref="T:RGB.NET.Core.IRGBDevice" />.</returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        #endregion
    }
}
