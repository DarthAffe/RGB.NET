using System;

namespace RGB.NET.Core;

/// <summary>
/// Represents a generic sampler to combine multipel data entries to a single one.
/// </summary>
/// <typeparam name="T">The type of the data to sample.</typeparam>
public interface ISampler<T>
{
    /// <summary>
    /// Samples the specified data to a single pixel-buffer.
    /// </summary>
    /// <param name="info">The information containing the data to sample.</param>
    /// <param name="pixelData">The buffer used to write the resulting pixel to.</param>
    void Sample(in SamplerInfo<T> info, in Span<T> pixelData);
}