using System.Collections.Generic;

namespace RGB.NET.Core
{
    /// <summary>
    /// Represents a special part of a <see cref="IRGBDevice"/>.
    /// </summary>
    public interface IRGBDeviceSpecialPart : IEnumerable<Led>
    { }
}
