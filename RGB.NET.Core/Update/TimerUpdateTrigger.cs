// ReSharper disable MemberCanBePrivate.Global

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace RGB.NET.Core
{
    /// <inheritdoc />
    /// <summary>
    /// Represents an <see cref="T:RGB.NET.Core.IUpdateTrigger" />
    /// </summary>
    public class TimerUpdateTrigger : AbstractUpdateTrigger
    {
        #region Properties & Fields

        private object _lock = new object();

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

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerUpdateTrigger"/> class.
        /// </summary>
        /// <param name="autostart">A value indicating if the trigger should automatically <see cref="Start"/> right after construction.</param>
        public TimerUpdateTrigger(bool autostart = true)
        {
            _sleepCounter = new Stopwatch();

            if (autostart)
                Start();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Starts the trigger if needed, causing it to performing updates.
        /// </summary>
        public void Start()
        {
            lock (_lock)
            {
                if (_updateTask == null)
                {
                    _updateTokenSource?.Dispose();
                    _updateTokenSource = new CancellationTokenSource();
                    _updateTask = Task.Factory.StartNew(UpdateLoop, (_updateToken = _updateTokenSource.Token), TaskCreationOptions.LongRunning, TaskScheduler.Default);
                }
            }
        }

        /// <summary>
        /// Stops the trigger if running, causing it to stop performing updates.
        /// </summary>
        public void Stop()
        {
            lock (_lock)
            {
                if (_updateTask != null)
                {
                    _updateTokenSource.Cancel();
                    // ReSharper disable once MethodSupportsCancellation
                    _updateTask.Wait();
                    _updateTask.Dispose();
                    _updateTask = null;
                }
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
