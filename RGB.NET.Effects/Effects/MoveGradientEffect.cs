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

            // ReSharper disable once CanBeReplacedWithTryCastAndCheckForNull
            if (Brush.Gradient is LinearGradient)
            {
                LinearGradient linearGradient = (LinearGradient)Brush.Gradient;

                movement /= 360.0;

                foreach (GradientStop gradientStop in linearGradient.GradientStops)
                {
                    gradientStop.Offset = gradientStop.Offset + movement;

                    if (gradientStop.Offset > 1)
                        gradientStop.Offset -= 1;
                    else if (gradientStop.Offset < 0)
                        gradientStop.Offset += 1;
                }
            }
            else if (Brush.Gradient is RainbowGradient)
            {
                RainbowGradient rainbowGradient = (RainbowGradient)Brush.Gradient;

                // RainbowGradient is calculated inverse but the movement should be the same for all.
                movement *= -1;

                rainbowGradient.StartHue += movement;
                rainbowGradient.EndHue += movement;

                if ((rainbowGradient.StartHue > 360) && (rainbowGradient.EndHue > 360))
                {
                    rainbowGradient.StartHue -= 360;
                    rainbowGradient.EndHue -= 360;
                }
                else if ((rainbowGradient.StartHue < -360) && (rainbowGradient.EndHue < -360))
                {
                    rainbowGradient.StartHue += 360;
                    rainbowGradient.EndHue += 360;
                }
            }
        }

        #endregion
    }
}
