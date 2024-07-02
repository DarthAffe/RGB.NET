using System;
using System.Numerics;

namespace RGB.NET.Core;

/// <summary>
/// Represents a sampled that averages multiple color to a single color.
/// </summary>
/// <remarks>
/// Averages all components (A, R, G, B) of the colors separately which isn't ideal in cases where multiple different colors are combined.
/// </remarks>
public sealed class AverageColorSampler : ISampler<Color>
{
    #region Constants

    private const int VALUES_PER_COLOR = 4;
    private static readonly int ELEMENTS_PER_VECTOR = Vector<float>.Count / VALUES_PER_COLOR;
    private static readonly int VALUES_PER_VECTOR = ELEMENTS_PER_VECTOR * VALUES_PER_COLOR;

    #endregion

    #region Methods

    /// <inheritdoc />
    public unsafe void Sample(in SamplerInfo<Color> info, Span<Color> pixelData)
    {
        int count = info.Width * info.Height;
        if (count == 0) return;

        float a = 0, r = 0, g = 0, b = 0;

        if (Vector.IsHardwareAccelerated && (info.Height > 1) && (info.Width >= ELEMENTS_PER_VECTOR))
        {
            int chunks = info.Width / ELEMENTS_PER_VECTOR;
            int missingElements = info.Width - (chunks * ELEMENTS_PER_VECTOR);

            Vector<float> sum = Vector<float>.Zero;

            for (int y = 0; y < info.Height; y++)
            {
                ReadOnlySpan<Color> data = info[y];

                fixed (Color* colorPtr = data)
                {
                    Color* current = colorPtr;
                    for (int i = 0; i < chunks; i++)
                    {
                        sum = Vector.Add(sum, *(Vector<float>*)current);
                        current += ELEMENTS_PER_VECTOR;
                    }
                }

                for (int i = 0; i < missingElements; i++)
                {
                    Color color = data[^(i + 1)];

                    a += color.A;
                    r += color.R;
                    g += color.G;
                    b += color.B;
                }
            }

            for (int i = 0; i < VALUES_PER_VECTOR; i += VALUES_PER_COLOR)
            {
                a += sum[i];
                r += sum[i + 1];
                g += sum[i + 2];
                b += sum[i + 3];
            }
        }
        else
        {
            for (int y = 0; y < info.Height; y++)
                foreach (Color color in info[y])
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