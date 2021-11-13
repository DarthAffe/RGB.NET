using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;

namespace RGB.NET.Core;

/// <summary>
/// Represents a generic update queue.
/// </summary>
/// <typeparam name="TIdentifier">The type of the key used to identify some data.</typeparam>
/// <typeparam name="TData">The type of the data.</typeparam>
public abstract class UpdateQueue<TIdentifier, TData> : IUpdateQueue<TIdentifier, TData>
    where TIdentifier : notnull
{
    #region Properties & Fields

    private readonly object _dataLock = new();
    private readonly IDeviceUpdateTrigger _updateTrigger;
    private readonly Dictionary<TIdentifier, TData> _currentDataSet = new();

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateQueue{TIdentifier,TData}"/> class.
    /// </summary>
    /// <param name="updateTrigger">The <see cref="IDeviceUpdateTrigger"/> causing this queue to update.</param>
    protected UpdateQueue(IDeviceUpdateTrigger updateTrigger)
    {
        this._updateTrigger = updateTrigger;

        _updateTrigger.Starting += OnStartup;
        _updateTrigger.Update += OnUpdate;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Event handler for the <see cref="IUpdateTrigger.Update"/>-event.
    /// </summary>
    /// <param name="sender">The <see cref="IUpdateTrigger"/> causing this update.</param>
    /// <param name="customData"><see cref="CustomUpdateData"/> provided by the trigger.</param>
    protected virtual void OnUpdate(object? sender, CustomUpdateData customData)
    {
        (TIdentifier, TData)[] dataSet;
        Span<(TIdentifier, TData)> data;
        lock (_dataLock)
        {
            if (_currentDataSet.Count == 0) return;

            dataSet = ArrayPool<(TIdentifier, TData)>.Shared.Rent(_currentDataSet.Count);
            data = new Span<(TIdentifier, TData)>(dataSet)[.._currentDataSet.Count];

            int i = 0;
            foreach ((TIdentifier key, TData value) in _currentDataSet)
                data[i++] = (key, value);

            _currentDataSet.Clear();
        }

        Update(data);

        ArrayPool<(TIdentifier, TData)>.Shared.Return(dataSet);
    }

    /// <summary>
    /// Event handler for the <see cref="IUpdateTrigger.Starting"/>-event.
    /// </summary>
    /// <param name="sender">The starting <see cref="IUpdateTrigger"/>.</param>
    /// <param name="customData"><see cref="CustomUpdateData"/> provided by the trigger.</param>
    protected virtual void OnStartup(object? sender, CustomUpdateData customData) { }

    /// <summary>
    /// Performs the update this queue is responsible for.
    /// </summary>
    /// <param name="dataSet">The set of data that needs to be updated.</param>
    protected abstract void Update(in ReadOnlySpan<(TIdentifier key, TData color)> dataSet);

    /// <summary>
    /// Sets or merges the provided data set in the current dataset and notifies the trigger that there is new data available.
    /// </summary>
    /// <param name="dataSet">The set of data.</param>
    // ReSharper disable once MemberCanBeProtected.Global
    public virtual void SetData(IEnumerable<(TIdentifier, TData)> dataSet)
    {
        IList<(TIdentifier, TData)> data = dataSet.ToList();
        if (data.Count == 0) return;

        lock (_dataLock)
        {
            foreach ((TIdentifier key, TData value) in data)
                _currentDataSet[key] = value;
        }

        _updateTrigger.TriggerHasData();
    }

    /// <summary>
    /// Resets the current data set.
    /// </summary>
    public virtual void Reset()
    {
        lock (_dataLock)
            _currentDataSet.Clear();
    }

    /// <inheritdoc />
    public virtual void Dispose()
    {
        _updateTrigger.Starting -= OnStartup;
        _updateTrigger.Update -= OnUpdate;

        Reset();

        GC.SuppressFinalize(this);
    }

    #endregion
}

/// <summary>
/// Represents a generic <see cref="UpdateQueue{TIdentifier,TData}"/> using an object as the key and a color as the value.
/// </summary>
public abstract class UpdateQueue : UpdateQueue<object, Color>, IUpdateQueue
{
    #region Constructors

    /// <inheritdoc />
    protected UpdateQueue(IDeviceUpdateTrigger updateTrigger)
        : base(updateTrigger)
    { }

    #endregion
}