using System;

namespace RGB.NET.Core;

/// <summary>
/// Represents a placeable element.
/// </summary>
public class Placeable : AbstractBindable, IPlaceable
{
    #region Properties & Fields

    /// <summary>
    /// Gets the parent this placeable is placed in.
    /// </summary>
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

    private Rectangle _boundary = new(Point.Invalid, Point.Invalid);
    /// <inheritdoc />
    public Rectangle Boundary
    {
        get => _boundary;
        private set
        {
            if (SetProperty(ref _boundary, value))
                OnBoundaryChanged();
        }
    }

    #endregion

    #region Events

    /// <inheritdoc />
    public event EventHandler<EventArgs>? LocationChanged;

    /// <inheritdoc />
    public event EventHandler<EventArgs>? SizeChanged;

    /// <inheritdoc />
    public event EventHandler<EventArgs>? ScaleChanged;

    /// <inheritdoc />
    public event EventHandler<EventArgs>? RotationChanged;

    /// <inheritdoc />
    public event EventHandler<EventArgs>? ActualLocationChanged;

    /// <inheritdoc />
    public event EventHandler<EventArgs>? ActualSizeChanged;

    /// <inheritdoc />
    public event EventHandler<EventArgs>? BoundaryChanged;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Placeable" /> class.
    /// </summary>
    public Placeable() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Placeable" /> class.
    /// </summary>
    /// <param name="parent">The parent this placeable is placed in.</param>
    public Placeable(IPlaceable parent)
    {
        this.Parent = parent;

        Parent.BoundaryChanged += (_, _) => UpdateActualPlaceableData();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Placeable" /> class.
    /// </summary>
    /// <param name="location">The location of this placeable.</param>
    /// <param name="size">The size of this placeable.</param>
    public Placeable(Point location, Size size)
    {
        this.Location = location;
        this.Size = size;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Placeable" /> class.
    /// </summary>
    /// <param name="parent">The parent placeable this placeable is placed in.</param>
    /// <param name="location">The location of this placeable.</param>
    /// <param name="size">The size of this placeable.</param>
    public Placeable(IPlaceable parent, Point location, Size size)
    {
        this.Parent = parent;
        this.Location = location;
        this.Size = size;

        Parent.BoundaryChanged += (_, _) => UpdateActualPlaceableData();
    }

    #endregion

    #region Methods

    /// <summary>
    /// Updates the <see cref="ActualSize"/>, <see cref="ActualLocation"/> and <see cref="Boundary"/> based on the <see cref="Size"/>, <see cref="Scale"/> and <see cref="Rotation"/>.
    /// </summary>
    protected virtual void UpdateActualPlaceableData()
    {
        if (Parent != null)
        {
            Size actualSize = Size * Parent.Scale;
            Point actualLocation = (Location * Parent.Scale);
            Rectangle boundary = new(actualLocation, actualSize);

            if (Parent.Rotation.IsRotated)
            {
                Point parentCenter = new Rectangle(Parent.ActualSize).Center;
                Point actualParentCenter = new Rectangle(Parent.Boundary.Size).Center;
                Point centerOffset = new(actualParentCenter.X - parentCenter.X, actualParentCenter.Y - parentCenter.Y);

                actualLocation = actualLocation.Rotate(Parent.Rotation, new Rectangle(Parent.ActualSize).Center) + centerOffset;
                boundary = new Rectangle(boundary.Rotate(Parent.Rotation, new Rectangle(Parent.ActualSize).Center)).Translate(centerOffset);
            }

            ActualLocation = actualLocation;
            ActualSize = actualSize;
            Boundary = boundary;
        }
        else
        {
            ActualLocation = Location;
            ActualSize = Size * Scale;
            Boundary = new Rectangle(Location, new Rectangle(new Rectangle(Location, ActualSize).Rotate(Rotation)).Size);
        }
    }

    /// <summary>
    /// Called when the <see cref="Location"/> property was changed.
    /// </summary>
    protected virtual void OnLocationChanged()
    {
        LocationChanged?.Invoke(this, new EventArgs());
        UpdateActualPlaceableData();
    }

    /// <summary>
    /// Called when the <see cref="Size"/> property was changed.
    /// </summary>
    protected virtual void OnSizeChanged()
    {
        SizeChanged?.Invoke(this, new EventArgs());
        UpdateActualPlaceableData();
    }

    /// <summary>
    /// Called when the <see cref="Scale"/> property was changed.
    /// </summary>
    protected virtual void OnScaleChanged()
    {
        ScaleChanged?.Invoke(this, new EventArgs());
        UpdateActualPlaceableData();
    }

    /// <summary>
    /// Called when the <see cref="Rotation"/> property was changed.
    /// </summary>
    protected virtual void OnRotationChanged()
    {
        RotationChanged?.Invoke(this, new EventArgs());
        UpdateActualPlaceableData();
    }

    /// <summary>
    /// Called when the <see cref="ActualLocation"/> property was changed.
    /// </summary>
    protected virtual void OnActualLocationChanged() => ActualLocationChanged?.Invoke(this, new EventArgs());

    /// <summary>
    /// Called when the <see cref="ActualLocation"/> property was changed.
    /// </summary>
    protected virtual void OnActualSizeChanged() => ActualSizeChanged?.Invoke(this, new EventArgs());

    /// <summary>
    /// Called when the <see cref="Boundary"/> property was changed.
    /// </summary>
    protected virtual void OnBoundaryChanged() => BoundaryChanged?.Invoke(this, new EventArgs());

    #endregion
}