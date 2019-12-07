﻿// ReSharper disable MemberCanBePrivate.Global

using System;
using System.ComponentModel;
using System.Diagnostics;

namespace RGB.NET.Core
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a single LED of a RGB-device.
    /// </summary>
    [DebuggerDisplay("{Id} {Color}")]
    public class Led : AbstractBindable
    {
        #region Properties & Fields

        /// <summary>
        /// Gets the <see cref="IRGBDevice"/> this <see cref="Led"/> is associated with.
        /// </summary>
        public IRGBDevice Device { get; }

        /// <summary>
        /// Gets the <see cref="LedId"/> of the <see cref="Led" />.
        /// </summary>
        public LedId Id { get; }

        private Shape _shape = Shape.Rectangle;
        /// <summary>
        /// Gets or sets the <see cref="Core.Shape"/> of the <see cref="Led"/>.
        /// </summary>
        public Shape Shape
        {
            get => _shape;
            set => SetProperty(ref _shape, value);
        }

        private string _shapeData;
        /// <summary>
        /// Gets or sets the data used for by the <see cref="Core.Shape.Custom"/>-<see cref="Core.Shape"/>.
        /// </summary>
        public string ShapeData
        {
            get => _shapeData;
            set => SetProperty(ref _shapeData, value);
        }

        private Point _location;
        /// <summary>
        /// Gets or sets the relative location of the <see cref="Led"/>.
        /// </summary>
        public Point Location
        {
            get => _location;
            set
            {
                if (SetProperty(ref _location, value))
                {
                    UpdateActualData();
                    UpdateAbsoluteData();
                }
            }
        }

        private Size _size;
        /// <summary>
        /// Gets or sets the size of the <see cref="Led"/>.
        /// </summary>
        public Size Size
        {
            get => _size;
            set
            {
                if (SetProperty(ref _size, value))
                {
                    UpdateActualData();
                    UpdateAbsoluteData();
                }
            }
        }

        private Point _actualLocation;
        /// <summary>
        /// Gets the actual location of the <see cref="Led"/>.
        /// This includes device-scaling and rotation.
        /// </summary>
        public Point ActualLocation
        {
            get => _actualLocation;
            private set => SetProperty(ref _actualLocation, value);
        }

        private Size _actualSize;
        /// <summary>
        /// Gets the actual size of the <see cref="Led"/>.
        /// This includes device-scaling.
        /// </summary>
        public Size ActualSize
        {
            get => _actualSize;
            private set => SetProperty(ref _actualSize, value);
        }

        private Rectangle _ledRectangle;
        /// <summary>
        /// Gets a rectangle representing the logical location of the <see cref="Led"/> relative to the <see cref="Device"/>.
        /// </summary>
        public Rectangle LedRectangle
        {
            get => _ledRectangle;
            private set => SetProperty(ref _ledRectangle, value);
        }

        private Rectangle _absoluteLedRectangle;
        /// <summary>
        /// Gets a rectangle representing the logical location of the <see cref="Led"/> on the <see cref="RGBSurface"/>.
        /// </summary>
        public Rectangle AbsoluteLedRectangle
        {
            get => _absoluteLedRectangle;
            private set => SetProperty(ref _absoluteLedRectangle, value);
        }

        /// <summary>
        /// Indicates whether the <see cref="Led" /> is about to change it's color.
        /// </summary>
        public bool IsDirty => RequestedColor.HasValue && (RequestedColor != InternalColor);

        private Color? _requestedColor;
        /// <summary>
        /// Gets a copy of the <see cref="Core.Color"/> the LED should be set to on the next update.
        /// Null if there is no update-request for the next update.
        /// </summary>
        public Color? RequestedColor
        {
            get => _requestedColor;
            private set
            {
                SetProperty(ref _requestedColor, value);

                // ReSharper disable once ExplicitCallerInfoArgument
                OnPropertyChanged(nameof(IsDirty));
            }
        }

        private Color _color = Color.Transparent;
        /// <summary>
        /// Gets the current <see cref="Core.Color"/> of the <see cref="Led"/>. Sets the <see cref="RequestedColor" /> for the next update.
        /// </summary>
        public Color Color
        {
            get => _color;
            set
            {
                if (!IsLocked)
                {
                    if (RequestedColor.HasValue)
                        RequestedColor += value;
                    else
                        RequestedColor = value;
                }

            }
        }

        /// <summary>
        /// Gets or set the <see cref="Color"/> ignoring all workflows regarding locks and update-requests. />
        /// </summary>
        internal Color InternalColor
        {
            get => _color;
            set => SetProperty(ref _color, value);
        }

        private bool _isLocked;
        /// <summary>
        /// Gets or sets if the color of this LED can be changed.
        /// </summary>
        public bool IsLocked
        {
            get => _isLocked;
            set => SetProperty(ref _isLocked, value);
        }

        /// <summary>
        /// Gets the URI of an image of the <see cref="Led"/> or null if there is no image.
        /// </summary>
        public Uri Image { get; set; }

        /// <summary>
        /// Gets the provider-specific data associated with this led.
        /// </summary>
        public object CustomData { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Led"/> class.
        /// </summary>
        /// <param name="device">The <see cref="IRGBDevice"/> the <see cref="Led"/> is associated with.</param>
        /// <param name="id">The <see cref="LedId"/> of the <see cref="Led"/>.</param>
        /// <param name="location">The physical location of the <see cref="Led"/> relative to the <see cref="Device"/>.</param>
        /// <param name="size">The size of the <see cref="Led"/>.</param>
        /// <param name="customData">The provider-specific data associated with this led.</param>
        internal Led(IRGBDevice device, LedId id, Point location, Size size, object customData = null)
        {
            this.Device = device;
            this.Id = id;
            this.Location = location;
            this.Size = size;
            this.CustomData = customData;

            device.PropertyChanged += DevicePropertyChanged;
        }

        #endregion

        #region Methods

        private void DevicePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ((e.PropertyName == nameof(IRGBDevice.Location)))
                UpdateAbsoluteData();
            else if (e.PropertyName == nameof(IRGBDevice.DeviceRectangle))
            {
                UpdateActualData();
                UpdateAbsoluteData();
            }
        }

        private void UpdateActualData()
        {
            ActualSize = Size * Device.Scale;

            Point actualLocation = (Location * Device.Scale);
            Rectangle ledRectangle = new Rectangle(Location * Device.Scale, Size * Device.Scale);

            if (Device.Rotation.IsRotated)
            {
                Point deviceCenter = new Rectangle(Device.ActualSize).Center;
                Point actualDeviceCenter = new Rectangle(Device.DeviceRectangle.Size).Center;
                Point centerOffset = new Point(actualDeviceCenter.X - deviceCenter.X, actualDeviceCenter.Y - deviceCenter.Y);

                actualLocation = actualLocation.Rotate(Device.Rotation, new Rectangle(Device.ActualSize).Center) + centerOffset;
                ledRectangle = new Rectangle(ledRectangle.Rotate(Device.Rotation, new Rectangle(Device.ActualSize).Center)).Translate(centerOffset);
            }

            ActualLocation = actualLocation;
            LedRectangle = ledRectangle;
        }

        private void UpdateAbsoluteData()
        {
            AbsoluteLedRectangle = LedRectangle.Translate(Device.Location);
        }

        /// <summary>
        /// Converts the <see cref="Id"/> and the <see cref="Color"/> of this <see cref="Led"/> to a human-readable string.
        /// </summary>
        /// <returns>A string that contains the <see cref="Id"/> and the <see cref="Color"/> of this <see cref="Led"/>. For example "Enter [A: 255, R: 255, G: 0, B: 0]".</returns>
        public override string ToString() => $"{Id} {Color}";

        /// <summary>
        /// Updates the <see cref="Led"/> to the requested <see cref="Core.Color"/>.
        /// </summary>
        internal void Update()
        {
            if (!RequestedColor.HasValue) return;

            _color = RequestedColor.Value;
            RequestedColor = null;

            // ReSharper disable once ExplicitCallerInfoArgument
            OnPropertyChanged(nameof(Color));
        }

        /// <summary>
        /// Resets the <see cref="Led"/> back to default.
        /// </summary>
        internal void Reset()
        {
            _color = Color.Transparent;
            RequestedColor = null;
            IsLocked = false;

            // ReSharper disable once ExplicitCallerInfoArgument
            OnPropertyChanged(nameof(Color));
        }

        #endregion

        #region Operators

        /// <summary>
        /// Converts a <see cref="Led" /> to a <see cref="Core.Color" />.
        /// </summary>
        /// <param name="led">The <see cref="Led"/> to convert.</param>
        public static implicit operator Color(Led led) => led?.Color ?? Color.Transparent;

        /// <summary>
        /// Converts a <see cref="Led" /> to a <see cref="Rectangle" />.
        /// </summary>
        /// <param name="led">The <see cref="Led"/> to convert.</param>
        public static implicit operator Rectangle(Led led) => led?.LedRectangle ?? new Rectangle();

        #endregion
    }
}
