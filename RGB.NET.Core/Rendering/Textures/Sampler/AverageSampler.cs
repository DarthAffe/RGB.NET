namespace RGB.NET.Core
{
    public class AverageSampler : ISampler
    {
        #region Properties & Fields

        public bool SampleAlpha { get; set; }

        #endregion

        #region Methods

        public Color SampleColor(int x, int y, int width, int height, GetColor getColor)
        {
            int maxX = x + width;
            int maxY = y + height;
            int count = width * height;
            if (count == 0) return Color.Transparent;

            float a = 0, r = 0, g = 0, b = 0;
            for (int yPos = y; yPos < maxY; yPos++)
                for (int xPos = x; xPos < maxX; xPos++)
                {
                    Color color = getColor(x, y);
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
