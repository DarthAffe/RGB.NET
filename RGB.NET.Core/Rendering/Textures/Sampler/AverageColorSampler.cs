namespace RGB.NET.Core
{
    public class AverageColorSampler : ISampler<Color>
    {
        #region Properties & Fields

        public bool SampleAlpha { get; set; }

        #endregion

        #region Methods

        public Color SampleColor(SamplerInfo<Color> info)
        {
            int count = info.Width * info.Height;
            if (count == 0) return Color.Transparent;

            float a = 0, r = 0, g = 0, b = 0;
            foreach (Color color in info.Data)
            {
                a += color.A;
                r += color.R;
                g += color.G;
                b += color.B;
            }

            return SampleAlpha
                       ? new Color(a / count, r / count, g / count, b / count)
                       : new Color(r / count, g / count, b / count);
        }

        #endregion
    }
}
