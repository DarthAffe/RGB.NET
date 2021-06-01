// ReSharper disable MemberCanBePrivate.Global

using System;
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

        private readonly object _lock = new();

        protected Task? UpdateTask { get; set; }
        protected CancellationTokenSource? UpdateTokenSource { get; set; }
        protected CancellationToken UpdateToken { get; set; }

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
        public override double LastUpdateTime { get; protected set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerUpdateTrigger"/> class.
        /// </summary>
        /// <param name="autostart">A value indicating if the trigger should automatically <see cref="Start"/> right after construction.</param>
        public TimerUpdateTrigger(bool autostart = true)
        {
            if (autostart)
                // ReSharper disable once VirtualMemberCallInConstructor - HACK DarthAffe 01.06.2021: I've no idea how to correctly handle that case, for now just disable it 
                Start();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Starts the trigger if needed, causing it to performing updates.
        /// </summary>
        public override void Start()
        {
            lock (_lock)
            {
                if (UpdateTask == null)
                {
                    UpdateTokenSource?.Dispose();
                    UpdateTokenSource = new CancellationTokenSource();
                    UpdateTask = Task.Factory.StartNew(UpdateLoop, (UpdateToken = UpdateTokenSource.Token), TaskCreationOptions.LongRunning, TaskScheduler.Default);
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
                if (UpdateTask != null)
                {
                    UpdateTokenSource?.Cancel();
                    // ReSharper disable once MethodSupportsCancellation
                    UpdateTask.Wait();
                    UpdateTask.Dispose();
                    UpdateTask = null;
                }
            }
        }

        private void UpdateLoop()
        {
            OnStartup();

            while (!UpdateToken.IsCancellationRequested)
            {
                long preUpdateTicks = Stopwatch.GetTimestamp();

                OnUpdate();

                if (UpdateFrequency > 0)
                {
                    double lastUpdateTime = ((Stopwatch.GetTimestamp() - preUpdateTicks) / 10000.0);
                    LastUpdateTime = lastUpdateTime;
                    int sleep = (int)((UpdateFrequency * 1000.0) - lastUpdateTime);
                    if (sleep > 0)
                        Thread.Sleep(sleep);
                }
            }
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            Stop();
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
