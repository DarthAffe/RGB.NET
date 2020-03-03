// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RGB.NET.Core;
using RGB.NET.Devices.DMX.E131;

namespace RGB.NET.Devices.DMX
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a device provider responsible for DMX devices.
    /// </summary>
    public class DMXDeviceProvider : IRGBDeviceProvider
    {
        #region Properties & Fields

        private static DMXDeviceProvider _instance;
        /// <summary>
        /// Gets the singleton <see cref="DMXDeviceProvider"/> instance.
        /// </summary>
        public static DMXDeviceProvider Instance => _instance ?? new DMXDeviceProvider();

        /// <inheritdoc />
        public bool IsInitialized { get; private set; }

        /// <inheritdoc />
        public IEnumerable<IRGBDevice> Devices { get; private set; }

        /// <inheritdoc />
        public bool HasExclusiveAccess => false;

        /// <summary>
        /// Gets a list of all defined device-definitions.
        /// </summary>
        public List<IDMXDeviceDefinition> DeviceDefinitions { get; } = new List<IDMXDeviceDefinition>();

        /// <summary>
        /// The <see cref="DeviceUpdateTrigger"/> used to trigger the updates for dmx devices. 
        /// </summary>
        public DeviceUpdateTrigger UpdateTrigger { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DMXDeviceProvider"/> class.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if this constructor is called even if there is already an instance of this class.</exception>
        public DMXDeviceProvider()
        {
            if (_instance != null) throw new InvalidOperationException($"There can be only one instance of type {nameof(DMXDeviceProvider)}");
            _instance = this;

            UpdateTrigger = new DeviceUpdateTrigger();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the given <see cref="IDMXDeviceDefinition" /> to this device-provider.
        /// </summary>
        /// <param name="deviceDefinition">The <see cref="IDMXDeviceDefinition"/> to add.</param>
        public void AddDeviceDefinition(IDMXDeviceDefinition deviceDefinition) => DeviceDefinitions.Add(deviceDefinition);

        /// <inheritdoc />
        public bool Initialize(RGBDeviceType loadFilter = RGBDeviceType.All, bool exclusiveAccessIfPossible = false, bool throwExceptions = false)
        {
            IsInitialized = false;

            try
            {
                UpdateTrigger.Stop();

                IList<IRGBDevice> devices = new List<IRGBDevice>();

                foreach (IDMXDeviceDefinition dmxDeviceDefinition in DeviceDefinitions)
                {
                    try
                    {
                        if (dmxDeviceDefinition is E131DMXDeviceDefinition e131DMXDeviceDefinition)
                        {
                            if (e131DMXDeviceDefinition.Leds.Count > 0)
                            {
                                E131Device device = new E131Device(new E131DeviceInfo(e131DMXDeviceDefinition), e131DMXDeviceDefinition.Leds);
                                device.Initialize(UpdateTrigger);
                                devices.Add(device);
                            }
                        }
                    }
                    catch { if (throwExceptions) throw; }
                }

                UpdateTrigger.Start();

                Devices = new ReadOnlyCollection<IRGBDevice>(devices);
                IsInitialized = true;
            }
            catch
            {
                if (throwExceptions) throw;
                return false;
            }

            return true;
        }

        /// <inheritdoc />
        public void ResetDevices()
        { }

        /// <inheritdoc />
        public void Dispose()
        {
            try { UpdateTrigger?.Dispose(); }
            catch { /* at least we tried */ }
        }

        #endregion
    }
}
