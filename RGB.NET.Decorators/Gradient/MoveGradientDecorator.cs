using RGB.NET.Brushes;
using RGB.NET.Brushes.Gradients;
using RGB.NET.Core;

namespace RGB.NET.Decorators.Gradient
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a decorator which allows to move an <see cref="T:RGB.NET.Brushes.Gradients.IGradient" /> by modifying his offset.
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
        public double Speed { get; set; }

        // ReSharper restore MemberCanBePrivate.Global
        // ReSharper restore AutoPropertyCanBeMadeGetOnly.Global
        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Decorators.Gradient.MoveGradientDecorator" /> class.
        /// </summary>
        /// <param name="speed">The speed of the movement in units per second.
        /// The meaning of units differs for the different <see cref="T:RGB.NET.Brushes.Gradients.IGradient" />  but 360 units will always be one complete cycle:
        ///   <see cref="T:RGB.NET.Brushes.Gradients.LinearGradient" />: 360 unit = 1 offset.
        ///   <see cref="T:RGB.NET.Brushes.Gradients.RainbowGradient" />: 1 unit = 1 degree.</param>
        /// <param name="direction">The direction the <see cref="T:RGB.NET.Brushes.Gradients.IGradient" /> is moved.
        /// True leads to an offset-increment (normaly moving to the right), false to an offset-decrement (normaly moving to the left).</param>
        public MoveGradientDecorator(double speed = 180.0, bool direction = true)
        {
            this.Speed = speed;
            this.Direction = direction;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void Update(double deltaTime)
        {
            double movement = Speed * deltaTime;

            if (!Direction)
                movement = -movement;

            foreach (IDecoratable decoratedObject in DecoratedObjects)
                if (decoratedObject is IGradient gradient)
                    gradient.Move(movement);
        }

        #endregion
    }
}
