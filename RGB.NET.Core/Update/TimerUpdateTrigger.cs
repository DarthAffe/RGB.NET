// ReSharper disable MemberCanBePrivate.Global

using System;
using System.Threading;
using System.Threading.Tasks;

namespace RGB.NET.Core;

/// <inheritdoc />
/// <summary>
/// Represents an update trigger that triggers in a set interval.
/// </summary>
public sealed class TimerUpdateTrigger : AbstractUpdateTrigger
{
    #region Properties & Fields

    private readonly Lock _lock = new();

    private readonly CustomUpdateData? _customUpdateData;

    /// <summary>
    /// Gets or sets the update loop of this trigger.
    /// </summary>
    private Task? _updateTask;

    /// <summary>
    /// Gets or sets the cancellation token source used to create the cancellation token checked by the <see cref="_updateTask"/>.
    /// </summary>
    private CancellationTokenSource? _updateTokenSource;

    /// <summary>
    /// Gets or sets the cancellation token checked by the <see cref="_updateTask"/>.
    /// </summary>
    private CancellationToken _updateToken;

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

    /// <summary>
    /// Initializes a new instance of the <see cref="TimerUpdateTrigger"/> class.
    /// </summary>
    /// <param name="customUpdateData">The update-data passed on each update triggered.</param>
    /// <param name="autostart">A value indicating if the trigger should automatically <see cref="Start"/> right after construction.</param>
    public TimerUpdateTrigger(CustomUpdateData? customUpdateData, bool autostart = true)
    {
        this._customUpdateData = customUpdateData;

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
                _updateTokenSource?.Cancel();
                try
                {
                    // ReSharper disable once MethodSupportsCancellation
                    _updateTask.Wait();
                }
                catch (AggregateException)
                {
                    // ignored
                }
                finally
                {
                    _updateTask.Dispose();
                    _updateTask = null;
                }
            }
        }
    }

    private void UpdateLoop()
    {
        OnStartup();

        using (TimerHelper.RequestHighResolutionTimer())
            while (!_updateToken.IsCancellationRequested)
                LastUpdateTime = TimerHelper.Execute(TimerExecute, UpdateFrequency * 1000);
    }

    private void TimerExecute() => OnUpdate(_customUpdateData);

    /// <inheritdoc />
    public override void Dispose()
    {
        Stop();
    }

    #endregion
}