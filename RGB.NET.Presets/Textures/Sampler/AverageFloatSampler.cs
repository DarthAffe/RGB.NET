using System;
using RGB.NET.Core;

namespace RGB.NET.Presets.Textures.Sampler;

/// <summary>
/// Represents a sampled that averages multiple float-data entries.
/// </summary>
public class AverageFloatSampler : ISampler<float>
{
    #region Methods

    /// <inheritdoc />
    public void Sample(in SamplerInfo<float> info, in Span<float> pixelData)
    {
        int count = info.Width * info.Height;
        if (count == 0) return;

        ReadOnlySpan<float> data = info.Data;

        int dataLength = pixelData.Length;
        Span<float> sums = stackalloc float[dataLength];
        for (int i = 0; i < data.Length; i += dataLength)
            for (int j = 0; j < sums.Length; j++)
                sums[j] += data[i + j];

        for (int i = 0; i < pixelData.Length; i++)
            pixelData[i] = sums[i] / count;
    }

    #endregion
}