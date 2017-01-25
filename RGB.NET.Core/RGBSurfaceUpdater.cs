using System;
using System.Threading;
using System.Threading.Tasks;

namespace RGB.NET.Core
{
    public static partial class RGBSurface
    {
        #region Properties & Fields

        private static CancellationTokenSource _updateTokenSource;
        private static CancellationToken _updateToken;
        private static Task _updateTask;

        /// <summary>
        /// Gets or sets the update-frequency in seconds. (Calculate by using '1.0 / updates per second')
        /// </summary>
        public static double UpdateFrequency { get; set; } = 1.0 / 30.0;

        private static UpdateMode _updateMode = UpdateMode.Manual;
        /// <summary>
        /// Gets or sets the update-mode.
        /// </summary>
        public static UpdateMode UpdateMode
        {
            get { return _updateMode; }
            set
            {
                _updateMode = value;
                CheckUpdateLoop();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Checks if automatic updates should occur and starts/stops the update-loop if needed.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the requested update-mode is not available.</exception>
        private static async void CheckUpdateLoop()
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

        private static void UpdateLoop()
        {
            while (!_updateToken.IsCancellationRequested)
            {
                long preUpdateTicks = DateTime.Now.Ticks;

                foreach (IRGBDevice device in _devices)
                    device.Update();

                int sleep = (int)((UpdateFrequency * 1000.0) - ((DateTime.Now.Ticks - preUpdateTicks) / 10000.0));
                if (sleep > 0)
                    Thread.Sleep(sleep);
            }
        }

        #endregion
    }
}
