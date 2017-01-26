// ReSharper disable CollectionNeverUpdated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable ReturnTypeCanBeEnumerable.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Brushes.Gradients;
using RGB.NET.Brushes.Helper;
using RGB.NET.Core;

namespace RGB.NET.Brushes
{
    /// <summary>
    /// Represents a brush drawing a linear gradient.
    /// </summary>
    public class LinearGradientBrush : AbstractBrush, IGradientBrush
    {
        #region Properties & Fields

        /// <summary>
        /// Gets or sets the start <see cref="Point"/> (as percentage in the range [0..1]) of the <see cref="IGradient"/> drawn by this <see cref="LinearGradientBrush"/>. (default: 0.0, 0.5)
        /// </summary>
        public Point StartPoint { get; set; } = new Point(0, 0.5);

        /// <summary>
        /// Gets or sets the end <see cref="Point"/>  (as percentage in the range [0..1]) of the <see cref="IGradient"/> drawn by this <see cref="LinearGradientBrush"/>. (default: 1.0, 0.5)
        /// </summary>
        public Point EndPoint { get; set; } = new Point(1, 0.5);

        /// <inheritdoc />
        public IGradient Gradient { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearGradientBrush"/> class.
        /// </summary>
        public LinearGradientBrush()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearGradientBrush"/> class.
        /// </summary>
        /// <param name="gradient">The <see cref="IGradient"/> drawn by this <see cref="LinearGradientBrush"/>.</param>
        public LinearGradientBrush(IGradient gradient)
        {
            this.Gradient = gradient;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="LinearGradientBrush"/> class.
        /// </summary>
        /// <param name="startPoint">The start <see cref="Point"/> (as percentage in the range [0..1]).</param>
        /// <param name="endPoint">The end <see cref="Point"/> (as percentage in the range [0..1]).</param>
        /// <param name="gradient">The <see cref="IGradient"/> drawn by this <see cref="LinearGradientBrush"/>.</param>
        public LinearGradientBrush(Point startPoint, Point endPoint, IGradient gradient)
        {
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;
            this.Gradient = gradient;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the color at an specific point assuming the brush is drawn into the given rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle in which the brush should be drawn.</param>
        /// <param name="renderTarget">The target (key/point) from which the color should be taken.</param>
        /// <returns>The color at the specified point.</returns>
        protected override Color GetColorAtPoint(Rectangle rectangle, BrushRenderTarget renderTarget)
        {
            if (Gradient == null) return Color.Transparent;

            Point startPoint = new Point(StartPoint.X * rectangle.Size.Width, StartPoint.Y * rectangle.Size.Height);
            Point endPoint = new Point(EndPoint.X * rectangle.Size.Width, EndPoint.Y * rectangle.Size.Height);

            double offset = GradientHelper.CalculateLinearGradientOffset(startPoint, endPoint, renderTarget.Point);
            return Gradient.GetColor(offset);
        }

        #endregion
    }
}
