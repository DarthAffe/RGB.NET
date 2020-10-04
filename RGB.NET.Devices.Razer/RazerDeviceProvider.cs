// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using RGB.NET.Core;
using RGB.NET.Devices.Razer.Native;

namespace RGB.NET.Devices.Razer
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a device provider responsible for razer devices.
    /// </summary>
    public class RazerDeviceProvider : IRGBDeviceProvider
    {
        #region Properties & Fields

        private static RazerDeviceProvider _instance;
        /// <summary>
        /// Gets the singleton <see cref="RazerDeviceProvider"/> instance.
        /// </summary>
        public static RazerDeviceProvider Instance => _instance ?? new RazerDeviceProvider();

        /// <summary>
        /// Gets a modifiable list of paths used to find the native SDK-dlls for x86 applications.
        /// The first match will be used.
        /// </summary>
        public static List<string> PossibleX86NativePaths { get; } = new List<string> { "x86/RzChromaSDK.dll" };

        /// <summary>
        /// Gets a modifiable list of paths used to find the native SDK-dlls for x64 applications.
        /// The first match will be used.
        /// </summary>
        public static List<string> PossibleX64NativePaths { get; } = new List<string> { "x64/RzChromaSDK.dll", "x64/RzChromaSDK64.dll" };

        /// <inheritdoc />
        /// <summary>
        /// Indicates if the SDK is initialized and ready to use.
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// Gets the loaded architecture (x64/x86).
        /// </summary>
        public string LoadedArchitecture => _RazerSDK.LoadedArchitecture;

        /// <inheritdoc />
        /// <summary>
        /// Gets whether the application has exclusive access to the SDK or not.
        /// </summary>
        public bool HasExclusiveAccess => false;

        /// <inheritdoc />
        public IEnumerable<IRGBDevice> Devices { get; private set; }

        /// <summary>
        /// Gets or sets a function to get the culture for a specific device.
        /// </summary>
        public Func<CultureInfo> GetCulture { get; set; } = CultureHelper.GetCurrentCulture;

        /// <summary>
        /// Forces to load the devices represented by the emulator even if they aren't reported to exist.
        /// </summary>
        public bool LoadEmulatorDevices { get; set; } = false;

        /// <summary>
        /// The <see cref="DeviceUpdateTrigger"/> used to trigger the updates for razer devices. 
        /// </summary>
        public DeviceUpdateTrigger UpdateTrigger { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RazerDeviceProvider"/> class.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if this constructor is called even if there is already an instance of this class.</exception>
        public RazerDeviceProvider()
        {
            if (_instance != null) throw new InvalidOperationException($"There can be only one instance of type {nameof(RazerDeviceProvider)}");
            _instance = this;

            UpdateTrigger = new DeviceUpdateTrigger();
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public bool Initialize(RGBDeviceType loadFilter = RGBDeviceType.All, bool exclusiveAccessIfPossible = false, bool throwExceptions = false)
        {
            if (IsInitialized)
                TryUnInit();

            IsInitialized = false;

            try
            {
                UpdateTrigger?.Stop();

                _RazerSDK.Reload();

                RazerError error;
                if (((error = _RazerSDK.Init()) != RazerError.Success)
                    && Enum.IsDefined(typeof(RazerError), error)) //HACK DarthAffe 08.02.2018: The x86-SDK seems to have a problem here ...
                    ThrowRazerError(error);

                IList<IRGBDevice> devices = new List<IRGBDevice>();

                if (loadFilter.HasFlag(RGBDeviceType.Keyboard))
                    foreach ((Guid guid, string model) in Razer.Devices.KEYBOARDS)
                        try
                        {
                            if (((_RazerSDK.QueryDevice(guid, out _DeviceInfo deviceInfo) != RazerError.Success) || (deviceInfo.Connected < 1))
                                && (!LoadEmulatorDevices || (Razer.Devices.KEYBOARDS.FirstOrDefault().guid != guid))) continue;

                            RazerKeyboardRGBDevice device = new RazerKeyboardRGBDevice(new RazerKeyboardRGBDeviceInfo(guid, model, GetCulture()));
                            device.Initialize(UpdateTrigger);
                            devices.Add(device);
                        }
                        catch { if (throwExceptions) throw; }

                if (loadFilter.HasFlag(RGBDeviceType.Mouse))
                    foreach ((Guid guid, string model) in Razer.Devices.MICE)
                        try
                        {
                            if (((_RazerSDK.QueryDevice(guid, out _DeviceInfo deviceInfo) != RazerError.Success) || (deviceInfo.Connected < 1))
                                && (!LoadEmulatorDevices || (Razer.Devices.MICE.FirstOrDefault().guid != guid))) continue;

                            RazerMouseRGBDevice device = new RazerMouseRGBDevice(new RazerMouseRGBDeviceInfo(guid, model));
                            device.Initialize(UpdateTrigger);
                            devices.Add(device);
                        }
                        catch { if (throwExceptions) throw; }

                if (loadFilter.HasFlag(RGBDeviceType.Headset))
                    foreach ((Guid guid, string model) in Razer.Devices.HEADSETS)
                        try
                        {
                            if (((_RazerSDK.QueryDevice(guid, out _DeviceInfo deviceInfo) != RazerError.Success) || (deviceInfo.Connected < 1))
                                && (!LoadEmulatorDevices || (Razer.Devices.HEADSETS.FirstOrDefault().guid != guid))) continue;

                            RazerHeadsetRGBDevice device = new RazerHeadsetRGBDevice(new RazerHeadsetRGBDeviceInfo(guid, model));
                            device.Initialize(UpdateTrigger);
                            devices.Add(device);
                        }
                        catch { if (throwExceptions) throw; }

                if (loadFilter.HasFlag(RGBDeviceType.Mousepad))
                    foreach ((Guid guid, string model) in Razer.Devices.MOUSEMATS)
                        try
                        {
                            if (((_RazerSDK.QueryDevice(guid, out _DeviceInfo deviceInfo) != RazerError.Success) || (deviceInfo.Connected < 1))
                                && (!LoadEmulatorDevices || (Razer.Devices.MOUSEMATS.FirstOrDefault().guid != guid))) continue;

                            RazerMousepadRGBDevice device = new RazerMousepadRGBDevice(new RazerMousepadRGBDeviceInfo(guid, model));
                            device.Initialize(UpdateTrigger);
                            devices.Add(device);
                        }
                        catch { if (throwExceptions) throw; }

                if (loadFilter.HasFlag(RGBDeviceType.Keypad))
                    foreach ((Guid guid, string model) in Razer.Devices.KEYPADS)
                        try
                        {
                            if (((_RazerSDK.QueryDevice(guid, out _DeviceInfo deviceInfo) != RazerError.Success) || (deviceInfo.Connected < 1))
                                && (!LoadEmulatorDevices || (Razer.Devices.KEYPADS.FirstOrDefault().guid != guid))) continue;

                            RazerKeypadRGBDevice device = new RazerKeypadRGBDevice(new RazerKeypadRGBDeviceInfo(guid, model));
                            device.Initialize(UpdateTrigger);
                            devices.Add(device);
                        }
                        catch { if (throwExceptions) throw; }

                if (loadFilter.HasFlag(RGBDeviceType.Keyboard))
                    foreach ((Guid guid, string model) in Razer.Devices.CHROMALINKS)
                        try
                        {
                            if (((_RazerSDK.QueryDevice(guid, out _DeviceInfo deviceInfo) != RazerError.Success) || (deviceInfo.Connected < 1))
                                && (!LoadEmulatorDevices || (Razer.Devices.CHROMALINKS.FirstOrDefault().guid != guid))) continue;

                            RazerChromaLinkRGBDevice device = new RazerChromaLinkRGBDevice(new RazerChromaLinkRGBDeviceInfo(guid, model));
                            device.Initialize(UpdateTrigger);
                            devices.Add(device);
                        }
                        catch { if (throwExceptions) throw; }

                UpdateTrigger?.Start();
                Devices = new ReadOnlyCollection<IRGBDevice>(devices);
                IsInitialized = true;
            }
            catch
            {
                TryUnInit();
                if (throwExceptions) throw;
                return false;
            }

            return true;
        }

        /// <inheritdoc />
        public void ResetDevices()
        {
            foreach (IRGBDevice device in Devices)
                ((IRazerRGBDevice)device).Reset();
        }

        private void ThrowRazerError(RazerError errorCode) => throw new RazerException(errorCode);

        private void TryUnInit()
        {
            try { _RazerSDK.UnInit(); }
            catch { /* We tried our best */ }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            try { UpdateTrigger?.Dispose(); }
            catch { /* at least we tried */ }

            TryUnInit();

            // DarthAffe 03.03.2020: Fails with an access-violation - verify if an unload is already triggered by uninit
            //try { _RazerSDK.UnloadRazerSDK(); }
            //catch { /* at least we tried */ }
        }

        #endregion
    }
}
