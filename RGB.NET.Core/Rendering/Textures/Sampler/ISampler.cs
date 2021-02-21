namespace RGB.NET.Core
{
    public delegate Color GetColor(int x, int y);

    public interface ISampler
    {
        Color SampleColor(int x, int y, int width, int height, GetColor getColorFunc);
    }
}
