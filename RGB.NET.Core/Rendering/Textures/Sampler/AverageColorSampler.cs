using System;

namespace RGB.NET.Core;

/// <summary>
/// Represents a sampled that averages multiple color to a single color.
/// </summary>
/// <remarks>
/// Averages all components (A, R, G, B) of the colors separately which isn't ideal in cases where multiple different colors are combined.
/// </remarks>
public class AverageColorSampler : ISampler<Color>
{
    #region Methods

    /// <inheritdoc />
    public void Sample(in SamplerInfo<Color> info, in Span<Color> pixelData)
    {
        int count = info.Width * info.Height;
        if (count == 0) return;

        float a = 0, r = 0, g = 0, b = 0;
        foreach (Color color in info.Data)
        {
            a += color.A;
            r += color.R;
            g += color.G;
            b += color.B;
        }

        pixelData[0] = new Color(a / count, r / count, g / count, b / count);
    }

    #endregion
}