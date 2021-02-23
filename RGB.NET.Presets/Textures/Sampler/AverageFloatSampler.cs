using System;
using RGB.NET.Core;

namespace RGB.NET.Presets.Textures.Sampler
{
    public class AverageFloatSampler : ISampler<float>
    {
        #region Methods

        public Color SampleColor(SamplerInfo<float> info)
        {
            int count = info.Width * info.Height;
            if (count == 0) return Color.Transparent;

            ReadOnlySpan<float> data = info.Data;

            float r = 0, g = 0, b = 0;
            for (int i = 0; i < data.Length; i += 3)
            {
                r += data[i];
                g += data[i + 1];
                b += data[i + 2];
            }

            return new Color(r / count, g / count, b / count);
        }

        #endregion
    }
}
