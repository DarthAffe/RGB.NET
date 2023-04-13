using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using RGB.NET.Core;

namespace RGB.NET.Presets.Textures.Sampler;

/// <summary>
/// Represents a sampled that averages multiple byte-data entries.
/// </summary>
public class AverageByteSampler : ISampler<byte>
{
    #region Constants

    private static readonly int INT_VECTOR_LENGTH = Vector<uint>.Count;

    #endregion

    #region Methods

    /// <inheritdoc />
    [SkipLocalsInit]
    public unsafe void Sample(in SamplerInfo<byte> info, in Span<byte> pixelData)
    {
        int count = info.Width * info.Height;
        if (count == 0) return;

        ReadOnlySpan<byte> data = info.Data;

        int dataLength = pixelData.Length;
        Span<uint> sums = stackalloc uint[dataLength];

        if (Vector.IsHardwareAccelerated && (data.Length >= Vector<byte>.Count) && (dataLength <= Vector<byte>.Count))
        {
            int elementsPerVector = Vector<byte>.Count / dataLength;
            int valuesPerVector = elementsPerVector * dataLength;

            int chunks = data.Length / valuesPerVector;
            int missingElements = data.Length - (chunks * valuesPerVector);

            Vector<uint> sum1 = Vector<uint>.Zero;
            Vector<uint> sum2 = Vector<uint>.Zero;
            Vector<uint> sum3 = Vector<uint>.Zero;
            Vector<uint> sum4 = Vector<uint>.Zero;

            fixed (byte* colorPtr = &MemoryMarshal.GetReference(data))
            {
                byte* current = colorPtr;
                for (int i = 0; i < chunks; i++)
                {
                    Vector<byte> bytes = *(Vector<byte>*)current;
                    Vector.Widen(bytes, out Vector<ushort> short1, out Vector<ushort> short2);
                    Vector.Widen(short1, out Vector<uint> int1, out Vector<uint> int2);
                    Vector.Widen(short2, out Vector<uint> int3, out Vector<uint> int4);

                    sum1 = Vector.Add(sum1, int1);
                    sum2 = Vector.Add(sum2, int2);
                    sum3 = Vector.Add(sum3, int3);
                    sum4 = Vector.Add(sum4, int4);

                    current += valuesPerVector;
                }
            }

            int value = 0;
            int sumIndex = 0;
            for (int j = 0; (j < INT_VECTOR_LENGTH) && (value < valuesPerVector); j++)
            {
                sums[sumIndex] += sum1[j];
                ++sumIndex;
                ++value;

                if (sumIndex >= dataLength)
                    sumIndex = 0;
            }

            for (int j = 0; (j < INT_VECTOR_LENGTH) && (value < valuesPerVector); j++)
            {
                sums[sumIndex] += sum2[j];
                ++sumIndex;
                ++value;

                if (sumIndex >= dataLength)
                    sumIndex = 0;
            }

            for (int j = 0; (j < INT_VECTOR_LENGTH) && (value < valuesPerVector); j++)
            {
                sums[sumIndex] += sum3[j];
                ++sumIndex;
                ++value;

                if (sumIndex >= dataLength)
                    sumIndex = 0;
            }

            for (int j = 0; (j < INT_VECTOR_LENGTH) && (value < valuesPerVector); j++)
            {
                sums[sumIndex] += sum4[j];
                ++sumIndex;
                ++value;

                if (sumIndex >= dataLength)
                    sumIndex = 0;
            }

            int offset = chunks * valuesPerVector;
            for (int i = 0; i < missingElements; i += dataLength)
                for (int j = 0; j < sums.Length; j++)
                    sums[j] += data[offset + i + j];
        }
        else
        {
            for (int i = 0; i < data.Length; i += dataLength)
                for (int j = 0; j < sums.Length; j++)
                    sums[j] += data[i + j];
        }

        float divisor = count * byte.MaxValue;
        for (int i = 0; i < pixelData.Length; i++)
            pixelData[i] = (sums[i] / divisor).GetByteValueFromPercentage();
    }

    #endregion
}