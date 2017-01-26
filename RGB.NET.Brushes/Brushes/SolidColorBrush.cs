// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

using RGB.NET.Core;

namespace RGB.NET.Brushes
{
    /// <summary>
    /// Represents a brush drawing only a single color.
    /// </summary>
    public class SolidColorBrush : AbstractBrush
    {
        #region Properties & Fields

        /// <summary>
        /// Gets or sets the <see cref="Color"/> drawn by this <see cref="SolidColorBrush"/>.
        /// </summary>
        public Color Color { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SolidColorBrush"/> class.
        /// </summary>
        /// <param name="color">The <see cref="Color"/> drawn by this <see cref="SolidColorBrush"/>.</param>
        public SolidColorBrush(Color color)
        {
            this.Color = color;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override Color GetColorAtPoint(Rectangle rectangle, BrushRenderTarget renderTarget)
        {
            return Color;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Converts a <see cref="Color" /> to a <see cref="SolidColorBrush" />.
        /// </summary>
        /// <param name="color">The <see cref="Color"/> to convert.</param>
        public static explicit operator SolidColorBrush(Color color)
        {
            return new SolidColorBrush(color);
        }

        /// <summary>
        /// Converts a <see cref="SolidColorBrush" /> to a <see cref="Color" />.
        /// </summary>
        /// <param name="brush">The <see cref="Color"/> to convert.</param>
        public static implicit operator Color(SolidColorBrush brush)
        {
            return brush.Color;
        }

        #endregion
    }
}