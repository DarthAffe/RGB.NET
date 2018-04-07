using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace RGB.NET.Core
{
    public class TimerUpdateTrigger : AbstractUpdateTrigger
    {
        #region Properties & Fields

        private CancellationTokenSource _updateTokenSource;
        private CancellationToken _updateToken;
        private Task _updateTask;
        private Stopwatch _sleepCounter;

        private double _updateFrequency = 1.0 / 30.0;
        /// <summary>
        /// Gets or sets the update-frequency in seconds. (Calculate by using '1.0 / updates per second')
        /// </summary>
        public double UpdateFrequency
        {
            get => _updateFrequency;
            set => SetProperty(ref _updateFrequency, value);
        }

        /// <summary>
        /// Gets the time it took the last update-loop cycle to run.
        /// </summary>
        public double LastUpdateTime { get; private set; }

        #endregion

        #region Constructors

        public TimerUpdateTrigger(bool autostart = true)
        {
            _sleepCounter = new Stopwatch();

            if (autostart)
                Start();
        }

        #endregion

        #region Methods

        public void Start()
        {
            if (_updateTask == null)
            {
                _updateTokenSource?.Dispose();
                _updateTokenSource = new CancellationTokenSource();
                _updateTask = Task.Factory.StartNew(UpdateLoop, (_updateToken = _updateTokenSource.Token), TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }
        }

        public async void Stop()
        {
            if (_updateTask != null)
            {
                _updateTokenSource.Cancel();
                await _updateTask;
                _updateTask.Dispose();
                _updateTask = null;
            }
        }

        private void UpdateLoop()
        {
            while (!_updateToken.IsCancellationRequested)
            {
                _sleepCounter.Restart();

                OnUpdate();

                _sleepCounter.Stop();
                LastUpdateTime = _sleepCounter.Elapsed.TotalSeconds;
                int sleep = (int)((UpdateFrequency * 1000.0) - _sleepCounter.ElapsedMilliseconds);
                if (sleep > 0)
                    Thread.Sleep(sleep);
            }
        }

        #endregion
    }
}
