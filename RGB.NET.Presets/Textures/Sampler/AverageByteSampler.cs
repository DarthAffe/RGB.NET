using System;
using RGB.NET.Core;

namespace RGB.NET.Presets.Textures.Sampler
{
    public class AverageByteSampler : ISampler<byte>
    {
        #region Methods

        public Color SampleColor(SamplerInfo<byte> info)
        {
            int count = info.Width * info.Height;
            if (count == 0) return Color.Transparent;

            ReadOnlySpan<byte> data = info.Data;

            uint r = 0, g = 0, b = 0;
            for (int i = 0; i < data.Length; i += 3)
            {
                r += data[i];
                g += data[i + 1];
                b += data[i + 2];
            }

            float divisor = count * byte.MaxValue;
            return new Color(r / divisor, g / divisor, b / divisor);
        }

        #endregion
    }
}
