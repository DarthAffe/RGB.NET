using System;

namespace RGB.NET.Core
{
    public class Placeable : AbstractBindable, IPlaceable
    {
        #region Properties & Fields

        protected IPlaceable? Parent { get; }

        private Point _location = Point.Invalid;
        /// <inheritdoc />
        public Point Location
        {
            get => _location;
            set
            {
                if (SetProperty(ref _location, value))
                    OnLocationChanged();
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
                    OnSizeChanged();
            }
        }

        private Scale _scale = new(1);
        /// <inheritdoc />
        public Scale Scale
        {
            get => _scale;
            set
            {
                if (SetProperty(ref _scale, value))
                    OnScaleChanged();
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
                    OnRotationChanged();
            }
        }

        private Point _actualLocation = Point.Invalid;
        /// <inheritdoc />
        public Point ActualLocation
        {
            get => _actualLocation;
            private set
            {
                if (SetProperty(ref _actualLocation, value))
                    OnActualLocationChanged();
            }
        }

        private Size _actualSize = Size.Invalid;
        /// <inheritdoc />
        public Size ActualSize
        {
            get => _actualSize;
            private set
            {
                if (SetProperty(ref _actualSize, value))
                    OnActualSizeChanged();
            }
        }

        private Rectangle _boundry = new(Point.Invalid, Point.Invalid);
        /// <inheritdoc />
        public Rectangle Boundry
        {
            get => _boundry;
            private set
            {
                if (SetProperty(ref _boundry, value))
                    OnBoundryChanged();
            }
        }

        #endregion

        #region Events

        public event EventHandler<EventArgs>? LocationChanged;
        public event EventHandler<EventArgs>? SizeChanged;
        public event EventHandler<EventArgs>? ScaleChanged;
        public event EventHandler<EventArgs>? RotationChanged;
        public event EventHandler<EventArgs>? ActualLocationChanged;
        public event EventHandler<EventArgs>? ActualSizeChanged;
        public event EventHandler<EventArgs>? BoundryChanged;

        #endregion

        #region Constructors

        public Placeable() { }

        public Placeable(IPlaceable parent)
        {
            this.Parent = parent;

            Parent.BoundryChanged += (_, _) => UpdateActualPlaceableData();
        }

        public Placeable(Point location, Size size)
        {
            this.Location = location;
            this.Size = size;
        }

        public Placeable(IPlaceable parent, Point location, Size size)
        {
            this.Parent = parent;
            this.Location = location;
            this.Size = size;

            Parent.BoundryChanged += (_, _) => UpdateActualPlaceableData();
        }

        #endregion

        #region Methods

        protected virtual void UpdateActualPlaceableData()
        {
            if (Parent != null)
            {
                Size actualSize = Size * Parent.Scale;
                Point actualLocation = (Location * Parent.Scale);
                Rectangle boundry = new(actualLocation, actualSize);

                if (Parent.Rotation.IsRotated)
                {
                    Point parentCenter = new Rectangle(Parent.ActualSize).Center;
                    Point actualParentCenter = new Rectangle(Parent.Boundry.Size).Center;
                    Point centerOffset = new(actualParentCenter.X - parentCenter.X, actualParentCenter.Y - parentCenter.Y);

                    actualLocation = actualLocation.Rotate(Parent.Rotation, new Rectangle(Parent.ActualSize).Center) + centerOffset;
                    boundry = new Rectangle(boundry.Rotate(Parent.Rotation, new Rectangle(Parent.ActualSize).Center)).Translate(centerOffset);
                }

                ActualLocation = actualLocation;
                ActualSize = actualSize;
                Boundry = boundry;
            }
            else
            {
                ActualLocation = Location;
                ActualSize = Size * Scale;
                Boundry = new Rectangle(Location, new Rectangle(new Rectangle(Location, ActualSize).Rotate(Rotation)).Size);
            }
        }

        protected virtual void OnLocationChanged()
        {
            LocationChanged?.Invoke(this, new EventArgs());
            UpdateActualPlaceableData();
        }

        protected virtual void OnSizeChanged()
        {
            SizeChanged?.Invoke(this, new EventArgs());
            UpdateActualPlaceableData();
        }

        protected virtual void OnScaleChanged()
        {
            ScaleChanged?.Invoke(this, new EventArgs());
            UpdateActualPlaceableData();
        }

        protected virtual void OnRotationChanged()
        {
            RotationChanged?.Invoke(this, new EventArgs());
            UpdateActualPlaceableData();
        }

        protected virtual void OnActualLocationChanged() => ActualLocationChanged?.Invoke(this, new EventArgs());
        protected virtual void OnActualSizeChanged() => ActualSizeChanged?.Invoke(this, new EventArgs());
        protected virtual void OnBoundryChanged() => BoundryChanged?.Invoke(this, new EventArgs());

        #endregion
    }
}
