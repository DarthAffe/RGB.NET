using System;

namespace RGB.NET.Core
{
    public class AverageColorSampler : ISampler<Color>
    {
        #region Methods

        public void SampleColor(in SamplerInfo<Color> info, Span<Color> pixelData)
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
}
