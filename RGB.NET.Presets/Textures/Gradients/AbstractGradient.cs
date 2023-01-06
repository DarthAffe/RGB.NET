// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using RGB.NET.Core;
using RGB.NET.Presets.Decorators;

namespace RGB.NET.Presets.Textures.Gradients;

/// <inheritdoc cref="AbstractDecoratable{T}" />
/// <inheritdoc cref="IGradient" />
/// <summary>
/// Represents a basic gradient.
/// </summary>
public abstract class AbstractGradient : AbstractDecoratable<IGradientDecorator>, IGradient
{
    #region Properties & Fields

    /// <summary>
    /// Gets a list of the stops used by this <see cref="AbstractGradient"/>.
    /// </summary>
    public ObservableCollection<GradientStop> GradientStops { get; } = new();

    private bool _wrapGradient;
    /// <summary>
    /// Gets or sets if the Gradient wraps around if there isn't a second stop to take.
    /// Example: There is a stop at offset 0.0, 0.5 and 0.75. 
    /// Without wrapping offset 1.0 will be calculated the same as 0.75; with wrapping it would be the same as 0.0.
    /// </summary>
    public bool WrapGradient
    {
        get => _wrapGradient;
        set => SetProperty(ref _wrapGradient, value);
    }

    #endregion

    #region Events

    /// <inheritdoc />
    public event EventHandler? GradientChanged;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractGradient"/> class.
    /// </summary>
    protected AbstractGradient()
    {
        GradientStops.CollectionChanged += GradientCollectionChanged;
        PropertyChanged += (_, _) => OnGradientChanged();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractGradient"/> class.
    /// </summary>
    /// <param name="gradientStops">The stops with which the gradient should be initialized.</param>
    protected AbstractGradient(params GradientStop[] gradientStops)
    {
        GradientStops.CollectionChanged += GradientCollectionChanged;
        PropertyChanged += (_, _) => OnGradientChanged();

        foreach (GradientStop gradientStop in gradientStops)
            GradientStops.Add(gradientStop);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractGradient"/> class.
    /// </summary>
    /// <param name="wrapGradient">Specifies whether the gradient should wrapp or not (see <see cref="WrapGradient"/> for an example of what this means).</param>
    /// <param name="gradientStops">The stops with which the gradient should be initialized.</param>
    protected AbstractGradient(bool wrapGradient, params GradientStop[] gradientStops)
    {
        this.WrapGradient = wrapGradient;

        GradientStops.CollectionChanged += GradientCollectionChanged;
        PropertyChanged += (_, _) => OnGradientChanged();

        foreach (GradientStop gradientStop in gradientStops)
            GradientStops.Add(gradientStop);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Clips the offset and ensures, that it is inside the bounds of the stop list.
    /// </summary>
    /// <param name="offset">The offset to clip.</param>
    /// <returns>The clipped offset.</returns>
    protected float ClipOffset(float offset)
    {
        float max = GradientStops.Max(stop => stop.Offset);
        if (offset > max)
            return max;

        float min = GradientStops.Min(stop => stop.Offset);
        return offset < min ? min : offset;
    }

    /// <inheritdoc />
    public abstract Color GetColor(float offset);

    /// <inheritdoc />
    public virtual void Move(float offset)
    {
        offset /= 360.0f;

        foreach (GradientStop gradientStop in GradientStops)
            gradientStop.Offset += offset;

        while (GradientStops.All(stop => stop.Offset > 1))
            foreach (GradientStop gradientStop in GradientStops)
                gradientStop.Offset -= 1;

        while (GradientStops.All(stop => stop.Offset < 0))
            foreach (GradientStop gradientStop in GradientStops)
                gradientStop.Offset += 1;
    }

    /// <summary>
    /// Should be called to indicate that the gradient was changed.
    /// </summary>
    protected void OnGradientChanged() => GradientChanged?.Invoke(this, EventArgs.Empty);

    private void GradientCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems != null)
            foreach (GradientStop gradientStop in e.OldItems)
                gradientStop.PropertyChanged -= GradientStopChanged;

        if (e.NewItems != null)
            foreach (GradientStop gradientStop in e.NewItems)
                gradientStop.PropertyChanged += GradientStopChanged;

        OnGradientChanged();
    }

    private void GradientStopChanged(object? sender, PropertyChangedEventArgs propertyChangedEventArgs) => OnGradientChanged();

    #endregion
}