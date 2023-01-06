using RGB.NET.Core;
using RGB.NET.Devices.Corsair.Native;

namespace RGB.NET.Devices.Corsair;

internal static class NativeExtensions
{
    internal static Rectangle ToRectangle(this _CorsairLedPosition position)
    {
        //HACK DarthAffe 08.07.2018: It seems like corsair introduced a bug here - it's always 0.
        float width = position.width < 0.5f ? 10 : (float)position.width;
        float height = position.height < 0.5f ? 10 : (float)position.height;
        float posX = (float)position.left;
        float posY = (float)position.top;

        return new Rectangle(posX, posY, width, height);
    }
}