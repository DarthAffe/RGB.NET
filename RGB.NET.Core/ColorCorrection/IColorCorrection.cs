// ReSharper disable UnusedMember.Global

namespace RGB.NET.Core
{
    /// <summary>
    /// Represents a generic color-correction.
    /// </summary>
    public interface IColorCorrection
    {
        /// <summary>
        /// Applies the <see cref="IColorCorrection"/> to the given <see cref="Color"/>. 
        /// </summary>
        /// <param name="color">The <see cref="Color"/> to correct.</param>
        Color ApplyTo(Color color);
    }
}
