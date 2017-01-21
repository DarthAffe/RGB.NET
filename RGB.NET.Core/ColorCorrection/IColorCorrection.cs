namespace RGB.NET.Core.ColorCorrection
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
        void ApplyTo(Color color);
    }
}
