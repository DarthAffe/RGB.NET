// ReSharper disable MemberCanBePrivate.Global

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace RGB.NET.Core;

/// <inheritdoc />
/// <summary>
/// Represents an update trigger that is manully triggered by calling <see cref="TriggerUpdate"/>.
/// </summary>
public sealed class ManualUpdateTrigger : AbstractUpdateTrigger
{
    #region Properties & Fields

    private readonly AutoResetEvent _mutex = new(false);
    private Task? UpdateTask { get; set; }
    private CancellationTokenSource? UpdateTokenSource { get; set; }
    private CancellationToken UpdateToken { get; set; }

    private CustomUpdateData? _customUpdateData;

    /// <summary>
    /// Gets the time it took the last update-loop cycle to run.
    /// </summary>
    public override double LastUpdateTime { get; protected set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ManualUpdateTrigger"/> class.
    /// </summary>
    public ManualUpdateTrigger()
    {
        Start();
    }

    #endregion

    #region Methods

    /// <summary>
    /// Starts the trigger if needed, causing it to performing updates.
    /// </summary>
    public override void Start()
    {
        if (UpdateTask == null)
        {
            UpdateTokenSource?.Dispose();
            UpdateTokenSource = new CancellationTokenSource();
            UpdateTask = Task.Factory.StartNew(UpdateLoop, (UpdateToken = UpdateTokenSource.Token), TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }
    }

    /// <summary>
    /// Stops the trigger if running, causing it to stop performing updates.
    /// </summary>
    private void Stop()
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

    /// <summary>
    /// Triggers an update.
    /// </summary>
    public void TriggerUpdate(CustomUpdateData? updateData = null)
    {
        _customUpdateData = updateData;
        _mutex.Set();
    }

    private void UpdateLoop()
    {
        OnStartup();

        while (!UpdateToken.IsCancellationRequested)
        {
            if (_mutex.WaitOne(100))
            {
                long preUpdateTicks = Stopwatch.GetTimestamp();
                OnUpdate(_customUpdateData);
                LastUpdateTime = ((Stopwatch.GetTimestamp() - preUpdateTicks) / 10000.0);
            }
        }
    }

    /// <inheritdoc />
    public override void Dispose() => Stop();

    #endregion
}