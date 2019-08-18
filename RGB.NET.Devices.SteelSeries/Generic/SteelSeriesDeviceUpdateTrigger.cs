// ReSharper disable MemberCanBePrivate.Global

using System;
using System.Diagnostics;
using System.Threading;
using RGB.NET.Core;

namespace RGB.NET.Devices.SteelSeries
{
    /// <summary>
    /// Represents an update-trigger used to update SteelSeries devices
    /// </summary>
    public class SteelSeriesDeviceUpdateTrigger : DeviceUpdateTrigger
    {
        #region Constants

        private const long FLUSH_TIMER = 5 * 1000 * TimeSpan.TicksPerMillisecond; // flush the device every 5 seconds to prevent timeouts

        #endregion

        #region Properties & Fields

        private long _lastUpdateTimestamp;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SteelSeriesDeviceUpdateTrigger"/> class.
        /// </summary>
        /// <param name="updateRateHardLimit">The hard limit of the update rate of this trigger.</param>
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
                        _lastUpdateTimestamp = Stopwatch.GetTimestamp();
                        double lastUpdateTime = ((_lastUpdateTimestamp - preUpdateTicks) / (double)TimeSpan.TicksPerMillisecond);
                        int sleep = (int)((UpdateFrequency * 1000.0) - lastUpdateTime);
                        if (sleep > 0)
                            Thread.Sleep(sleep);
                    }
                }
                else if (((Stopwatch.GetTimestamp() - _lastUpdateTimestamp) > FLUSH_TIMER))
                    OnUpdate(new CustomUpdateData(("refresh", true)));
            }
        }

        #endregion
    }
}
