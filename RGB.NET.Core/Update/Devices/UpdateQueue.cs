using System;
using System.Collections.Generic;
using System.Linq;

namespace RGB.NET.Core
{
    /// <summary>
    /// Represents a generic update queue.
    /// </summary>
    /// <typeparam name="TIdentifier">The type of the key used to identify some data.</typeparam>
    /// <typeparam name="TData">The type of the data.</typeparam>
    public abstract class UpdateQueue<TIdentifier, TData> : IDisposable
    {
        #region Properties & Fields

        private readonly object _dataLock = new object();
        private readonly IDeviceUpdateTrigger _updateTrigger;
        private Dictionary<TIdentifier, TData> _currentDataSet;

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
        protected virtual void OnUpdate(object sender, CustomUpdateData customData)
        {
            Dictionary<TIdentifier, TData> dataSet;
            lock (_dataLock)
            {
                dataSet = _currentDataSet;
                _currentDataSet = null;
            }

            if ((dataSet != null) && (dataSet.Count != 0))
                Update(dataSet);
        }

        /// <summary>
        /// Event handler for the <see cref="IUpdateTrigger.Starting"/>-event.
        /// </summary>
        /// <param name="sender">The starting <see cref="IUpdateTrigger"/>.</param>
        /// <param name="customData"><see cref="CustomUpdateData"/> provided by the trigger.</param>
        protected virtual void OnStartup(object sender, CustomUpdateData customData) { }

        /// <summary>
        /// Performs the update this queue is responsible for.
        /// </summary>
        /// <param name="dataSet">The set of data that needs to be updated.</param>
        protected abstract void Update(Dictionary<TIdentifier, TData> dataSet);

        /// <summary>
        /// Sets or merges the provided data set in the current dataset and notifies the trigger that there is new data available.
        /// </summary>
        /// <param name="dataSet">The set of data.</param>
        // ReSharper disable once MemberCanBeProtected.Global
        public virtual void SetData(Dictionary<TIdentifier, TData> dataSet)
        {
            if ((dataSet == null) || (dataSet.Count == 0)) return;

            lock (_dataLock)
            {
                if (_currentDataSet == null)
                    _currentDataSet = dataSet;
                else
                {
                    foreach (KeyValuePair<TIdentifier, TData> command in dataSet)
                        _currentDataSet[command.Key] = command.Value;
                }
            }

            _updateTrigger.TriggerHasData();
        }

        /// <summary>
        /// Resets the current data set.
        /// </summary>
        public virtual void Reset()
        {
            lock (_dataLock)
                _currentDataSet = null;
        }

        public void Dispose()
        {
            _updateTrigger.Starting -= OnStartup;
            _updateTrigger.Update -= OnUpdate;

            Reset();
        }

        #endregion
    }

    /// <summary>
    /// Represents a generic <see cref="UpdateQueue{TIdentifier,TData}"/> using an object as the key and a color as the value.
    /// </summary>
    public abstract class UpdateQueue : UpdateQueue<object, Color>
    {
        #region Constructors

        /// <inheritdoc />
        protected UpdateQueue(IDeviceUpdateTrigger updateTrigger)
            : base(updateTrigger)
        { }

        #endregion

        #region Methods

        /// <summary>
        /// Calls <see cref="UpdateQueue{TIdentifier,TData}.SetData"/> for a data set created out of the provided list of <see cref="Led"/>.
        /// </summary>
        /// <param name="leds"></param>
        public void SetData(IEnumerable<Led> leds) => SetData(leds?.ToDictionary(x => x.CustomData ?? x.Id, x => x.Color));

        #endregion
    }
}
