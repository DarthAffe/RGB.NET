using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace RGB.NET.Core;

/// <summary>
/// Represents a sampled that averages multiple color to a single color.
/// </summary>
/// <remarks>
/// Averages all components (A, R, G, B) of the colors separately which isn't ideal in cases where multiple different colors are combined.
/// </remarks>
public class AverageColorSampler : ISampler<Color>
{
    #region Constants

    private const int VALUES_PER_COLOR = 4;
    private static readonly int ELEMENTS_PER_VECTOR = Vector<float>.Count / VALUES_PER_COLOR;
    private static readonly int VALUES_PER_VECTOR = ELEMENTS_PER_VECTOR * VALUES_PER_COLOR;

    #endregion

    #region Methods

    /// <inheritdoc />
    public unsafe void Sample(in SamplerInfo<Color> info, in Span<Color> pixelData)
    {
        int count = info.Width * info.Height;
        if (count == 0) return;

        float a = 0, r = 0, g = 0, b = 0;

        if (Vector.IsHardwareAccelerated && (info.Data.Length >= Vector<float>.Count))
        {
            int chunks = info.Data.Length / ELEMENTS_PER_VECTOR;
            int missingElements = info.Data.Length - (chunks * ELEMENTS_PER_VECTOR);

            Vector<float> sum = Vector<float>.Zero;

            fixed (Color* colorPtr = &MemoryMarshal.GetReference(info.Data))
            {
                Color* current = colorPtr;
                for (int i = 0; i < chunks; i++)
                {
                    sum = Vector.Add(sum, *(Vector<float>*)current);
                    current += ELEMENTS_PER_VECTOR;
                }
            }

            for (int i = 0; i < VALUES_PER_VECTOR; i += VALUES_PER_COLOR)
            {
                a += sum[i];
                r += sum[i + 1];
                g += sum[i + 2];
                b += sum[i + 3];
            }

            for (int i = 0; i < missingElements; i++)
            {
                Color color = info.Data[^(i + 1)];

                a += color.A;
                r += color.R;
                g += color.G;
                b += color.B;
            }
        }
        else
        {
            foreach (Color color in info.Data)
            {
                a += color.A;
                r += color.R;
                g += color.G;
                b += color.B;
            }
        }

        pixelData[0] = new Color(a / count, r / count, g / count, b / count);
    }

    #endregion
}