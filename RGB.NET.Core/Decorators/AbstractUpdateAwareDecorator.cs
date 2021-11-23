namespace RGB.NET.Core;

/// <inheritdoc />
/// <summary>
/// Represents a basic decorator which is aware of the <see cref="E:RGB.NET.Core.RGBSurface.Updating" /> event.
/// </summary>
public abstract class AbstractUpdateAwareDecorator : AbstractDecorator
{
    #region Properties & Fields

    /// <summary>
    /// Gets the surface this decorator is attached to.
    /// </summary>
    protected RGBSurface Surface { get; }

    /// <summary>
    /// Gets or sets if the <see cref="AbstractUpdateAwareDecorator"/> should call <see cref="Update"/> even if the Decorator is disabled.
    /// </summary>
    protected bool UpdateIfDisabled { get; set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractUpdateAwareDecorator"/> class.
    /// </summary>
    /// <param name="surface">The surface this decorator is attached to.</param>
    /// <param name="updateIfDisabled">Bool indicating if the <see cref="AbstractUpdateAwareDecorator"/> should call <see cref="Update"/> even if the Decorator is disabled.</param>
    protected AbstractUpdateAwareDecorator(RGBSurface surface, bool updateIfDisabled = false)
    {
        this.Surface = surface;
        this.UpdateIfDisabled = updateIfDisabled;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public override void OnAttached(IDecoratable decoratable)
    {
        if (DecoratedObjects.Count == 0)
            Surface.Updating += OnSurfaceUpdating;

        base.OnAttached(decoratable);
    }

    /// <inheritdoc />
    public override void OnDetached(IDecoratable decoratable)
    {
        base.OnDetached(decoratable);

        if (DecoratedObjects.Count == 0)
            Surface.Updating -= OnSurfaceUpdating;
    }

    private void OnSurfaceUpdating(UpdatingEventArgs args)
    {
        if (IsEnabled || UpdateIfDisabled)
            Update(args.DeltaTime);
    }

    /// <summary>
    /// Updates this <see cref="AbstractUpdateAwareDecorator"/>.
    /// </summary>
    /// <param name="deltaTime">The elapsed time (in seconds) since the last update.</param>
    protected abstract void Update(double deltaTime);

    #endregion
}