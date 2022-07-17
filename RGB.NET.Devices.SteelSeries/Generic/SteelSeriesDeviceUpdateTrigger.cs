// ReSharper disable MemberCanBePrivate.Global

using System.Diagnostics;
using System.Threading;
using RGB.NET.Core;

namespace RGB.NET.Devices.SteelSeries;

/// <summary>
/// Represents an update-trigger used to update SteelSeries devices
/// </summary>
public class SteelSeriesDeviceUpdateTrigger : DeviceUpdateTrigger
{
    #region Constants

    private static readonly long FLUSH_TIMER = 5 * 1000 * (long)(Stopwatch.Frequency / 1000.0); // flush the device every 5 seconds to prevent timeouts

    #endregion

    #region Properties & Fields

    private long _lastUpdateTimestamp;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="SteelSeriesDeviceUpdateTrigger"/> class.
    /// </summary>
    public SteelSeriesDeviceUpdateTrigger()
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SteelSeriesDeviceUpdateTrigger"/> class.
    /// </summary>
    /// <param name="updateRateHardLimit">The hard limit of the update rate of this trigger.</param>
    public SteelSeriesDeviceUpdateTrigger(double updateRateHardLimit)
        : base(updateRateHardLimit)
    { }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override void UpdateLoop()
    {
        OnStartup();

        while (!UpdateToken.IsCancellationRequested)
        {
            if (HasDataEvent.WaitOne(Timeout))
            {
                long preUpdateTicks = Stopwatch.GetTimestamp();

                OnUpdate();

                if (UpdateFrequency > 0)
                {
                    double lastUpdateTime = ((_lastUpdateTimestamp - preUpdateTicks) / (Stopwatch.Frequency / 1000.0));
                    int sleep = (int)((UpdateFrequency * 1000.0) - lastUpdateTime);
                    if (sleep > 0)
                        Thread.Sleep(sleep);
                }
            }
            else if ((_lastUpdateTimestamp > 0) && ((Stopwatch.GetTimestamp() - _lastUpdateTimestamp) > FLUSH_TIMER))
                OnUpdate(new CustomUpdateData(("refresh", true)));
        }
    }

    /// <inheritdoc />
    protected override void OnUpdate(CustomUpdateData? updateData = null)
    {
        base.OnUpdate(updateData);
        _lastUpdateTimestamp = Stopwatch.GetTimestamp();
    }

    #endregion
}