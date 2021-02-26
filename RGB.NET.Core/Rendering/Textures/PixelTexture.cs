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

        private readonly int _dataPerPixel;
        private readonly int _stride;

        public ISampler<T> Sampler { get; set; }
        public Size Size { get; }

        protected abstract ReadOnlySpan<T> Data { get; }

        public virtual Color this[in Point point]
        {
            get
            {
                if (Data.Length == 0) return Color.Transparent;

                int x = (int)MathF.Round(Size.Width * point.X.Clamp(0, 1));
                int y = (int)MathF.Round(Size.Height * point.Y.Clamp(0, 1));
                return GetColor(GetPixelData(x, y));
            }
        }

        public virtual Color this[in Rectangle rectangle]
        {
            get
            {
                if (Data.Length == 0) return Color.Transparent;

                int x = (int)MathF.Round(Size.Width * rectangle.Location.X.Clamp(0, 1));
                int y = (int)MathF.Round(Size.Height * rectangle.Location.Y.Clamp(0, 1));
                int width = (int)MathF.Round(Size.Width * rectangle.Size.Width.Clamp(0, 1));
                int height = (int)MathF.Round(Size.Height * rectangle.Size.Height.Clamp(0, 1));

                if ((width == 0) || (height == 0)) return Color.Transparent;
                if ((width == 1) && (height == 1)) return GetColor(GetPixelData(x, y));

                int bufferSize = width * height * _dataPerPixel;
                if (bufferSize <= STACK_ALLOC_LIMIT)
                {
                    Span<T> buffer = stackalloc T[bufferSize];
                    GetRegionData(x, y, width, height, buffer);

                    Span<T> pixelData = stackalloc T[_dataPerPixel];
                    Sampler.SampleColor(new SamplerInfo<T>(width, height, buffer), pixelData);

                    return GetColor(pixelData);
                }
                else
                {
                    T[] rent = ArrayPool<T>.Shared.Rent(bufferSize);

                    Span<T> buffer = new Span<T>(rent).Slice(0, bufferSize);
                    GetRegionData(x, y, width, height, buffer);

                    Span<T> pixelData = stackalloc T[_dataPerPixel];
                    Sampler.SampleColor(new SamplerInfo<T>(width, height, buffer), pixelData);

                    ArrayPool<T>.Shared.Return(rent);

                    return GetColor(pixelData);
                }
            }
        }

        #endregion

        #region Constructors

        public PixelTexture(int with, int height, int dataPerPixel, ISampler<T> sampler)
        {
            this._stride = with;
            this._dataPerPixel = dataPerPixel;
            this.Sampler = sampler;

            Size = new Size(with, height);
        }

        #endregion

        #region Methods

        protected abstract Color GetColor(in ReadOnlySpan<T> pixel);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual ReadOnlySpan<T> GetPixelData(int x, int y) => Data.Slice((y * _stride) + x, _dataPerPixel);

        protected virtual void GetRegionData(int x, int y, int width, int height, in Span<T> buffer)
        {
            int dataWidth = width * _dataPerPixel;
            ReadOnlySpan<T> data = Data;
            for (int i = 0; i < height; i++)
            {
                ReadOnlySpan<T> dataSlice = data.Slice((((y + i) * _stride) + x) * _dataPerPixel, dataWidth);
                Span<T> destination = buffer.Slice(i * dataWidth, dataWidth);
                dataSlice.CopyTo(destination);
            }
        }

        #endregion
    }

    public sealed class PixelTexture : PixelTexture<Color>
    {
        #region Properties & Fields

        private readonly Color[] _data;

        protected override ReadOnlySpan<Color> Data => _data;

        #endregion

        #region Constructors

        public PixelTexture(int with, int height, Color[] data)
            : this(with, height, data, new AverageColorSampler())
        { }

        public PixelTexture(int with, int height, Color[] data, ISampler<Color> sampler)
            : base(with, height, 1, sampler)
        {
            this._data = data;

            if (Data.Length != (with * height)) throw new ArgumentException($"Data-Length {Data.Length} differs from the given size {with}x{height} ({with * height}).");
        }

        #endregion

        #region Methods

        protected override Color GetColor(in ReadOnlySpan<Color> pixel) => pixel[0];

        #endregion
    }
}
