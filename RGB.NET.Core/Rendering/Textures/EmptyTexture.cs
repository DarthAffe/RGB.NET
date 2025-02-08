namespace RGB.NET.Core;

internal sealed class EmptyTexture : ITexture
{
    #region Properties & Fields

    public Size Size { get; } = new(0, 0);
    public Color this[Point point] => Color.Transparent;
    public Color this[Rectangle rectangle] => Color.Transparent;

    #endregion
}