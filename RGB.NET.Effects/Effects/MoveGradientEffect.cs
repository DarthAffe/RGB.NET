using RGB.NET.Brushes;
using RGB.NET.Brushes.Gradients;
using RGB.NET.Core;

namespace RGB.NET.Effects
{
    /// <summary>
    /// Represents an effect which allows to move an <see cref="IGradient"/> by modifying his offset.
    /// </summary>
    public class MoveGradientEffect : AbstractBrushEffect<IGradientBrush>
    {
        #region Properties & Fields
        // ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
        // ReSharper disable MemberCanBePrivate.Global

        /// <summary>
        /// Gets or sets the direction the <see cref="IGradient"/>  is moved.
        /// True leads to an offset-increment (normaly moving to the right), false to an offset-decrement (normaly moving to the left).
        /// </summary>
        public bool Direction { get; set; }

        /// <summary>
        /// Gets or sets the speed of the movement in units per second.
        /// The meaning of units differs for the different <see cref="IGradient"/> , but 360 units will always be one complete cycle:
        ///   <see cref="LinearGradient"/>: 360 unit = 1 offset.
        ///   <see cref="RainbowGradient"/>: 1 unit = 1 degree.
        /// </summary>
        public double Speed { get; set; }

        // ReSharper restore MemberCanBePrivate.Global
        // ReSharper restore AutoPropertyCanBeMadeGetOnly.Global
        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="direction"></param>
        public MoveGradientEffect(double speed = 180.0, bool direction = true)
        {
            this.Speed = speed;
            this.Direction = direction;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public override void Update(double deltaTime)
        {
            double movement = Speed * deltaTime;

            if (!Direction)
                movement = -movement;

            Brush?.Gradient?.Move(movement);
        }

        #endregion
    }
}
