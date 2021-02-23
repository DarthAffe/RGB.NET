using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace RGB.NET.Core
{
    public abstract class PixelTexture<T> : ITexture
        where T : unmanaged
    {
        #region Constants

        private const int STACK_ALLOC_LIMIT = 1024;

        #endregion

        #region Properties & Fields

        private readonly int _dataPerColor;

        protected ISampler<T> Sampler { get; set; }
        protected T[] Data { get; set; }

        public Size Size { get; }

        public virtual Color this[in Point point]
        {
            get
            {
                if (Data.Length == 0) return Color.Transparent;

                int x = (int)Math.Round(Size.Width * point.X.Clamp(0, 1));
                int y = (int)Math.Round(Size.Height * point.Y.Clamp(0, 1));
                return GetColor(x, y);
            }
        }

        public virtual Color this[in Rectangle rectangle]
        {
            get
            {
                if (Data.Length == 0) return Color.Transparent;

                int x = (int)Math.Round(Size.Width * rectangle.Location.X.Clamp(0, 1));
                int y = (int)Math.Round(Size.Height * rectangle.Location.Y.Clamp(0, 1));
                int width = (int)Math.Round(Size.Width * rectangle.Size.Width.Clamp(0, 1));
                int height = (int)Math.Round(Size.Height * rectangle.Size.Height.Clamp(0, 1));

                int bufferSize = width * height * _dataPerColor;
                if (bufferSize <= STACK_ALLOC_LIMIT)
                {
                    Span<T> buffer = stackalloc T[bufferSize];
                    GetRegionData(x, y, width, height, buffer);
                    return Sampler.SampleColor(new SamplerInfo<T>(width, height, buffer));
                }
                else
                {
                    T[] rent = ArrayPool<T>.Shared.Rent(bufferSize);
                    Span<T> buffer = new Span<T>(rent).Slice(0, bufferSize);
                    GetRegionData(x, y, width, height, buffer);
                    Color color = Sampler.SampleColor(new SamplerInfo<T>(width, height, buffer));
                    ArrayPool<T>.Shared.Return(rent);

                    return color;
                }
            }
        }

        #endregion

        #region Constructors

        public PixelTexture(int with, int height, T[] data, int dataPerColor, ISampler<T> sampler)
        {
            this.Data = data;
            this._dataPerColor = dataPerColor;
            this.Sampler = sampler;

            Size = new Size(with, height);
        }

        #endregion

        #region Methods

        protected abstract Color GetColor(int x, int y);
        protected abstract void GetRegionData(int x, int y, int width, int height, in Span<T> buffer);

        #endregion
    }

    public sealed class PixelTexture : PixelTexture<Color>
    {
        #region Properties & Fields

        private readonly int _stride;

        #endregion

        #region Constructors

        public PixelTexture(int with, int height, Color[] data)
            : this(with, height, data, new AverageColorSampler())
        { }

        public PixelTexture(int with, int height, Color[] data, ISampler<Color> sampler)
            : base(with, height, data, 1, sampler)
        {
            this._stride = with;

            if (Data.Length != (with * height)) throw new ArgumentException($"Data-Length {Data.Length} differs from the given size {with}x{height} ({with * height}).");
        }

        #endregion

        #region Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override Color GetColor(int x, int y) => Data[(y * _stride) + x];

        protected override void GetRegionData(int x, int y, int width, int height, in Span<Color> buffer)
        {
            Span<Color> data = Data.AsSpan();
            for (int i = 0; i < height; i++)
            {
                Span<Color> dataSlice = data.Slice(((y + i) * _stride) + x, width);
                Span<Color> destination = buffer.Slice(i * width, width);
                dataSlice.CopyTo(destination);
            }
        }

        #endregion
    }
}
