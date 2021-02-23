using System;

namespace RGB.NET.Core
{
    public readonly ref struct SamplerInfo<T>
    {
        #region Properties & Fields

        public int Width { get; }
        public int Height { get; }
        public ReadOnlySpan<T> Data { get; }

        #endregion

        #region Constructors

        public SamplerInfo(int width, int height, ReadOnlySpan<T> data)
        {
            this.Width = width;
            this.Height = height;
            this.Data = data;
        }

        #endregion
    }
}
