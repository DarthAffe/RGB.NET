using System;
using RGB.NET.Core;
using RGB.NET.Presets.Textures.Sampler;

namespace RGB.NET.Presets.Textures
{
    public sealed class BytePixelTexture : PixelTexture<byte>
    {
        #region Properties & Fields

        private readonly byte[] _data;
        protected override ReadOnlySpan<byte> Data => _data;

        public ColorFormat ColorFormat { get; }

        #endregion

        #region Constructors

        public BytePixelTexture(int with, int height, byte[] data, ColorFormat colorFormat = ColorFormat.RGB)
            : this(with, height, data, new AverageByteSampler(), colorFormat)
        { }

        public BytePixelTexture(int with, int height, byte[] data, ISampler<byte> sampler, ColorFormat colorFormat = ColorFormat.RGB)
            : base(with, height, 3, sampler)
        {
            this._data = data;
            this.ColorFormat = colorFormat;

            if (Data.Length != ((with * height) * 3)) throw new ArgumentException($"Data-Length {Data.Length} differs from the given size {with}x{height} * 3 bytes ({with * height * 3}).");
        }

        #endregion

        #region Methods

        protected override Color GetColor(in ReadOnlySpan<byte> pixel)
        {
            if (ColorFormat == ColorFormat.BGR)
                return new Color(pixel[2], pixel[1], pixel[0]);

            return new Color(pixel[0], pixel[1], pixel[2]);
        }

        #endregion
    }
}
