using RGB.NET.Core;

namespace RGB.NET.Devices.SteelSeries.Helper;

internal static class ColorExtensions
{
    internal static int[] ToIntArray(this Color color) => [color.GetR(), color.GetG(), color.GetB()];
}