namespace RGB.NET.Core
{
    /// <summary>
    /// Represents a generic Id of a <see cref="Led"/>.
    /// </summary>
    public interface ILedId
    {
        /// <summary>
        /// Gets a value indicating if this <see cref="ILedId"/> is valid.
        /// </summary>
        bool IsValid { get; }
    }
}
