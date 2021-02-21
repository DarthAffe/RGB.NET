using System;
using System.Runtime.CompilerServices;

namespace RGB.NET.Core
{
    public abstract class PixelTexture<T> : ITexture
    {
        #region Properties & Fields

        protected ISampler Sampler { get; set; }
        protected T[] Data { get; set; }

        public Size Size { get; }

        public Color this[in Point point]
        {
            get
            {
                if (Data.Length == 0) return Color.Transparent;

                int x = (int)Math.Round(Size.Width * point.X.Clamp(0, 1));
                int y = (int)Math.Round(Size.Height * point.Y.Clamp(0, 1));
                return GetColor(x, y);
            }
        }

        public Color this[in Rectangle rectangle]
        {
            get
            {
                if (Data.Length == 0) return Color.Transparent;

                int x = (int)Math.Round(Size.Width * rectangle.Location.X.Clamp(0, 1));
                int y = (int)Math.Round(Size.Height * rectangle.Location.Y.Clamp(0, 1));
                int width = (int)Math.Round(Size.Width * rectangle.Size.Width.Clamp(0, 1));
                int height = (int)Math.Round(Size.Height * rectangle.Size.Height.Clamp(0, 1));

                return Sampler.SampleColor(x, y, width, height, GetColor);
            }
        }

        #endregion

        #region Constructors

        public PixelTexture(int with, int height, T[] data, ISampler sampler)
        {
            this.Data = data;
            this.Sampler = sampler;

            Size = new Size(with, height);
        }

        #endregion

        #region Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected abstract Color GetColor(int x, int y);

        #endregion
    }

    public sealed class PixelTexture : PixelTexture<Color>
    {
        #region Properties & Fields

        private readonly int _stride;

        #endregion

        #region Constructors

        public PixelTexture(int with, int height, Color[] data)
            : this(with, height, data, new AverageSampler())
        { }

        public PixelTexture(int with, int height, Color[] data, ISampler sampler)
            : base(with, height, data, sampler)
        {
            this._stride = with;

            if (Data.Length != (with * height)) throw new ArgumentException($"Data-Length {Data.Length} differs from the given size {with}x{height} ({with * height}).");
        }

        #endregion

        #region Methods

        protected override Color GetColor(int x, int y) => Data[(y * _stride) + x];

        #endregion
    }
}
