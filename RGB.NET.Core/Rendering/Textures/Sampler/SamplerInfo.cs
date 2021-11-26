using System;

namespace RGB.NET.Core;

/// <summary>
/// Represents the information used to sample data.
/// </summary>
/// <typeparam name="T">The type of the data to sample.</typeparam>
public readonly ref struct SamplerInfo<T>
{
    #region Properties & Fields

    /// <summary>
    /// Gets the width of the region the data comes from.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Gets the height of region the data comes from.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// Gets the data to sample.
    /// </summary>
    public ReadOnlySpan<T> Data { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="SamplerInfo{T}" /> class.
    /// </summary>
    /// <param name="width">The width of the region the data comes from.</param>
    /// <param name="height">The height of region the data comes from.</param>
    /// <param name="data">The data to sample.</param>
    public SamplerInfo(int width, int height, ReadOnlySpan<T> data)
    {
        this.Width = width;
        this.Height = height;
        this.Data = data;
    }

    #endregion
}