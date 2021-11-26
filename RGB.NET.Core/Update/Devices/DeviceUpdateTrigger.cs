// ReSharper disable MemberCanBePrivate.Global

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace RGB.NET.Core;

/// <summary>
/// Represents an update-trigger used to update devices with a maximum update-rate.
/// </summary>
public class DeviceUpdateTrigger : AbstractUpdateTrigger, IDeviceUpdateTrigger
{
    #region Properties & Fields

    /// <summary>
    /// Gets or sets the timeout used by the blocking wait for data availability.
    /// </summary>
    public int Timeout { get; set; } = 100;

    /// <summary>
    /// Gets the update frequency used by the trigger if not limited by data shortage.
    /// </summary>
    public double UpdateFrequency { get; private set; }

    private double _maxUpdateRate;
    /// <summary>
    /// Gets or sets the maximum update rate of this trigger (is overwriten if the <see cref="UpdateRateHardLimit"/> is smaller).
    /// &lt;= 0 removes the limit.
    /// </summary>
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
    /// <summary>
    /// Gets the hard limit of the update rate of this trigger. Updates will never perform faster then then this value if it's set.
    /// &lt;= 0 removes the limit.
    /// </summary>
    public double UpdateRateHardLimit
    {
        get => _updateRateHardLimit;
        protected set
        {
            _updateRateHardLimit = value;
            UpdateUpdateFrequency();
        }
    }

    /// <inheritdoc />
    public override double LastUpdateTime { get; protected set; }

    /// <summary>
    /// Gets or sets the event to trigger when new data is available (<see cref="TriggerHasData"/>).
    /// </summary>
    protected AutoResetEvent HasDataEvent { get; set; } = new(false);

    /// <summary>
    /// Gets or sets a bool indicating if the trigger is currently updating.
    /// </summary>
    protected bool IsRunning { get; set; }

    /// <summary>
    /// Gets or sets the update loop of this trigger.
    /// </summary>
    protected Task? UpdateTask { get; set; }

    /// <summary>
    /// Gets or sets the cancellation token source used to create the cancellation token checked by the <see cref="UpdateTask"/>.
    /// </summary>
    protected CancellationTokenSource? UpdateTokenSource { get; set; }

    /// <summary>
    /// Gets or sets the cancellation token checked by the <see cref="UpdateTask"/>.
    /// </summary>
    protected CancellationToken UpdateToken { get; set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="DeviceUpdateTrigger"/> class.
    /// </summary>
    public DeviceUpdateTrigger()
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="DeviceUpdateTrigger"/> class.
    /// </summary>
    /// <param name="updateRateHardLimit">The hard limit of the update rate of this trigger.</param>
    public DeviceUpdateTrigger(double updateRateHardLimit)
    {
        this.UpdateRateHardLimit = updateRateHardLimit;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Starts the trigger.
    /// </summary>
    public override void Start()
    {
        if (IsRunning) return;

        IsRunning = true;

        UpdateTokenSource?.Dispose();
        UpdateTokenSource = new CancellationTokenSource();
        UpdateTask = Task.Factory.StartNew(UpdateLoop, (UpdateToken = UpdateTokenSource.Token), TaskCreationOptions.LongRunning, TaskScheduler.Default);
    }

    /// <summary>
    /// Stops the trigger.
    /// </summary>
    public async void Stop()
    {
        if (!IsRunning) return;

        IsRunning = false;

        UpdateTokenSource?.Cancel();
        if (UpdateTask != null)
            await UpdateTask;

        UpdateTask?.Dispose();
        UpdateTask = null;
    }

    /// <summary>
    /// The update loop called by the <see cref="UpdateTask"/>.
    /// </summary>
    protected virtual void UpdateLoop()
    {
        OnStartup();

        while (!UpdateToken.IsCancellationRequested)
        {
            if (HasDataEvent.WaitOne(Timeout))
            {
                long preUpdateTicks = Stopwatch.GetTimestamp();

                OnUpdate();

                double lastUpdateTime = ((Stopwatch.GetTimestamp() - preUpdateTicks) / 10000.0);
                LastUpdateTime = lastUpdateTime;

                if (UpdateFrequency > 0)
                {
                    int sleep = (int)((UpdateFrequency * 1000.0) - lastUpdateTime);
                    if (sleep > 0)
                        Thread.Sleep(sleep);
                }
            }
        }
    }

    /// <inheritdoc />
    public void TriggerHasData() => HasDataEvent.Set();

    private void UpdateUpdateFrequency()
    {
        UpdateFrequency = MaxUpdateRate;
        if ((UpdateFrequency <= 0) || ((UpdateRateHardLimit > 0) && (UpdateRateHardLimit < UpdateFrequency)))
            UpdateFrequency = UpdateRateHardLimit;
    }

    /// <inheritdoc />
    public override void Dispose() => Stop();

    #endregion
}