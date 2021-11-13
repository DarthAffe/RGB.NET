using System;
using RGB.NET.Core;

namespace RGB.NET.Presets.Textures.Sampler;

/// <summary>
/// Represents a sampled that averages multiple byte-data entries.
/// </summary>
public class AverageByteSampler : ISampler<byte>
{
    #region Methods

    /// <inheritdoc />
    public void Sample(in SamplerInfo<byte> info, in Span<byte> pixelData)
    {
        int count = info.Width * info.Height;
        if (count == 0) return;

        ReadOnlySpan<byte> data = info.Data;

        int dataLength = pixelData.Length;
        Span<uint> sums = stackalloc uint[dataLength];
        for (int i = 0; i < data.Length; i += dataLength)
            for (int j = 0; j < sums.Length; j++)
                sums[j] += data[i + j];

        float divisor = count * byte.MaxValue;
        for (int i = 0; i < pixelData.Length; i++)
            pixelData[i] = (sums[i] / divisor).GetByteValueFromPercentage();
    }

    #endregion
}