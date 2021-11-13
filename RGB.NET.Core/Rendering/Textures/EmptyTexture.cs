namespace RGB.NET.Core;

internal class EmptyTexture : ITexture
{
    #region Properties & Fields

    public Size Size { get; } = new(0, 0);
    public Color this[in Point point] => Color.Transparent;
    public Color this[in Rectangle rectangle] => Color.Transparent;

    #endregion
}