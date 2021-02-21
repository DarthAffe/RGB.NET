// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable ReturnTypeCanBeEnumerable.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedMember.Global

using System;
using RGB.NET.Core;
using RGB.NET.Presets.Textures.Gradients;

namespace RGB.NET.Presets.Textures
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a brush drawing a conical gradient.
    /// </summary>
    public sealed class ConicalGradientTexture : AbstractGradientTexture
    {
        #region Constants

        private const double PI2 = Math.PI * 2;

        #endregion

        #region Properties & Fields

        private double _origin = Math.Atan2(-1, 0);
        /// <summary>
        /// Gets or sets the origin (radian-angle) this <see cref="ConicalGradientTexture"/> is drawn to. (default: -π/2)
        /// </summary>
        public double Origin
        {
            get => _origin;
            set => SetProperty(ref _origin, value);
        }

        private Point _center = new(0.5, 0.5);
        /// <summary>
        /// Gets or sets the center <see cref="Point"/> (as percentage in the range [0..1]) of the <see cref="IGradient"/> drawn by this <see cref="ConicalGradientTexture"/>. (default: 0.5, 0.5)
        /// </summary>
        public Point Center
        {
            get => _center;
            set => SetProperty(ref _center, value);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Presets.Textures.ConicalGradientTexture" /> class.
        /// </summary>
        /// <param name="gradient">The <see cref="T:RGB.NET.Presets.Gradients.IGradient" /> drawn by this <see cref="T:RGB.NET.Presets.Textures.ConicalGradientTexture" />.</param>
        public ConicalGradientTexture(Size size, IGradient gradient)
            : base(size, gradient)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Presets.Textures.ConicalGradientTexture" /> class.
        /// </summary>
        /// <param name="center">The center <see cref="T:RGB.NET.Core.Point" /> (as percentage in the range [0..1]).</param>
        /// <param name="gradient">The <see cref="T:RGB.NET.Presets.Gradients.IGradient" /> drawn by this <see cref="T:RGB.NET.Presets.Textures.ConicalGradientTexture" />.</param>
        public ConicalGradientTexture(Size size, IGradient gradient, Point center)
            : base(size, gradient)
        {
            this.Center = center;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Presets.Textures.ConicalGradientTexture" /> class.
        /// </summary>
        /// <param name="center">The center <see cref="T:RGB.NET.Core.Point" /> (as percentage in the range [0..1]).</param>
        /// <param name="origin">The origin (radian-angle) the <see cref="T:RGB.NET.Core.IBrush" /> is drawn to.</param>
        /// <param name="gradient">The <see cref="T:RGB.NET.Presets.Gradients.IGradient" /> drawn by this <see cref="T:RGB.NET.Presets.Textures.ConicalGradientTexture" />.</param>
        public ConicalGradientTexture(Size size, IGradient gradient, Point center, float origin)
            : base(size, gradient)
        {
            this.Center = center;
            this.Origin = origin;
        }

        #endregion

        #region Methods

        protected override Color GetColor(in Point point)
        {
            double angle = Math.Atan2(point.Y - Center.Y, point.X - Center.X) - Origin;
            if (angle < 0) angle += PI2;
            double offset = angle / PI2;

            return Gradient.GetColor(offset);
        }

        #endregion
    }
}
