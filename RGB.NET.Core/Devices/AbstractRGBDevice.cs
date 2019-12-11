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

        /// <inheritdoc />
        public abstract TDeviceInfo DeviceInfo { get; }

        /// <inheritdoc />
        IRGBDeviceInfo IRGBDevice.DeviceInfo => DeviceInfo;

        private Point _location = new Point(0, 0);
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
            protected set
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

        private Scale _scale = new Scale(1);
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

        private Rotation _rotation = new Rotation(0);
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

        /// <inheritdoc />
        public DeviceUpdateMode UpdateMode { get; set; } = DeviceUpdateMode.Sync;

        /// <summary>
        /// Gets a dictionary containing all <see cref="Led"/> of the <see cref="IRGBDevice"/>.
        /// </summary>
        protected Dictionary<LedId, Led> LedMapping { get; } = new Dictionary<LedId, Led>();

        /// <summary>
        /// Gets a dictionary containing all <see cref="IRGBDeviceSpecialPart"/> associated with this <see cref="IRGBDevice"/>.
        /// </summary>
        protected Dictionary<Type, IRGBDeviceSpecialPart> SpecialDeviceParts { get; } = new Dictionary<Type, IRGBDeviceSpecialPart>();

        #region Indexer

        /// <inheritdoc />
        Led IRGBDevice.this[LedId ledId] => LedMapping.TryGetValue(ledId, out Led led) ? led : null;

        /// <inheritdoc />
        Led IRGBDevice.this[Point location] => LedMapping.Values.FirstOrDefault(x => x.LedRectangle.Contains(location));

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

            if (UpdateMode.HasFlag(DeviceUpdateMode.Sync))
                UpdateLeds(ledsToUpdate);
        }

        protected virtual IEnumerable<Led> GetLedsToUpdate(bool flushLeds) => ((RequiresFlush || flushLeds) ? LedMapping.Values : LedMapping.Values.Where(x => x.IsDirty));

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
        /// <param name="ledId">The <see cref="LedId"/> to initialize.</param>
        /// <param name="ledRectangle">The <see cref="Rectangle"/> representing the position of the <see cref="Led"/> to initialize.</param>
        /// <returns></returns>
        [Obsolete("Use InitializeLed(LedId ledId, Point location, Size size) instead.")]
        protected virtual Led InitializeLed(LedId ledId, Rectangle rectangle) => InitializeLed(ledId, rectangle.Location, rectangle.Size);

        /// <summary>
        /// Initializes the <see cref="Led"/> with the specified id.
        /// </summary>
        /// <param name="ledId">The <see cref="LedId"/> to initialize.</param>
        /// <param name="location">The location of the <see cref="Led"/> to initialize.</param>
        /// <param name="size">The size of the <see cref="Led"/> to initialize.</param>
        /// <returns>The initialized led.</returns>
        protected virtual Led InitializeLed(LedId ledId, Point location, Size size)
        {
            if ((ledId == LedId.Invalid) || LedMapping.ContainsKey(ledId)) return null;

            Led led = new Led(this, ledId, location, size, CreateLedCustomData(ledId));
            LedMapping.Add(ledId, led);
            return led;
        }

        /// <summary>
        /// Applies the give <see cref="Color"/> to the <see cref="Led"/> ignoring internal workflows regarding locks and update-requests.
        /// This should be only used for syncbacks!
        /// </summary>
        /// <param name="led">The <see cref="Led"/> the <see cref="Color"/> should be aplied to.</param>
        /// <param name="color">The <see cref="Color"/> to apply.</param>
        protected virtual void SetLedColorWithoutRequest(Led led, Color color)
        {
            if (led == null) return;

            led.InternalColor = color;
        }

        /// <summary>
        /// Applies the given layout.
        /// </summary>
        /// <param name="layoutPath">The file containing the layout.</param>
        /// <param name="imageLayout">The name of the layout used to get the images of the leds.</param>
        /// <param name="createMissingLeds">If set to true a new led is initialized for every id in the layout if it doesn't already exist.</param>
        protected virtual DeviceLayout ApplyLayoutFromFile(string layoutPath, string imageLayout, bool createMissingLeds = false)
        {
            DeviceLayout layout = DeviceLayout.Load(layoutPath);
            if (layout != null)
            {
                string imageBasePath = string.IsNullOrWhiteSpace(layout.ImageBasePath) ? null : PathHelper.GetAbsolutePath(this, layout.ImageBasePath);
                if ((imageBasePath != null) && !string.IsNullOrWhiteSpace(layout.DeviceImage) && (DeviceInfo != null))
                    DeviceInfo.Image = new Uri(Path.Combine(imageBasePath, layout.DeviceImage), UriKind.Absolute);

                LedImageLayout ledImageLayout = layout.LedImageLayouts.FirstOrDefault(x => string.Equals(x.Layout, imageLayout, StringComparison.OrdinalIgnoreCase));

                Size = new Size(layout.Width, layout.Height);

                if (layout.Leds != null)
                    foreach (LedLayout layoutLed in layout.Leds)
                    {
                        if (Enum.TryParse(layoutLed.Id, true, out LedId ledId))
                        {
                            if (!LedMapping.TryGetValue(ledId, out Led led) && createMissingLeds)
                                led = InitializeLed(ledId, new Point(), new Size());

                            if (led != null)
                            {
                                led.Location = new Point(layoutLed.X, layoutLed.Y);
                                led.Size = new Size(layoutLed.Width, layoutLed.Height);
                                led.Shape = layoutLed.Shape;
                                led.ShapeData = layoutLed.ShapeData;

                                LedImage image = ledImageLayout?.LedImages.FirstOrDefault(x => x.Id == layoutLed.Id);
                                if ((imageBasePath != null) && !string.IsNullOrEmpty(image?.Image))
                                    led.Image = new Uri(Path.Combine(imageBasePath, image.Image), UriKind.Absolute);
                            }
                        }
                    }
            }

            return layout;
        }

        /// <summary>
        /// Creates provider-specific data associated with this <see cref="LedId"/>.
        /// </summary>
        /// <param name="ledId">The <see cref="LedId"/>.</param>
        protected virtual object CreateLedCustomData(LedId ledId) => null;

        /// <inheritdoc />
        public void AddSpecialDevicePart<T>(T specialDevicePart)
            where T : class, IRGBDeviceSpecialPart
            => SpecialDeviceParts[typeof(T)] = specialDevicePart;

        /// <inheritdoc />
        public T GetSpecialDevicePart<T>()
            where T : class, IRGBDeviceSpecialPart
            => SpecialDeviceParts.TryGetValue(typeof(T), out IRGBDeviceSpecialPart devicePart) ? (T)devicePart : default;

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
