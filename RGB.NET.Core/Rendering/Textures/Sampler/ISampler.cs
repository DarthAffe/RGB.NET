using System;

namespace RGB.NET.Core
{
    public interface ISampler<T>
    {
        void SampleColor(in SamplerInfo<T> info, Span<T> pixelData);
    }
}
