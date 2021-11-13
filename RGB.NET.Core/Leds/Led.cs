// ReSharper disable MemberCanBePrivate.Global

using System.Diagnostics;

namespace RGB.NET.Core;

/// <inheritdoc />
/// <summary>
/// Represents a single LED of a RGB-device.
/// </summary>
[DebuggerDisplay("{Id} {Color}")]
public class Led : Placeable
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

    private string? _shapeData;
    /// <summary>
    /// Gets or sets the data used for by the <see cref="Core.Shape.Custom"/>-<see cref="Core.Shape"/>.
    /// </summary>
    public string? ShapeData
    {
        get => _shapeData;
        set => SetProperty(ref _shapeData, value);
    }

    private Rectangle _absoluteBoundary;
    /// <summary>
    /// Gets a rectangle representing the logical location of the <see cref="Led"/> on the <see cref="RGBSurface"/>.
    /// </summary>
    public Rectangle AbsoluteBoundary
    {
        get => _absoluteBoundary;
        private set => SetProperty(ref _absoluteBoundary, value);
    }

    /// <summary>
    /// Indicates whether the <see cref="Led" /> is about to change it's color.
    /// </summary>
    public bool IsDirty => RequestedColor.HasValue && (RequestedColor != Color);

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
            if (RequestedColor.HasValue)
                RequestedColor = RequestedColor.Value + value;
            else
                RequestedColor = value;
        }
    }

    /// <summary>
    /// Gets the provider-specific data associated with this led.
    /// </summary>
    public object? CustomData { get; }

    /// <summary>
    /// Gets or sets some custom metadata of this led.
    /// </summary>
    public object? LayoutMetadata { get; set; }

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
    internal Led(IRGBDevice device, LedId id, Point location, Size size, object? customData = null)
        : base(device)
    {
        this.Device = device;
        this.Id = id;
        this.Location = location;
        this.Size = size;
        this.CustomData = customData;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override void UpdateActualPlaceableData()
    {
        base.UpdateActualPlaceableData();

        AbsoluteBoundary = Boundary.Translate(Device.Location);
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

    #endregion

    #region Operators

    /// <summary>
    /// Converts a <see cref="Led" /> to a <see cref="Core.Color" />.
    /// </summary>
    /// <param name="led">The <see cref="Led"/> to convert.</param>
    public static implicit operator Color(Led led) => led.Color;

    /// <summary>
    /// Converts a <see cref="Led" /> to a <see cref="Rectangle" />.
    /// </summary>
    /// <param name="led">The <see cref="Led"/> to convert.</param>
    public static implicit operator Rectangle(Led led) => led.Boundary;

    #endregion
}