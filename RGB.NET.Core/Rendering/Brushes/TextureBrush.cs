namespace RGB.NET.Core
{
    public class TextureBrush : AbstractBrush
    {
        #region Properties & Fields

        private ITexture _texture = ITexture.Empty;
        public ITexture Texture
        {
            get => _texture;
            set => SetProperty(ref _texture, value);
        }

        #endregion

        #region Constructors

        public TextureBrush(ITexture texture)
        {
            this.Texture = texture;
        }

        #endregion

        #region Methods

        protected override Color GetColorAtPoint(in Rectangle rectangle, in RenderTarget renderTarget)
        {
            Rectangle normalizedRect = renderTarget.Rectangle / rectangle;
            return Texture[normalizedRect];
        }

        #endregion
    }
}
