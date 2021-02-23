namespace RGB.NET.Core
{
    public interface ISampler<T>
    {
        Color SampleColor(SamplerInfo<T> info);
    }
}
