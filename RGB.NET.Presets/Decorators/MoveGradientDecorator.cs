using RGB.NET.Core;
using RGB.NET.Presets.Textures.Gradients;

namespace RGB.NET.Presets.Decorators;

/// <inheritdoc cref="AbstractUpdateAwareDecorator" />
/// <inheritdoc cref="IGradientDecorator" />
/// <summary>
/// Represents a decorator which allows to move an <see cref="T:RGB.NET.Presets.Gradients.IGradient" /> by modifying his offset.
/// </summary>
public class MoveGradientDecorator : AbstractUpdateAwareDecorator, IGradientDecorator
{
    #region Properties & Fields
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
    // ReSharper disable MemberCanBePrivate.Global

    /// <summary>
    /// Gets or sets the direction the <see cref="IGradient"/> is moved.
    /// True leads to an offset-increment (normaly moving to the right), false to an offset-decrement (normaly moving to the left).
    /// </summary>
    public bool Direction { get; set; }

    /// <summary>
    /// Gets or sets the speed of the movement in units per second.
    /// The meaning of units differs for the different <see cref="IGradient"/>, but 360 units will always be one complete cycle:
    ///   <see cref="LinearGradient"/>: 360 unit = 1 offset.
    ///   <see cref="RainbowGradient"/>: 1 unit = 1 degree.
    /// </summary>
    public float Speed { get; set; }

    // ReSharper restore MemberCanBePrivate.Global
    // ReSharper restore AutoPropertyCanBeMadeGetOnly.Global
    #endregion

    #region Constructors

    /// <inheritdoc />
    /// <summary>
    /// Initializes a new instance of the <see cref="T:RGB.NET.Presets.Decorators.MoveGradientDecorator" /> class.
    /// </summary>
    /// <param name="surface">The surface this decorator belongs to.</param>
    /// <param name="speed">The speed of the movement in units per second.
    /// The meaning of units differs for the different <see cref="T:RGB.NET.Presets.Gradients.IGradient" />  but 360 units will always be one complete cycle:
    ///   <see cref="T:RGB.NET.Presets.Gradients.LinearGradient" />: 360 unit = 1 offset.
    ///   <see cref="T:RGB.NET.Presets.Gradients.RainbowGradient" />: 1 unit = 1 degree.</param>
    /// <param name="direction">The direction the <see cref="T:RGB.NET.Presets.Gradients.IGradient" /> is moved.
    /// True leads to an offset-increment (normaly moving to the right), false to an offset-decrement (normaly moving to the left).</param>
    public MoveGradientDecorator(RGBSurface surface, float speed = 180.0f, bool direction = true)
        : base(surface)
    {
        this.Speed = speed;
        this.Direction = direction;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override void Update(double deltaTime)
    {
        float movement = Speed * (float)deltaTime;

        if (!Direction)
            movement = -movement;

        foreach (IDecoratable decoratedObject in DecoratedObjects)
            if (decoratedObject is IGradient gradient)
                gradient.Move(movement);
    }

    #endregion
}