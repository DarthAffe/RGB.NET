using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace RGB.NET.Core
{
    public class DeviceUpdateTrigger : AbstractUpdateTrigger, IDeviceUpdateTrigger
    {
        #region Properties & Fields

        public int Timeout { get; set; } = 100;

        public double UpdateFrequency { get; private set; }

        private double _maxUpdateRate;
        public double MaxUpdateRate
        {
            get => _maxUpdateRate;
            set
            {
                _maxUpdateRate = value;
                UpdateUpdateFrequency();
            }
        }

        private double _updateRateHardLimit;
        public double UpdateRateHardLimit
        {
            get => _updateRateHardLimit;
            protected set
            {
                _updateRateHardLimit = value;
                UpdateUpdateFrequency();
            }
        }

        private AutoResetEvent _hasDataEvent = new AutoResetEvent(false);

        private bool _isRunning;
        private Task _updateTask;
        private CancellationTokenSource _updateTokenSource;
        private CancellationToken _updateToken;

        #endregion

        #region Constructors

        public DeviceUpdateTrigger()
        { }

        public DeviceUpdateTrigger(double updateRateHardLimit)
        {
            this._updateRateHardLimit = updateRateHardLimit;
        }

        #endregion

        #region Methods

        public void Start()
        {
            if (_isRunning) return;

            _isRunning = true;

            _updateTokenSource?.Dispose();
            _updateTokenSource = new CancellationTokenSource();
            _updateTask = Task.Factory.StartNew(UpdateLoop, (_updateToken = _updateTokenSource.Token), TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        public async void Stop()
        {
            if (!_isRunning) return;

            _isRunning = false;

            _updateTokenSource.Cancel();
            await _updateTask;
            _updateTask.Dispose();
            _updateTask = null;
        }

        private void UpdateLoop()
        {
            OnStartup();
            while (!_updateToken.IsCancellationRequested)
            {
                if (_hasDataEvent.WaitOne(Timeout))
                {
                    long preUpdateTicks = Stopwatch.GetTimestamp();

                    OnUpdate();

                    if (UpdateFrequency > 0)
                    {
                        double lastUpdateTime = ((Stopwatch.GetTimestamp() - preUpdateTicks) / 10000.0);
                        int sleep = (int)((UpdateFrequency * 1000.0) - lastUpdateTime);
                        if (sleep > 0)
                            Thread.Sleep(sleep);
                    }
                }
            }
        }

        /// <inheritdoc />
        public void TriggerHasData() => _hasDataEvent.Set();

        private void UpdateUpdateFrequency()
        {
            UpdateFrequency = MaxUpdateRate;
            if ((UpdateFrequency <= 0) || ((UpdateRateHardLimit > 0) && (UpdateRateHardLimit < UpdateFrequency)))
                UpdateFrequency = UpdateRateHardLimit;
        }

        #endregion
    }
}
