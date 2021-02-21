using System;

namespace RGB.NET.Core
{
    public interface ISampler
    {
        Color SampleColor(in ReadOnlyMemory<Color> data, int x, int y, int width, int height);
    }
}
