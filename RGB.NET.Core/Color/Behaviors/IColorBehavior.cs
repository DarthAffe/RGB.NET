namespace RGB.NET.Core
{
    public interface IColorBehavior
    {
        string ToString(Color color);

        bool Equals(Color color, object obj);

        int GetHashCode(Color color);

        Color Blend(Color baseColor, Color blendColor);
    }
}
