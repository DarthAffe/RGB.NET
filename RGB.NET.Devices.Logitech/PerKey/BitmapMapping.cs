using System.Runtime.CompilerServices;
using RGB.NET.Core;

namespace RGB.NET.Devices.Logitech
{
    internal static class BitmapMapping
    {
        #region Constants

        private const int BITMAP_SIZE = 21 * 6 * 4;

        #endregion
        
        #region Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static byte[] CreateBitmap() => new byte[BITMAP_SIZE];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void SetColor(byte[] bitmap, int offset, Color color)
        {
            bitmap[offset] = color.GetB();
            bitmap[offset + 1] = color.GetG();
            bitmap[offset + 2] = color.GetR();
            bitmap[offset + 3] = color.GetA();
        }

        #endregion
    }
}
