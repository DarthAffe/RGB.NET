// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RGB.NET.Core.Layout;

namespace RGB.NET.Core
{
    /// <inheritdoc cref="AbstractBindable" />
    /// <inheritdoc cref="IRGBDevice{TDeviceInfo}" />
    /// <summary>
    /// Represents a generic RGB-device.
    /// </summary>
    public abstract class AbstractRGBDevice<TDeviceInfo> : AbstractBindable, IRGBDevice<TDeviceInfo>
        where TDeviceInfo : class, IRGBDeviceInfo
    {
        #region Properties & Fields

        RGBSurface? IRGBDevice.Surface { get; set; }

        /// <inheritdoc />
        public abstract TDeviceInfo DeviceInfo { get; }

        /// <inheritdoc />
        IRGBDeviceInfo IRGBDevice.DeviceInfo => DeviceInfo;

        private Point _location = new(0, 0);
        /// <inheritdoc />
        public Point Location
        {
            get => _location;
            set
            {
                if (SetProperty(ref _location, value))
                    UpdateActualData();
            }
        }

        private Size _size = Size.Invalid;
        /// <inheritdoc />
        public Size Size
        {
            get => _size;
            set
            {
                if (SetProperty(ref _size, value))
                    UpdateActualData();
            }
        }

        private Size _actualSize;
        /// <inheritdoc />
        public Size ActualSize
        {
            get => _actualSize;
            private set => SetProperty(ref _actualSize, value);
        }

        private Rectangle _deviceRectangle;
        /// <inheritdoc />
        public Rectangle DeviceRectangle
        {
            get => _deviceRectangle;
            private set => SetProperty(ref _deviceRectangle, value);
        }

        private Scale _scale = new(1);
        /// <inheritdoc />
        public Scale Scale
        {
            get => _scale;
            set
            {
                if (SetProperty(ref _scale, value))
                    UpdateActualData();
            }
        }

        private Rotation _rotation = new(0);
        /// <inheritdoc />
        public Rotation Rotation
        {
            get => _rotation;
            set
            {
                if (SetProperty(ref _rotation, value))
                    UpdateActualData();
            }
        }

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
        Led? IRGBDevice.this[Point location] => LedMapping.Values.FirstOrDefault(x => x.LedRectangle.Contains(location));

        /// <inheritdoc />
        IEnumerable<Led> IRGBDevice.this[Rectangle referenceRect, double minOverlayPercentage]
            => LedMapping.Values.Where(x => referenceRect.CalculateIntersectPercentage(x.LedRectangle) >= minOverlayPercentage);

        #endregion

        #endregion

        #region Methods

        private void UpdateActualData()
        {
            ActualSize = Size * Scale;
            DeviceRectangle = new Rectangle(Location, new Rectangle(new Rectangle(Location, ActualSize).Rotate(Rotation)).Size);
        }

        /// <inheritdoc />
        public virtual void Update(bool flushLeds = false)
        {
            // Device-specific updates
            DeviceUpdate();

            // Send LEDs to SDK
            List<Led> ledsToUpdate = GetLedsToUpdate(flushLeds)?.ToList() ?? new List<Led>();
            foreach (Led ledToUpdate in ledsToUpdate)
                ledToUpdate.Update();

            UpdateLeds(ledsToUpdate);
        }

        protected virtual IEnumerable<Led> GetLedsToUpdate(bool flushLeds) => ((RequiresFlush || flushLeds) ? LedMapping.Values : LedMapping.Values.Where(x => x.IsDirty));

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
        public virtual Led? AddLed(LedId ledId, Point location, Size size, object? customData = null)
        {
            if ((ledId == LedId.Invalid) || LedMapping.ContainsKey(ledId)) return null;

            Led led = new(this, ledId, location, size, customData);
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
