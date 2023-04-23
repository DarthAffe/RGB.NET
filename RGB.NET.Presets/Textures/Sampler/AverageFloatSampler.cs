using System;
using System.Numerics;
using System.Runtime.InteropServices;
using RGB.NET.Core;

namespace RGB.NET.Presets.Textures.Sampler;

/// <summary>
/// Represents a sampled that averages multiple float-data entries.
/// </summary>
public sealed class AverageFloatSampler : ISampler<float>
{
    #region Methods

    /// <inheritdoc />
    public unsafe void Sample(in SamplerInfo<float> info, in Span<float> pixelData)
    {
        int count = info.Width * info.Height;
        if (count == 0) return;

        int dataLength = pixelData.Length;
        Span<float> sums = stackalloc float[dataLength];

        int elementsPerVector = Vector<float>.Count / dataLength;
        int valuesPerVector = elementsPerVector * dataLength;
        if (Vector.IsHardwareAccelerated && (info.Height > 1) && (info.Width >= valuesPerVector) && (dataLength <= Vector<float>.Count))
        {
            int chunks = info.Width / elementsPerVector;

            Vector<float> sum = Vector<float>.Zero;

            for (int y = 0; y < info.Height; y++)
            {
                ReadOnlySpan<float> data = info[y];

                fixed (float* colorPtr = &MemoryMarshal.GetReference(data))
                {
                    float* current = colorPtr;
                    for (int i = 0; i < chunks; i++)
                    {
                        sum = Vector.Add(sum, *(Vector<float>*)current);
                        current += valuesPerVector;
                    }
                }

                int missingElements = data.Length - (chunks * valuesPerVector);
                int offset = chunks * valuesPerVector;
                for (int i = 0; i < missingElements; i += dataLength)
                    for (int j = 0; j < sums.Length; j++)
                        sums[j] += data[offset + i + j];
            }

            for (int i = 0; i < valuesPerVector; i += dataLength)
                for (int j = 0; j < sums.Length; j++)
                    sums[j] += sum[i + j];
        }
        else
        {
            for (int y = 0; y < info.Height; y++)
            {
                ReadOnlySpan<float> data = info[y];
                for (int i = 0; i < data.Length; i += dataLength)
                    for (int j = 0; j < sums.Length; j++)
                        sums[j] += data[i + j];
            }
        }

        for (int i = 0; i < pixelData.Length; i++)
            pixelData[i] = sums[i] / count;
    }

    #endregion
}