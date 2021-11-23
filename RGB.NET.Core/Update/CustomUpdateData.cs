using System.Collections.Generic;

namespace RGB.NET.Core;

/// <summary>
/// Represents an index used to identify data in the <see cref="CustomUpdateData"/>.
/// </summary>
public static class CustomUpdateDataIndex
{
    /// <summary>
    /// Checked by the <see cref="RGBSurface"/> to see if all LEDs needs to be flushed even if they aren't changed in this update.
    /// default: false
    /// </summary>
    public const string FLUSH_LEDS = "flushLeds";

    /// <summary>
    /// Checked by the <see cref="RGBSurface"/> to see if the surface should be rendered in this update.
    /// default: true
    /// </summary>
    public const string RENDER = "render";

    /// <summary>
    /// Checked by the <see cref="RGBSurface"/> to see if devies should be updated after rendering.
    /// default: true
    /// </summary>
    public const string UPDATE_DEVICES = "updateDevices";
}

/// <summary>
/// Represents a set of custom data, each indexed by a string-key.
/// </summary>
public class CustomUpdateData
{
    #region Properties & Fields

    private Dictionary<string, object?> _data = new();

    #endregion

    #region Indexer

    /// <summary>
    /// Gets or sets the value for a specific key.
    /// </summary>
    /// <param name="key">The key of the value.</param>
    /// <returns>The value represented by the specified key.</returns>
    public object? this[string key]
    {
        get => _data.TryGetValue(key.ToUpperInvariant(), out object? data) ? data : default;
        set => _data[key.ToUpperInvariant()] = value;
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomUpdateData"/> class.
    /// </summary>
    public CustomUpdateData()
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomUpdateData"/> class.
    /// </summary>
    /// <param name="values">A params-list of tuples containing the key (string) and the value of a specific custom-data.</param>
    public CustomUpdateData(params (string key, object value)[] values)
    {
        foreach ((string key, object value) in values)
            this[key] = value;
    }

    #endregion
}
