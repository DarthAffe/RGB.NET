using RGB.NET.Core;
using RGB.NET.Devices.Corsair.Native;
using System.Runtime.InteropServices;

namespace RGB.NET.Devices.Corsair;

internal static class NativeExtensions
{
    internal static Rectangle ToRectangle(this _CorsairLedPosition position)
    {
        const float WIDTH = 10;
        const float HEIGHT = 10;
        float posX = (float)position.cx;
        float posY = (float)position.cy;

        return new Rectangle(posX, posY, WIDTH, HEIGHT);
    }

    internal static T[] ToArray<T>(this nint ptr, int size)
    {
        int tSize = Marshal.SizeOf<T>();
        T[] array = new T[size];
        nint loopPtr = ptr;
        for (int i = 0; i < size; i++)
        {
            array[i] = Marshal.PtrToStructure<T>(loopPtr)!;
            loopPtr += tSize;
        }

        return array;
    }
}