// ReSharper disable MemberCanBePrivate.Global

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RGB.NET.Core;

namespace RGB.NET.Devices.WS281X
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a device provider responsible for WS2812B- and WS2811-Led-devices.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class WS281XDeviceProvider : IRGBDeviceProvider
    {
        #region Properties & Fields

        private static WS281XDeviceProvider? _instance;
        /// <summary>
        /// Gets the singleton <see cref="WS281XDeviceProvider"/> instance.
        /// </summary>
        public static WS281XDeviceProvider Instance => _instance ?? new WS281XDeviceProvider();

        /// <inheritdoc />
        public bool IsInitialized { get; private set; }

        /// <inheritdoc />
        public IEnumerable<IRGBDevice> Devices { get; private set; } = Enumerable.Empty<IRGBDevice>();

        /// <summary>
        /// Gets a list of all defined device-definitions.
        /// </summary>
        // ReSharper disable once CollectionNeverUpdated.Global
        // ReSharper disable once ReturnTypeCanBeEnumerable.Global
        public List<IWS281XDeviceDefinition> DeviceDefinitions { get; } = new();

        /// <summary>
        /// The <see cref="DeviceUpdateTrigger"/> used to trigger the updates for corsair devices. 
        /// </summary>
        public DeviceUpdateTrigger UpdateTrigger { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WS281XDeviceProvider"/> class.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if this constructor is called even if there is already an instance of this class.</exception>
        public WS281XDeviceProvider()
        {
            if (_instance != null) throw new InvalidOperationException($"There can be only one instance of type {nameof(WS281XDeviceProvider)}");
            _instance = this;

            UpdateTrigger = new DeviceUpdateTrigger();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the given <see cref="IWS281XDeviceDefinition" /> to this device-provider.
        /// </summary>
        /// <param name="deviceDefinition">The <see cref="IWS281XDeviceDefinition"/> to add.</param>
        // ReSharper disable once UnusedMember.Global
        public void AddDeviceDefinition(IWS281XDeviceDefinition deviceDefinition) => DeviceDefinitions.Add(deviceDefinition);

        /// <inheritdoc />
        public bool Initialize(RGBDeviceType loadFilter = RGBDeviceType.Unknown, bool throwExceptions = false)
        {
            IsInitialized = false;

            try
            {
                UpdateTrigger.Stop();

                List<IRGBDevice> devices = new();
                foreach (IWS281XDeviceDefinition deviceDefinition in DeviceDefinitions)
                {
                    try
                    {
                        devices.AddRange(deviceDefinition.CreateDevices(UpdateTrigger));
                    }
                    catch { if (throwExceptions) throw; }
                }
                UpdateTrigger.Start();

                Devices = new ReadOnlyCollection<IRGBDevice>(devices);
                IsInitialized = true;
            }
            catch
            {
                if (throwExceptions)
                    throw;
                return false;
            }

            return true;
        }
        
        /// <inheritdoc />
        public void Dispose()
        {
            try { UpdateTrigger.Dispose(); }
            catch { /* at least we tried */}

            foreach (IRGBDevice device in Devices)
                try { device.Dispose(); }
                catch { /* at least we tried */ }
            Devices = Enumerable.Empty<IRGBDevice>();

            DeviceDefinitions.Clear();
        }

        #endregion
    }
}
