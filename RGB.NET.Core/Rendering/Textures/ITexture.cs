namespace RGB.NET.Core
{
    public interface ITexture
    {
        static ITexture Empty => new EmptyTexture();

        Size Size { get; }

        Color this[in Point point] { get; }
        Color this[in Rectangle rectangle] { get; }
    }
}
