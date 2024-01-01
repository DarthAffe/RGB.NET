using System;

namespace RGB.NET.Core;

/// <summary>
/// Represents a generic update queue.
/// </summary>
/// <typeparam name="TIdentifier">The identifier used to identify the data processed by this queue.</typeparam>
/// <typeparam name="TData">The type of the data processed by this queue.</typeparam>
public interface IUpdateQueue<TIdentifier, TData> : IReferenceCounting, IDisposable
    where TIdentifier : notnull
{
    /// <summary>
    /// Gets a bool indicating if the queue requires a flush of all data due to an internal error.
    /// </summary>
    bool RequiresFlush { get; }

    /// <summary>
    /// Sets or merges the provided data set in the current dataset and notifies the trigger that there is new data available.
    /// </summary>
    /// <param name="dataSet">The set of data.</param>
    // ReSharper disable once MemberCanBeProtected.Global
    void SetData(ReadOnlySpan<(TIdentifier, TData)> dataSet);

    /// <summary>
    /// Resets the current data set.
    /// </summary>
    void Reset();
}

/// <summary>
/// Represents a generic update queue processing <see cref="Color"/>-data using <see cref="object"/>-identifiers.
/// </summary>
public interface IUpdateQueue : IUpdateQueue<object, Color>;