using System;
using System.Collections.Generic;
using System.Linq;

namespace RGB.NET.Core
{
    public abstract class UpdateQueue<TIdentifier, TData>
    {
        #region Properties & Fields

        private readonly object _dataLock = new object();
        private readonly IUpdateTrigger _updateTrigger;
        private Dictionary<TIdentifier, TData> _currentDataSet;

        #endregion

        #region Constructors

        public UpdateQueue(IUpdateTrigger updateTrigger)
        {
            this._updateTrigger = updateTrigger;

            _updateTrigger.Starting += (sender, args) => OnStartup();
            _updateTrigger.Update += OnUpdate;
        }

        #endregion

        #region Methods

        protected virtual void OnUpdate(object sender, EventArgs e)
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

        protected virtual void OnStartup() { }

        protected abstract void Update(Dictionary<TIdentifier, TData> dataSet);

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

        public virtual void Reset()
        {
            lock (_dataLock)
                _currentDataSet = null;
        }

        #endregion
    }

    public abstract class UpdateQueue : UpdateQueue<object, Color>
    {
        #region Constructors

        /// <inheritdoc />
        protected UpdateQueue(IUpdateTrigger updateTrigger)
            : base(updateTrigger)
        { }

        #endregion

        #region Methods

        public void SetData(IEnumerable<Led> leds) => SetData(leds?.ToDictionary(x => x.CustomData ?? x.Id, x => x.Color));

        #endregion
    }
}
