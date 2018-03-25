using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace RGB.NET.Core
{
    public partial class RGBSurface
    {
        #region Properties & Fields

        private CancellationTokenSource _updateTokenSource;
        private CancellationToken _updateToken;
        private Task _updateTask;
        private Stopwatch _sleepCounter;

        // ReSharper disable MemberCanBePrivate.Global

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

        private UpdateMode _updateMode = UpdateMode.Continuous;
        /// <summary>
        /// Gets or sets the update-mode.
        /// </summary>
        public UpdateMode UpdateMode
        {
            get => _updateMode;
            set
            {
                if (SetProperty(ref _updateMode, value))
                    CheckUpdateLoop();
            }
        }

        // ReSharper restore MemberCanBePrivate.Global
        #endregion

        #region Methods

        /// <summary>
        /// Checks if automatic updates should occur and starts/stops the update-loop if needed.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the requested update-mode is not available.</exception>
        private async void CheckUpdateLoop()
        {
            bool shouldRun;
            switch (UpdateMode)
            {
                case UpdateMode.Manual:
                    shouldRun = false;
                    break;
                case UpdateMode.Continuous:
                    shouldRun = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (shouldRun && (_updateTask == null)) // Start task
            {
                _updateTokenSource?.Dispose();
                _updateTokenSource = new CancellationTokenSource();
                _updateTask = Task.Factory.StartNew(UpdateLoop, (_updateToken = _updateTokenSource.Token), TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }
            else if (!shouldRun && (_updateTask != null)) // Stop task
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

                Update();

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
