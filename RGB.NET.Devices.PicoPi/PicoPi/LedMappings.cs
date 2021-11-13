using RGB.NET.Core;

namespace RGB.NET.Devices.PicoPi;

/// <summary>
/// Contains mappings for <see cref="LedId"/> to the buffer-offset.
/// </summary>
public static class LedMappings
{
    #region Properties & Fields

    /// <summary>
    /// Gets the defautlt offset-mapping.
    /// </summary>
    public static LedMapping<int> StripeMapping = new();

    #endregion

    #region Constructors

    static LedMappings()
    {
        for (int i = 0; i < 255; i++)
            StripeMapping.Add(LedId.LedStripe1 + i, i);
    }

    #endregion
}