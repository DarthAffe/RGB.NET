// ReSharper disable UnusedMember.Global

using System.Collections.Generic;
using System.Linq;
using RGB.NET.Core;

namespace RGB.NET.Brushes.Gradients
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a linear interpolated gradient with n stops.
    /// </summary>
    public class LinearGradient : AbstractGradient
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Brushes.Gradients.LinearGradient" /> class.
        /// </summary>
        public LinearGradient()
        { }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Brushes.Gradients.LinearGradient" /> class.
        /// </summary>
        /// <param name="gradientStops">The stops with which the gradient should be initialized.</param>
        public LinearGradient(params GradientStop[] gradientStops)
            : base(gradientStops)
        { }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Brushes.Gradients.AbstractGradient" /> class.
        /// </summary>
        /// <param name="wrapGradient">Specifies whether the gradient should wrapp or not (see <see cref="P:RGB.NET.Brushes.Gradients.AbstractGradient.WrapGradient" /> for an example of what this means).</param>
        /// <param name="gradientStops">The stops with which the gradient should be initialized.</param>
        public LinearGradient(bool wrapGradient, params GradientStop[] gradientStops)
            : base(wrapGradient, gradientStops)
        { }

        #endregion

        #region Methods

        /// <inheritdoc />
        /// <summary>
        /// Gets the linear interpolated <see cref="T:RGB.NET.Core.Color" /> at the given offset.
        /// </summary>
        /// <param name="offset">The percentage offset to take the color from.</param>
        /// <returns>The <see cref="T:RGB.NET.Core.Color" /> at the specific offset.</returns>
        public override Color GetColor(double offset)
        {
            if (GradientStops.Count == 0) return Color.Transparent;
            if (GradientStops.Count == 1) return new Color(GradientStops[0].Color);

            (GradientStop gsBefore, GradientStop gsAfter) = GetEnclosingGradientStops(offset, GradientStops, WrapGradient);

            double blendFactor = 0;
            if (!gsBefore.Offset.Equals(gsAfter.Offset))
                blendFactor = ((offset - gsBefore.Offset) / (gsAfter.Offset - gsBefore.Offset));

            byte colA = (byte)(((gsAfter.Color.A - gsBefore.Color.A) * blendFactor) + gsBefore.Color.A);
            byte colR = (byte)(((gsAfter.Color.R - gsBefore.Color.R) * blendFactor) + gsBefore.Color.R);
            byte colG = (byte)(((gsAfter.Color.G - gsBefore.Color.G) * blendFactor) + gsBefore.Color.G);
            byte colB = (byte)(((gsAfter.Color.B - gsBefore.Color.B) * blendFactor) + gsBefore.Color.B);

            return new Color(colA, colR, colG, colB);
        }

        /// <summary>
        /// Get the two <see cref="GradientStop"/>s encapsulating the given offset.
        /// </summary>
        /// <param name="offset">The reference offset.</param>
        /// <param name="stops">The <see cref="GradientStop"/> to choose from.</param>
        /// <param name="wrap">Bool indicating if the gradient should be wrapped or not.</param>
        /// <returns></returns>
        protected virtual (GradientStop gsBefore, GradientStop gsAfter) GetEnclosingGradientStops(double offset, IEnumerable<GradientStop> stops, bool wrap)
        {
            LinkedList<GradientStop> orderedStops = new LinkedList<GradientStop>(stops.OrderBy(x => x.Offset));

            if (wrap)
            {
                GradientStop gsBefore, gsAfter;

                do
                {
                    gsBefore = orderedStops.LastOrDefault(n => n.Offset <= offset);
                    if (gsBefore == null)
                    {
                        GradientStop lastStop = orderedStops.Last.Value;
                        orderedStops.AddFirst(new GradientStop(lastStop.Offset - 1, lastStop.Color));
                        orderedStops.RemoveLast();
                    }

                    gsAfter = orderedStops.FirstOrDefault(n => n.Offset >= offset);
                    if (gsAfter == null)
                    {
                        GradientStop firstStop = orderedStops.First.Value;
                        orderedStops.AddLast(new GradientStop(firstStop.Offset + 1, firstStop.Color));
                        orderedStops.RemoveFirst();
                    }

                } while ((gsBefore == null) || (gsAfter == null));

                return (gsBefore, gsAfter);
            }

            offset = ClipOffset(offset);
            return (orderedStops.Last(n => n.Offset <= offset), orderedStops.First(n => n.Offset >= offset));
        }

        #endregion
    }
}
