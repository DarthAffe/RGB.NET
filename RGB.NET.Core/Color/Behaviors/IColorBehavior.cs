namespace RGB.NET.Core
{
    public interface IColorBehavior
    {
        string ToString(in Color color);

        bool Equals(in Color color, object? obj);

        int GetHashCode(in Color color);

        Color Blend(in Color baseColor, in Color blendColor);
    }
}
