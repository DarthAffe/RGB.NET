using RGB.NET.Core;
using RGB.NET.Devices.Corsair.Native;

namespace RGB.NET.Devices.Corsair
{
    internal static class NativeExtensions
    {
        internal static Rectangle ToRectangle(this _CorsairLedPosition position)
        {
            //HACK DarthAffe 08.07.2018: It seems like corsair introduced a bug here - it's always 0.
            double width = position.width < 0.5 ? 10 : position.width;
            double height = position.height < 0.5 ? 10 : position.height;
            double posX = position.left;
            double posY = position.top;

            return new Rectangle(posX, posY, width, height);
        }
    }
}
