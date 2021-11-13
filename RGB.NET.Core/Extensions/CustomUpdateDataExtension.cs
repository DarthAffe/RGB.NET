namespace RGB.NET.Core;

/// <summary>
/// Offers some extensions for easier use of <see cref="CustomUpdateData"/>.
/// </summary>
public static class CustomUpdateDataExtension
{
    /// <summary>
    /// Sets the <see cref="CustomUpdateDataIndex.FLUSH_LEDS"/>-Parameter to the given value.
    /// </summary>
    /// <param name="customUpdateData">The update-data to modify.</param>
    /// <param name="value">The value to set.</param>
    /// <returns>The modified update-data.</returns>
    public static CustomUpdateData FlushLeds(this CustomUpdateData customUpdateData, bool value = true)
    {
        customUpdateData[CustomUpdateDataIndex.FLUSH_LEDS] = value;
        return customUpdateData;
    }

    /// <summary>
    /// Sets the <see cref="CustomUpdateDataIndex.RENDER"/>-Parameter to the given value.
    /// </summary>
    /// <param name="customUpdateData">The update-data to modify.</param>
    /// <param name="value">The value to set.</param>
    /// <returns>The modified update-data.</returns>
    public static CustomUpdateData Render(this CustomUpdateData customUpdateData, bool value = true)
    {
        customUpdateData[CustomUpdateDataIndex.RENDER] = value;
        return customUpdateData;
    }

    /// <summary>
    /// Sets the <see cref="CustomUpdateDataIndex.UPDATE_DEVICES"/>-Parameter to the given value.
    /// </summary>
    /// <param name="customUpdateData">The update-data to modify.</param>
    /// <param name="value">The value to set.</param>
    /// <returns>The modified update-data.</returns>
    public static CustomUpdateData UpdateDevices(this CustomUpdateData customUpdateData, bool value = true)
    {
        customUpdateData[CustomUpdateDataIndex.UPDATE_DEVICES] = value;
        return customUpdateData;
    }
}