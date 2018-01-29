// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RGB.NET.Core;
using Sanford.Multimedia.Midi;

namespace RGB.NET.Devices.Novation
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a device provider responsible for Novation  devices.
    /// </summary>
    public class NovationDeviceProvider : IRGBDeviceProvider
    {
        #region Properties & Fields

        private static NovationDeviceProvider _instance;
        /// <summary>
        /// Gets the singleton <see cref="NovationDeviceProvider"/> instance.
        /// </summary>
        public static NovationDeviceProvider Instance => _instance ?? new NovationDeviceProvider();

        /// <inheritdoc />
        /// <summary>
        /// Indicates if the SDK is initialized and ready to use.
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <inheritdoc />
        /// <summary>
        /// Gets whether the application has exclusive access to the SDK or not.
        /// </summary>
        public bool HasExclusiveAccess => false;

        /// <inheritdoc />
        public IEnumerable<IRGBDevice> Devices { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NovationDeviceProvider"/> class.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if this constructor is called even if there is already an instance of this class.</exception>
        private NovationDeviceProvider()
        {
            if (_instance != null) throw new InvalidOperationException($"There can be only one instance of type {nameof(NovationDeviceProvider)}");
            _instance = this;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public bool Initialize(RGBDeviceType loadFilter = RGBDeviceType.All, bool exclusiveAccessIfPossible = false, bool throwExceptions = false)
        {
            IsInitialized = false;

            try
            {
                IList<IRGBDevice> devices = new List<IRGBDevice>();

                if (loadFilter.HasFlag(RGBDeviceType.LedMatrix))
                    for (int index = 0; index < OutputDeviceBase.DeviceCount; index++)
                    {
                        try
                        {
                            MidiOutCaps outCaps = OutputDeviceBase.GetDeviceCapabilities(index);
                            if (outCaps.name == null) continue;

                            NovationDevices? deviceId = (NovationDevices?)Enum.GetValues(typeof(NovationDevices))
                                                                              .Cast<Enum>()
                                                                              .FirstOrDefault(x => string.Equals(x.GetDeviceId(), outCaps.name, StringComparison.OrdinalIgnoreCase));

                            if (deviceId == null) continue;

                            INovationRGBDevice device = new NovationLaunchpadRGBDevice(new NovationLaunchpadRGBDeviceInfo(outCaps.name, index, deviceId.GetColorCapability()));
                            device.Initialize();
                            devices.Add(device);
                        }
                        catch { if (throwExceptions) throw; }
                    }

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
        public void ResetDevices()
        {
            foreach (IRGBDevice device in Devices)
            {
                NovationLaunchpadRGBDevice novationDevice = device as NovationLaunchpadRGBDevice;
                novationDevice?.Reset();
            }
        }

        /// <inheritdoc />
        public void Dispose()
        { }

        #endregion
    }
}
