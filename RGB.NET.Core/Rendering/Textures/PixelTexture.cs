using System;

namespace RGB.NET.Core
{
    public sealed class PixelTexture : ITexture
    {
        #region Properties & Fields

        private readonly int _dataWidth;
        private readonly int _dataHeight;
        private readonly ReadOnlyMemory<Color> _data;
        private readonly ISampler _sampler;

        public Size Size { get; }

        public Color this[in Point point]
        {
            get
            {
                if (_data.IsEmpty) return Color.Transparent;

                int x = (int)Math.Round(Size.Width * point.X.Clamp(0, 1));
                int y = (int)Math.Round(Size.Height * point.Y.Clamp(0, 1));

                return _data.Span[(y * _dataWidth) + x];
            }
        }

        public Color this[in Rectangle rectangle]
        {
            get
            {
                if (_data.IsEmpty) return Color.Transparent;

                int x = (int)Math.Round(Size.Width * rectangle.Location.X.Clamp(0, 1));
                int y = (int)Math.Round(Size.Height * rectangle.Location.Y.Clamp(0, 1));
                int width = (int)Math.Round(Size.Width * rectangle.Size.Width.Clamp(0, 1));
                int height = (int)Math.Round(Size.Height * rectangle.Size.Height.Clamp(0, 1));

                return _sampler.SampleColor(in _data, x, y, width, height);
            }
        }

        #endregion

        #region Constructors

        public PixelTexture(int with, int height, ReadOnlyMemory<Color> data)
            : this(with, height, data, new AverageSampler())
        { }

        public PixelTexture(int with, int height, ReadOnlyMemory<Color> data, ISampler sampler)
        {
            this._data = data;
            this._dataWidth = with;
            this._dataHeight = height;
            this._sampler = sampler;

            if (_data.Length != (with * height)) throw new ArgumentException($"Data-Length {_data.Length} differs from the given size {with}x{height} ({with * height}).");

            Size = new Size(with, height);
        }

        #endregion
    }
}
