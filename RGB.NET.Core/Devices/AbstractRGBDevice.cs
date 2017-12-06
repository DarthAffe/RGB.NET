// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RGB.NET.Core
{
    /// <inheritdoc cref="AbstractBindable" />
    /// <inheritdoc cref="IRGBDevice{TDeviceInfo}" />
    /// <summary>
    /// Represents a generic RGB-device
    /// </summary>
    public abstract class AbstractRGBDevice<TDeviceInfo> : AbstractBindable, IRGBDevice<TDeviceInfo>
        where TDeviceInfo : IRGBDeviceInfo
    {
        #region Properties & Fields

        /// <inheritdoc />
        public abstract TDeviceInfo DeviceInfo { get; }

        /// <inheritdoc />
        IRGBDeviceInfo IRGBDevice.DeviceInfo => DeviceInfo;

        private Size _size = Size.Invalid;
        /// <inheritdoc />
        public Size Size
        {
            get => _size;
            set => SetProperty(ref _size, value);
        }

        private Point _location = new Point(0, 0);
        /// <inheritdoc />
        public Point Location
        {
            get => _location;
            set => SetProperty(ref _location, value);
        }

        /// <inheritdoc />
        public DeviceUpdateMode UpdateMode { get; set; } = DeviceUpdateMode.Sync;

        /// <summary>
        /// Gets a dictionary containing all <see cref="Led"/> of the <see cref="IRGBDevice"/>.
        /// </summary>
        protected Dictionary<ILedId, Led> LedMapping { get; } = new Dictionary<ILedId, Led>();

        /// <summary>
        /// Gets a dictionary containing all <see cref="IRGBDeviceSpecialPart"/> associated with this <see cref="IRGBDevice"/>.
        /// </summary>
        protected Dictionary<Type, IRGBDeviceSpecialPart> SpecialDeviceParts { get; } = new Dictionary<Type, IRGBDeviceSpecialPart>();

        #region Indexer

        /// <inheritdoc />
        Led IRGBDevice.this[ILedId ledId] => LedMapping.TryGetValue(ledId, out Led led) ? led : null;

        /// <inheritdoc />
        Led IRGBDevice.this[Point location] => LedMapping.Values.FirstOrDefault(x => x.LedRectangle.Contains(location));

        /// <inheritdoc />
        IEnumerable<Led> IRGBDevice.this[Rectangle referenceRect, double minOverlayPercentage]
            => LedMapping.Values.Where(x => referenceRect.CalculateIntersectPercentage(x.LedRectangle) >= minOverlayPercentage);

        #endregion

        #endregion

        #region Methods

        /// <inheritdoc />
        public virtual void Update(bool render = true, bool flushLeds = false)
        {
            // Device-specific updates
            DeviceUpdate();

            // Send LEDs to SDK
            IEnumerable<Led> ledsToUpdate = (flushLeds ? LedMapping.Values : LedMapping.Values.Where(x => x.IsDirty)).ToList();
            foreach (Led ledToUpdate in ledsToUpdate)
                ledToUpdate.Update();

            if (UpdateMode.HasFlag(DeviceUpdateMode.Sync))
                UpdateLeds(ledsToUpdate);
        }

        /// <inheritdoc />
        public virtual void SyncBack()
        { }

        /// <inheritdoc />
        public virtual void Dispose()
        {
            SpecialDeviceParts.Clear();
            LedMapping.Clear();
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

        /// <inheritdoc />
        public void AddSpecialDevicePart<T>(T specialDevicePart)
            where T : class, IRGBDeviceSpecialPart
            => SpecialDeviceParts[typeof(T)] = specialDevicePart;

        /// <inheritdoc />
        public T GetSpecialDevicePart<T>()
            where T : class, IRGBDeviceSpecialPart
            => SpecialDeviceParts.TryGetValue(typeof(T), out IRGBDeviceSpecialPart devicePart) ? (T)devicePart : default(T);

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
