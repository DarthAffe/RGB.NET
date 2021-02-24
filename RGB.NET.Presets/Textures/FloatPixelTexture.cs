using System;
using RGB.NET.Core;
using RGB.NET.Presets.Textures.Sampler;

namespace RGB.NET.Presets.Textures
{
    public sealed class FloatPixelTexture : PixelTexture<float>
    {
        #region Properties & Fields

        private readonly float[] _data;
        protected override ReadOnlySpan<float> Data => _data;

        public ColorFormat ColorFormat { get; }

        #endregion

        #region Constructors

        public FloatPixelTexture(int with, int height, float[] data, ColorFormat colorFormat = ColorFormat.RGB)
            : this(with, height, data, new AverageFloatSampler(), colorFormat)
        { }

        public FloatPixelTexture(int with, int height, float[] data, ISampler<float> sampler, ColorFormat colorFormat = ColorFormat.RGB)
            : base(with, height, 3, sampler)
        {
            this._data = data;
            this.ColorFormat = colorFormat;

            if (Data.Length != ((with * height) * 3)) throw new ArgumentException($"Data-Length {Data.Length} differs from the given size {with}x{height} * 3 bytes ({with * height * 3}).");
        }

        #endregion

        #region Methods

        protected override Color GetColor(ReadOnlySpan<float> pixel)
        {
            if (ColorFormat == ColorFormat.BGR)
                return new Color(pixel[2], pixel[1], pixel[0]);

            return new Color(pixel[0], pixel[1], pixel[2]);
        }

        #endregion
    }
}
