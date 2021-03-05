using System;
using System.Collections.Generic;
using RGB.NET.Core;
using RGB.NET.Devices.SteelSeries.API;
using RGB.NET.Devices.SteelSeries.HID;

namespace RGB.NET.Devices.SteelSeries
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a device provider responsible for SteelSeries- devices.
    /// </summary>
    public class SteelSeriesDeviceProvider : AbstractRGBDeviceProvider
    {
        #region Properties & Fields

        private static SteelSeriesDeviceProvider? _instance;
        /// <summary>
        /// Gets the singleton <see cref="SteelSeriesDeviceProvider"/> instance.
        /// </summary>
        public static SteelSeriesDeviceProvider Instance => _instance ?? new SteelSeriesDeviceProvider();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SteelSeriesDeviceProvider"/> class.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if this constructor is called even if there is already an instance of this class.</exception>
        public SteelSeriesDeviceProvider()
        {
            if (_instance != null) throw new InvalidOperationException($"There can be only one instance of type {nameof(SteelSeriesDeviceProvider)}");
            _instance = this;
        }

        #endregion

        #region Methods

        protected override void InitializeSDK()
        {
            if (!SteelSeriesSDK.IsInitialized)
                SteelSeriesSDK.Initialize();
        }

        protected override IEnumerable<IRGBDevice> GetLoadedDevices(RGBDeviceType loadFilter)
        {
            DeviceChecker.LoadDeviceList(loadFilter);

            return base.GetLoadedDevices(loadFilter);
        }

        protected override IEnumerable<IRGBDevice> LoadDevices()
        {
            foreach ((string model, RGBDeviceType deviceType, int _, SteelSeriesDeviceType steelSeriesDeviceType, Dictionary<LedId, SteelSeriesLedId> ledMapping) in DeviceChecker.ConnectedDevices)
            {
                string? apiName = steelSeriesDeviceType.GetAPIName();
                if (apiName == null)
                    Throw(new RGBDeviceException($"Missing API-name for device {model}"));
                else
                    yield return new SteelSeriesRGBDevice(new SteelSeriesRGBDeviceInfo(deviceType, model, steelSeriesDeviceType), apiName, ledMapping, GetUpdateTrigger());
            }
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            base.Dispose();

            try { SteelSeriesSDK.Dispose(); }
            catch { /* shit happens */ }
        }

        #endregion
    }
}
