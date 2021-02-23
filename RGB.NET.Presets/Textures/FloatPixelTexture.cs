using System;
using RGB.NET.Core;
using RGB.NET.Presets.Textures.Sampler;

namespace RGB.NET.Presets.Textures
{
    public sealed class FloatPixelTexture : PixelTexture<float>
    {
        #region Properties & Fields

        private readonly int _stride;

        public ColorFormat ColorFormat { get; }

        public override Color this[in Rectangle rectangle]
        {
            get
            {
                Color color = base[rectangle];
                if (ColorFormat == ColorFormat.BGR)
                    return new Color(color.A, color.B, color.G, color.R);

                return color;
            }
        }

        #endregion

        #region Constructors

        public FloatPixelTexture(int with, int height, float[] data, ColorFormat colorFormat = ColorFormat.RGB)
            : this(with, height, data, new AverageFloatSampler(), colorFormat)
        { }

        public FloatPixelTexture(int with, int height, float[] data, ISampler<float> sampler, ColorFormat colorFormat = ColorFormat.RGB)
            : base(with, height, data, 3, sampler)
        {
            this._stride = with;
            this.ColorFormat = colorFormat;

            if (Data.Length != ((with * height) * 3)) throw new ArgumentException($"Data-Length {Data.Length} differs from the given size {with}x{height} * 3 bytes ({with * height * 3}).");
        }

        #endregion

        #region Methods

        protected override Color GetColor(int x, int y)
        {
            int offset = ((y * _stride) + x) * 3;

            if (ColorFormat == ColorFormat.BGR)
                return new Color(Data[offset + 2], Data[offset + 1], Data[offset + 1]);

            return new Color(Data[offset], Data[offset + 1], Data[offset + 2]);
        }

        protected override void GetRegionData(int x, int y, int width, int height, in Span<float> buffer)
        {
            int width3 = width * 3;
            ReadOnlySpan<float> data = Data.AsSpan();
            for (int i = 0; i < height; i++)
            {
                ReadOnlySpan<float> dataSlice = data.Slice((((y + i) * _stride) + x) * 3, width3);
                Span<float> destination = buffer.Slice(i * width3, width3);
                dataSlice.CopyTo(destination);
            }
        }

        #endregion
    }
}
