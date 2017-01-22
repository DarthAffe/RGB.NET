// ReSharper disable UnusedMember.Global

namespace RGB.NET.Core
{
    /// <summary>
    /// Contains a list of all brush calculation modes.
    /// </summary>
    public enum BrushCalculationMode
    {
        /// <summary>
        /// The calculation <see cref="Rectangle"/> for <see cref="IBrush"/> will be the rectangle around the <see cref="ILedGroup"/> the <see cref="IBrush"/> is applied to.
        /// </summary>
        Relative,

        /// <summary>
        /// The calculation <see cref="Rectangle"/> for <see cref="IBrush"/> will always be the rectangle completly containing all affected <see cref="IRGBDevice"/>.
        /// </summary>
        Absolute
    }
}
