using System;

namespace RGB.NET.Core
{
    public class AverageSampler : ISampler
    {
        #region Properties & Fields

        private Func<ReadOnlyMemory<Color>, int, int, int, int, Color> _sampleMethod = SampleWithoutAlpha;

        private bool _sampleAlpha;
        public bool SampleAlpha
        {
            get => _sampleAlpha;
            set
            {
                _sampleAlpha = value;
                _sampleMethod = value ? SampleWithAlpha : SampleWithoutAlpha;
            }
        }

        #endregion

        #region Methods

        public Color SampleColor(in ReadOnlyMemory<Color> data, int x, int y, int width, int height) => _sampleMethod(data, x, y, width, height);

        private static Color SampleWithAlpha(ReadOnlyMemory<Color> data, int x, int y, int width, int height)
        {
            ReadOnlySpan<Color> span = data.Span;

            int maxY = y + height;
            int count = width * height;
            double a = 0, r = 0, g = 0, b = 0;
            for (int yPos = y; yPos < maxY; yPos++)
            {
                ReadOnlySpan<Color> line = span.Slice((yPos * width) + x, width);
                foreach (Color color in line)
                {
                    a += color.A;
                    r += color.R;
                    g += color.G;
                    b += color.B;
                }
            }

            if (count == 0) return Color.Transparent;

            return new Color(a / count, r / count, g / count, b / count);
        }

        private static Color SampleWithoutAlpha(ReadOnlyMemory<Color> data, int x, int y, int width, int height)
        {
            ReadOnlySpan<Color> span = data.Span;

            int maxY = y + height;
            int count = width * height;
            double r = 0, g = 0, b = 0;
            for (int yPos = y; yPos < maxY; yPos++)
            {
                ReadOnlySpan<Color> line = span.Slice((yPos * width) + x, width);
                foreach (Color color in line)
                {
                    r += color.R;
                    g += color.G;
                    b += color.B;
                }
            }

            if (count == 0) return Color.Transparent;

            return new Color(r / count, g / count, b / count);
        }

        #endregion
    }
}
