using System;
using System.Collections.Generic;

namespace RGB.NET.Core;

/// <summary>
/// Represents a generic update queue.
/// </summary>
/// <typeparam name="TIdentifier">The identifier used to identify the data processed by this queue.</typeparam>
/// <typeparam name="TData">The type of the data processed by this queue.</typeparam>
public interface IUpdateQueue<TIdentifier, TData> : IDisposable
    where TIdentifier : notnull
{
    /// <summary>
    /// Sets or merges the provided data set in the current dataset and notifies the trigger that there is new data available.
    /// </summary>
    /// <param name="dataSet">The set of data.</param>
    // ReSharper disable once MemberCanBeProtected.Global
    void SetData(IEnumerable<(TIdentifier, TData)> dataSet);

    /// <summary>
    /// Resets the current data set.
    /// </summary>
    void Reset();
}

/// <summary>
/// Represents a generic update queue processing <see cref="Color"/>-data using <see cref="object"/>-identifiers.
/// </summary>
public interface IUpdateQueue : IUpdateQueue<object, Color>
{ }