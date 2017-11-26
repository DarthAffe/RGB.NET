// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedMember.Global

using System;
using RGB.NET.Brushes.Gradients;
using RGB.NET.Brushes.Helper;
using RGB.NET.Core;

namespace RGB.NET.Brushes
{
    /// <inheritdoc cref="AbstractBrush" />
    /// <inheritdoc cref="IGradientBrush" />
    /// <summary>
    /// Represents a brush drawing a radial gradient around a center point.
    /// </summary>
    public class RadialGradientBrush : AbstractBrush, IGradientBrush
    {
        #region Properties & Fields

        /// <summary>
        /// Gets or sets the center <see cref="Point"/> (as percentage in the range [0..1]) around which the <see cref="RadialGradientBrush"/> should be drawn. (default: 0.5, 0.5)
        /// </summary>
        public Point Center { get; set; } = new Point(0.5, 0.5);

        /// <inheritdoc />
        public IGradient Gradient { get; set; }

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Brushes.RadialGradientBrush" /> class.
        /// </summary>
        public RadialGradientBrush()
        { }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Brushes.RadialGradientBrush" /> class.
        /// </summary>
        /// <param name="gradient">The gradient drawn by the brush.</param>
        public RadialGradientBrush(IGradient gradient)
        {
            this.Gradient = gradient;
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Brushes.RadialGradientBrush" /> class.
        /// </summary>
        /// <param name="center">The center point (as percentage in the range [0..1]).</param>
        /// <param name="gradient">The gradient drawn by the brush.</param>
        public RadialGradientBrush(Point center, IGradient gradient)
        {
            this.Center = center;
            this.Gradient = gradient;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override Color GetColorAtPoint(Rectangle rectangle, BrushRenderTarget renderTarget)
        {
            if (Gradient == null) return Color.Transparent;

            Point centerPoint = new Point(rectangle.Location.X + (rectangle.Size.Width * Center.X), rectangle.Location.Y + (rectangle.Size.Height * Center.Y));

            // Calculate the distance to the farthest point from the center as reference (this has to be a corner)
            // ReSharper disable once RedundantCast - never trust this ...
            double refDistance = Math.Max(Math.Max(Math.Max(GradientHelper.CalculateDistance(rectangle.Location, centerPoint),
                GradientHelper.CalculateDistance(new Point(rectangle.Location.X + rectangle.Size.Width, rectangle.Location.Y), centerPoint)),
                GradientHelper.CalculateDistance(new Point(rectangle.Location.X, rectangle.Location.Y + rectangle.Size.Height), centerPoint)),
                GradientHelper.CalculateDistance(new Point(rectangle.Location.X + rectangle.Size.Width, rectangle.Location.Y + rectangle.Size.Height), centerPoint));

            double distance = GradientHelper.CalculateDistance(renderTarget.Point, centerPoint);
            double offset = distance / refDistance;
            return Gradient.GetColor(offset);
        }

        #endregion
    }
}
