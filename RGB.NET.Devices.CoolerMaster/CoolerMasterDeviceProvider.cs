// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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

        private static CoolerMasterDeviceProvider _instance;
        /// <summary>
        /// Gets the singleton <see cref="CoolerMasterDeviceProvider"/> instance.
        /// </summary>
        public static CoolerMasterDeviceProvider Instance => _instance ?? new CoolerMasterDeviceProvider();

        /// <summary>
        /// Gets a modifiable list of paths used to find the native SDK-dlls for x86 applications.
        /// The first match will be used.
        /// </summary>
        public static List<string> PossibleX86NativePaths { get; } = new List<string> { "x86/CMSDK.dll" };

        /// <summary>
        /// Gets a modifiable list of paths used to find the native SDK-dlls for x64 applications.
        /// The first match will be used.
        /// </summary>
        public static List<string> PossibleX64NativePaths { get; } = new List<string> { "x64/CMSDK.dll" };

        /// <inheritdoc />
        /// <summary>
        /// Indicates if the SDK is initialized and ready to use.
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// Gets the loaded architecture (x64/x86).
        /// </summary>
        public string LoadedArchitecture => _CoolerMasterSDK.LoadedArchitecture;

        /// <inheritdoc />
        /// <summary>
        /// Gets whether the application has exclusive access to the SDK or not.
        /// </summary>
        public bool HasExclusiveAccess { get; private set; }

        /// <inheritdoc />
        public IEnumerable<IRGBDevice> Devices { get; private set; }

        /// <summary>
        /// Gets or sets a function to get the culture for a specific device.
        /// </summary>
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
        public Func<CultureInfo> GetCulture { get; set; } = CultureHelper.GetCurrentCulture;

        public UpdateTrigger UpdateTrigger { get; private set; }

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

            UpdateTrigger = new UpdateTrigger();
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public bool Initialize(RGBDeviceType loadFilter = RGBDeviceType.All, bool exclusiveAccessIfPossible = false, bool throwExceptions = false)
        {
            IsInitialized = false;

            try
            {
                UpdateTrigger?.Stop();

                _CoolerMasterSDK.Reload();
                if (_CoolerMasterSDK.GetSDKVersion() <= 0) return false;

                IList<IRGBDevice> devices = new List<IRGBDevice>();

                foreach (CoolerMasterDevicesIndexes index in Enum.GetValues(typeof(CoolerMasterDevicesIndexes)))
                {

                    try
                    {
                        _CoolerMasterSDK.SetControlDevice(index);
                        if (_CoolerMasterSDK.IsDevicePlugged())
                        {
                            RGBDeviceType deviceType = index.GetDeviceType();
                            if (!loadFilter.HasFlag(deviceType)) continue;

                            ICoolerMasterRGBDevice device;
                            switch (deviceType)
                            {
                                case RGBDeviceType.Keyboard:
                                    CoolerMasterPhysicalKeyboardLayout physicalLayout = _CoolerMasterSDK.GetDeviceLayout();
                                    device = new CoolerMasterKeyboardRGBDevice(new CoolerMasterKeyboardRGBDeviceInfo(index, physicalLayout, GetCulture()));
                                    break;
                                default:
                                    if (throwExceptions)
                                        throw new RGBDeviceException("Unknown Device-Type");
                                    else
                                        continue;
                            }

                            _CoolerMasterSDK.EnableLedControl(true);

                            device.Initialize(UpdateTrigger);
                            devices.Add(device);
                        }
                    }
                    catch { if (throwExceptions) throw; }
                }

                UpdateTrigger?.Start();

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
            if (IsInitialized)
                foreach (IRGBDevice device in Devices)
                {
                    try
                    {
                        CoolerMasterRGBDeviceInfo deviceInfo = (CoolerMasterRGBDeviceInfo)device.DeviceInfo;
                        _CoolerMasterSDK.SetControlDevice(deviceInfo.DeviceIndex);
                        _CoolerMasterSDK.EnableLedControl(false);
                        _CoolerMasterSDK.EnableLedControl(true);
                    }
                    catch {/* shit happens */}
                }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (IsInitialized)
                foreach (IRGBDevice device in Devices)
                {
                    try
                    {
                        CoolerMasterRGBDeviceInfo deviceInfo = (CoolerMasterRGBDeviceInfo)device.DeviceInfo;
                        _CoolerMasterSDK.SetControlDevice(deviceInfo.DeviceIndex);
                        _CoolerMasterSDK.EnableLedControl(false);
                    }
                    catch {/* shit happens */}
                }
        }

        #endregion
    }
}
