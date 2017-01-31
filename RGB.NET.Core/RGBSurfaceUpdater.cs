using System;
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

        private double _updateFrequency = 1.0 / 30.0;
        /// <summary>
        /// Gets or sets the update-frequency in seconds. (Calculate by using '1.0 / updates per second')
        /// </summary>
        public double UpdateFrequency
        {
            get { return _updateFrequency; }
            set { SetProperty(ref _updateFrequency, value); }
        }

        private UpdateMode _updateMode = UpdateMode.Manual;
        /// <summary>
        /// Gets or sets the update-mode.
        /// </summary>
        public UpdateMode UpdateMode
        {
            get { return _updateMode; }
            set
            {
                if (SetProperty(ref _updateMode, value))
                    CheckUpdateLoop();
            }
        }

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
                _updateTask = Task.Factory.StartNew(UpdateLoop, (_updateToken = _updateTokenSource.Token));
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
                long preUpdateTicks = DateTime.Now.Ticks;

                Update();

                int sleep = (int)((UpdateFrequency * 1000.0) - ((DateTime.Now.Ticks - preUpdateTicks) / 10000.0));
                if (sleep > 0)
                    Thread.Sleep(sleep);
            }
        }

        #endregion
    }
}
