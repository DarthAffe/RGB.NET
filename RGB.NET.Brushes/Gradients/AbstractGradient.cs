// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

using System.Collections.Generic;
using System.Linq;
using RGB.NET.Core;

namespace RGB.NET.Brushes.Gradients
{
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
        public IList<GradientStop> GradientStops { get; } = new List<GradientStop>();

        /// <summary>
        /// Gets or sets if the Gradient wraps around if there isn't a second stop to take.
        /// Example: There is a stop at offset 0.0, 0.5 and 0.75. 
        /// Without wrapping offset 1.0 will be calculated the same as 0.75; with wrapping it would be the same as 0.0.
        /// </summary>
        public bool WrapGradient { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractGradient"/> class.
        /// </summary>
        protected AbstractGradient()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractGradient"/> class.
        /// </summary>
        /// <param name="gradientStops">The stops with which the gradient should be initialized.</param>
        protected AbstractGradient(params GradientStop[] gradientStops)
        {
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

            foreach (GradientStop gradientStop in gradientStops)
                GradientStops.Add(gradientStop);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clips the offset and ensures, that it is inside the bounds of the stop list.
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        protected double ClipOffset(double offset)
        {
            double max = GradientStops.Max(n => n.Offset);
            if (offset > max)
                return max;

            double min = GradientStops.Min(n => n.Offset);
            return offset < min ? min : offset;
        }

        /// <inheritdoc />
        public abstract Color GetColor(double offset);

        /// <inheritdoc />
        public virtual void Move(double offset)
        {
            offset /= 360.0;

            foreach (GradientStop gradientStop in GradientStops)
                gradientStop.Offset += offset;

            while (GradientStops.All(x => x.Offset > 1))
                foreach (GradientStop gradientStop in GradientStops)
                    gradientStop.Offset -= 1;

            while (GradientStops.All(x => x.Offset < 0))
                foreach (GradientStop gradientStop in GradientStops)
                    gradientStop.Offset += 1;
        }

        #endregion
    }
}
