// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RGB.NET.Core;
using RGB.NET.Devices.CoolerMaster.Helper;
using RGB.NET.Devices.CoolerMaster.Native;

namespace RGB.NET.Devices.CoolerMaster
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a device provider responsible for Cooler Master devices.
    /// </summary>
    public class CoolerMasterDeviceProvider : IRGBDeviceProvider
    {
        #region Properties & Fields

        private static CoolerMasterDeviceProvider? _instance;
        /// <summary>
        /// Gets the singleton <see cref="CoolerMasterDeviceProvider"/> instance.
        /// </summary>
        public static CoolerMasterDeviceProvider Instance => _instance ?? new CoolerMasterDeviceProvider();

        /// <summary>
        /// Gets a modifiable list of paths used to find the native SDK-dlls for x86 applications.
        /// The first match will be used.
        /// </summary>
        public static List<string> PossibleX86NativePaths { get; } = new() { "x86/CMSDK.dll" };

        /// <summary>
        /// Gets a modifiable list of paths used to find the native SDK-dlls for x64 applications.
        /// The first match will be used.
        /// </summary>
        public static List<string> PossibleX64NativePaths { get; } = new() { "x64/CMSDK.dll" };

        /// <inheritdoc />
        /// <summary>
        /// Indicates if the SDK is initialized and ready to use.
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <inheritdoc />
        public IEnumerable<IRGBDevice> Devices { get; private set; } = Enumerable.Empty<IRGBDevice>();

        /// <summary>
        /// The <see cref="DeviceUpdateTrigger"/> used to trigger the updates for cooler master devices. 
        /// </summary>
        public DeviceUpdateTrigger UpdateTrigger { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CoolerMasterDeviceProvider"/> class.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if this constructor is called even if there is already an instance of this class.</exception>
        public CoolerMasterDeviceProvider()
        {
            if (_instance != null) throw new InvalidOperationException($"There can be only one instance of type {nameof(CoolerMasterDeviceProvider)}");
            _instance = this;

            UpdateTrigger = new DeviceUpdateTrigger();
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public bool Initialize(RGBDeviceType loadFilter = RGBDeviceType.All, bool throwExceptions = false)
        {
            IsInitialized = false;

            try
            {
                UpdateTrigger.Stop();

                _CoolerMasterSDK.Reload();
                if (_CoolerMasterSDK.GetSDKVersion() <= 0) return false;

                IList<IRGBDevice> devices = new List<IRGBDevice>();

                foreach (CoolerMasterDevicesIndexes index in Enum.GetValues(typeof(CoolerMasterDevicesIndexes)))
                {
                    try
                    {
                        RGBDeviceType deviceType = index.GetDeviceType();
                        if (deviceType == RGBDeviceType.None) continue;

                        if (_CoolerMasterSDK.IsDevicePlugged(index))
                        {
                            if (!loadFilter.HasFlag(deviceType)) continue;

                            ICoolerMasterRGBDevice device;
                            switch (deviceType)
                            {
                                case RGBDeviceType.Keyboard:
                                    CoolerMasterPhysicalKeyboardLayout physicalLayout = _CoolerMasterSDK.GetDeviceLayout(index);
                                    device = new CoolerMasterKeyboardRGBDevice(new CoolerMasterKeyboardRGBDeviceInfo(index, physicalLayout));
                                    break;

                                case RGBDeviceType.Mouse:
                                    device = new CoolerMasterMouseRGBDevice(new CoolerMasterMouseRGBDeviceInfo(index));
                                    break;

                                default:
                                    if (throwExceptions)
                                        throw new RGBDeviceException("Unknown Device-Type");
                                    else
                                        continue;
                            }

                            if (!_CoolerMasterSDK.EnableLedControl(true, index))
                                throw new RGBDeviceException("Failed to enable LED control for device " + index);

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
            catch { /* at least we tried */ }

            foreach (IRGBDevice device in Devices)
                try { device.Dispose(); }
                catch { /* at least we tried */ }
            Devices = Enumerable.Empty<IRGBDevice>();

            // DarthAffe 03.03.2020: Should be done but isn't possible due to an weird winodws-hook inside the sdk which corrupts the stack when unloading the dll
            //try { _CoolerMasterSDK.UnloadCMSDK(); }
            //catch { /* at least we tried */ }
        }

        #endregion
    }
}
