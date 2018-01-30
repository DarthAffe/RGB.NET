// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable ReturnTypeCanBeEnumerable.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedMember.Global

using System;
using RGB.NET.Brushes.Gradients;
using RGB.NET.Core;

namespace RGB.NET.Brushes
{
    /// <inheritdoc cref="AbstractBrush" />
    /// <inheritdoc cref="IGradientBrush" />
    /// <summary>
    /// Represents a brush drawing a conical gradient.
    /// </summary>
    public class ConicalGradientBrush : AbstractBrush, IGradientBrush
    {
        #region Properties & Fields

        private float _origin = (float)Math.Atan2(-1, 0);
        /// <summary>
        /// Gets or sets the origin (radian-angle) this <see cref="ConicalGradientBrush"/> is drawn to. (default: -π/2)
        /// </summary>
        public float Origin
        {
            get => _origin;
            set => SetProperty(ref _origin, value);
        }

        private Point _center = new Point(0.5, 0.5);
        /// <summary>
        /// Gets or sets the center <see cref="Point"/> (as percentage in the range [0..1]) of the <see cref="IGradient"/> drawn by this <see cref="ConicalGradientBrush"/>. (default: 0.5, 0.5)
        /// </summary>
        public Point Center
        {
            get => _center;
            set => SetProperty(ref _center, value);
        }

        private IGradient _gradient;
        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the gradient drawn by the brush. If null it will default to full transparent.
        /// </summary>
        public IGradient Gradient
        {
            get => _gradient;
            set => SetProperty(ref _gradient, value);
        }

        #endregion

        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Brushes.ConicalGradientBrush" /> class.
        /// </summary>
        public ConicalGradientBrush()
        { }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Brushes.ConicalGradientBrush" /> class.
        /// </summary>
        /// <param name="gradient">The <see cref="T:RGB.NET.Brushes.Gradients.IGradient" /> drawn by this <see cref="T:RGB.NET.Brushes.ConicalGradientBrush" />.</param>
        public ConicalGradientBrush(IGradient gradient)
        {
            this.Gradient = gradient;
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Brushes.ConicalGradientBrush" /> class.
        /// </summary>
        /// <param name="center">The center <see cref="T:RGB.NET.Core.Point" /> (as percentage in the range [0..1]).</param>
        /// <param name="gradient">The <see cref="T:RGB.NET.Brushes.Gradients.IGradient" /> drawn by this <see cref="T:RGB.NET.Brushes.ConicalGradientBrush" />.</param>
        public ConicalGradientBrush(Point center, IGradient gradient)
        {
            this.Center = center;
            this.Gradient = gradient;
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Brushes.ConicalGradientBrush" /> class.
        /// </summary>
        /// <param name="center">The center <see cref="T:RGB.NET.Core.Point" /> (as percentage in the range [0..1]).</param>
        /// <param name="origin">The origin (radian-angle) the <see cref="T:RGB.NET.Core.IBrush" /> is drawn to.</param>
        /// <param name="gradient">The <see cref="T:RGB.NET.Brushes.Gradients.IGradient" /> drawn by this <see cref="T:RGB.NET.Brushes.ConicalGradientBrush" />.</param>
        public ConicalGradientBrush(Point center, float origin, IGradient gradient)
        {
            this.Center = center;
            this.Origin = origin;
            this.Gradient = gradient;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override Color GetColorAtPoint(Rectangle rectangle, BrushRenderTarget renderTarget)
        {
            double centerX = rectangle.Size.Width * Center.X;
            double centerY = rectangle.Size.Height * Center.Y;

            double angle = Math.Atan2(renderTarget.Point.Y - centerY, renderTarget.Point.X - centerX) - Origin;
            if (angle < 0) angle += Math.PI * 2;
            double offset = angle / (Math.PI * 2);

            return Gradient.GetColor(offset);
        }

        #endregion
    }
}
