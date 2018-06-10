// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RGB.NET.Core;
using RGB.NET.Devices.SoIP.Client;
using RGB.NET.Devices.SoIP.Generic;
using RGB.NET.Devices.SoIP.Server;

namespace RGB.NET.Devices.SoIP
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a device provider responsible for debug devices.
    /// </summary>
    public class SoIPDeviceProvider : IRGBDeviceProvider
    {
        #region Properties & Fields

        private static SoIPDeviceProvider _instance;
        /// <summary>
        /// Gets the singleton <see cref="SoIPDeviceProvider"/> instance.
        /// </summary>
        public static SoIPDeviceProvider Instance => _instance ?? new SoIPDeviceProvider();

        /// <inheritdoc />
        public bool IsInitialized { get; private set; }

        /// <inheritdoc />
        public IEnumerable<IRGBDevice> Devices { get; private set; }

        /// <inheritdoc />
        public bool HasExclusiveAccess => false;

        /// <summary>
        /// Gets a list of all defined device-definitions.
        /// </summary>
        public List<ISoIPDeviceDefinition> DeviceDefinitions { get; } = new List<ISoIPDeviceDefinition>();

        /// <summary>
        /// The <see cref="DeviceUpdateTrigger"/> used to trigger the updates for dmx devices. 
        /// </summary>
        public DeviceUpdateTrigger UpdateTrigger { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SoIPDeviceProvider"/> class.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if this constructor is called even if there is already an instance of this class.</exception>
        public SoIPDeviceProvider()
        {
            if (_instance != null) throw new InvalidOperationException($"There can be only one instance of type {nameof(SoIPDeviceProvider)}");
            _instance = this;
            
            UpdateTrigger = new DeviceUpdateTrigger();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the given <see cref="ISoIPDeviceDefinition" /> to this device-provider.
        /// </summary>
        /// <param name="deviceDefinition">The <see cref="ISoIPDeviceDefinition"/> to add.</param>
        public void AddDeviceDefinition(ISoIPDeviceDefinition deviceDefinition) => DeviceDefinitions.Add(deviceDefinition);

        /// <inheritdoc />
        public bool Initialize(RGBDeviceType loadFilter = RGBDeviceType.Unknown, bool exclusiveAccessIfPossible = false, bool throwExceptions = false)
        {

            IsInitialized = false;

            try
            {
                UpdateTrigger.Stop();

                IList<IRGBDevice> devices = new List<IRGBDevice>();

                foreach (ISoIPDeviceDefinition deviceDefinition in DeviceDefinitions)
                {
                    try
                    {
                        ISoIPRGBDevice device = null;

                        switch (deviceDefinition)
                        {
                            case SoIPServerDeviceDefinition serverDeviceDefinition:
                                if (serverDeviceDefinition.Leds.Count > 0)
                                    device = new SoIPServerRGBDevice(new SoIPServerRGBDeviceInfo(serverDeviceDefinition), serverDeviceDefinition.Leds);
                                break;

                            case SoIPClientDeviceDefinition clientDeviceDefinition:
                                device = new SoIPClientRGBDevice(new SoIPClientRGBDeviceInfo(clientDeviceDefinition));
                                break;
                        }

                        if (device != null)
                        {
                            device.Initialize(UpdateTrigger);
                            devices.Add(device);
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
            foreach (IRGBDevice device in Devices)
                device.Dispose();
        }

        #endregion
    }
}
