using RGB.NET.Core;
using RGB.NET.Presets.Textures.Gradients;

namespace RGB.NET.Presets.Textures
{
    public abstract class AbstractGradientTexture : AbstractBindable, ITexture
    {
        #region Properties & Fields

        /// <summary>
        /// Gets the gradient drawn by the brush.
        /// </summary>
        public IGradient Gradient { get; }

        /// <inheritdoc />
        public Size Size { get; }

        /// <inheritdoc />
        public Color this[in Point point] => GetColor(point);

        /// <inheritdoc />
        public Color this[in Rectangle rectangle] => GetColor(rectangle.Center);

        #endregion

        #region Constructors

        protected AbstractGradientTexture(Size size, IGradient gradient)
        {
            this.Size = size;
            this.Gradient = gradient;
        }

        #endregion

        #region Methods

        protected abstract Color GetColor(in Point point);

        #endregion
    }
}
