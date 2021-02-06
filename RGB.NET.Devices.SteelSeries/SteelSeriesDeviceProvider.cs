using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RGB.NET.Core;
using RGB.NET.Devices.SteelSeries.API;
using RGB.NET.Devices.SteelSeries.HID;

namespace RGB.NET.Devices.SteelSeries
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a device provider responsible for SteelSeries- devices.
    /// </summary>
    public class SteelSeriesDeviceProvider : IRGBDeviceProvider
    {
        #region Properties & Fields

        private static SteelSeriesDeviceProvider? _instance;
        /// <summary>
        /// Gets the singleton <see cref="SteelSeriesDeviceProvider"/> instance.
        /// </summary>
        public static SteelSeriesDeviceProvider Instance => _instance ?? new SteelSeriesDeviceProvider();

        /// <inheritdoc />
        /// <summary>
        /// Indicates if the SDK is initialized and ready to use.
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <inheritdoc />
        public IEnumerable<IRGBDevice> Devices { get; private set; } = Enumerable.Empty<IRGBDevice>();

        /// <summary>
        /// The <see cref="SteelSeriesDeviceUpdateTrigger"/> used to trigger the updates for SteelSeries devices. 
        /// </summary>
        public SteelSeriesDeviceUpdateTrigger UpdateTrigger { get; }

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

            UpdateTrigger = new SteelSeriesDeviceUpdateTrigger();
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public bool Initialize(RGBDeviceType loadFilter = RGBDeviceType.All, bool throwExceptions = false)
        {
            try
            {
                IsInitialized = false;

                UpdateTrigger.Stop();

                if (!SteelSeriesSDK.IsInitialized)
                    SteelSeriesSDK.Initialize();

                IList<IRGBDevice> devices = new List<IRGBDevice>();
                DeviceChecker.LoadDeviceList(loadFilter);

                try
                {
                    foreach ((string model, RGBDeviceType deviceType, int _, SteelSeriesDeviceType steelSeriesDeviceType, Dictionary<LedId, SteelSeriesLedId> ledMapping) in DeviceChecker.ConnectedDevices)
                    {
                        ISteelSeriesRGBDevice device = new SteelSeriesRGBDevice(new SteelSeriesRGBDeviceInfo(deviceType, model, steelSeriesDeviceType));
                        string apiName = steelSeriesDeviceType.GetAPIName() ?? throw new RGBDeviceException($"Missing API-name for device {model}");
                        SteelSeriesDeviceUpdateQueue updateQueue = new(UpdateTrigger, apiName);
                        device.Initialize(updateQueue, ledMapping);
                        devices.Add(device);
                    }
                }
                catch { if (throwExceptions) throw; }

                UpdateTrigger.Start();

                Devices = new ReadOnlyCollection<IRGBDevice>(devices);
                IsInitialized = true;
            }
            catch
            {
                IsInitialized = false;
                if (throwExceptions) throw;
                return false;
            }

            return true;
        }
        
        /// <inheritdoc />
        public void Dispose()
        {
            try { UpdateTrigger.Dispose(); }
            catch { /* at least we tried */ }

            foreach (IRGBDevice device in Devices)
                try { device.Dispose(); }
                catch { /* at least we tried */ }
            Devices = Enumerable.Empty<IRGBDevice>();

            try { SteelSeriesSDK.Dispose(); }
            catch { /* shit happens */ }
        }

        #endregion
    }
}
