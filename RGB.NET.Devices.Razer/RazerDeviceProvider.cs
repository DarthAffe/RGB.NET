// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private static RazerDeviceProvider? _instance;
        /// <summary>
        /// Gets the singleton <see cref="RazerDeviceProvider"/> instance.
        /// </summary>
        public static RazerDeviceProvider Instance => _instance ?? new RazerDeviceProvider();

        /// <summary>
        /// Gets a modifiable list of paths used to find the native SDK-dlls for x86 applications.
        /// The first match will be used.
        /// </summary>
        public static List<string> PossibleX86NativePaths { get; } = new() { @"%systemroot%\SysWOW64\RzChromaSDK.dll" };

        /// <summary>
        /// Gets a modifiable list of paths used to find the native SDK-dlls for x64 applications.
        /// The first match will be used.
        /// </summary>
        public static List<string> PossibleX64NativePaths { get; } = new() { @"%systemroot%\System32\RzChromaSDK.dll", @"%systemroot%\System32\RzChromaSDK64.dll" };

        /// <inheritdoc />
        /// <summary>
        /// Indicates if the SDK is initialized and ready to use.
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <inheritdoc />
        public IEnumerable<IRGBDevice> Devices { get; private set; } = Enumerable.Empty<IRGBDevice>();

        /// <summary>
        /// Forces to load the devices represented by the emulator even if they aren't reported to exist.
        /// </summary>
        public bool LoadEmulatorDevices { get; set; } = false;

        /// <summary>
        /// The <see cref="DeviceUpdateTrigger"/> used to trigger the updates for razer devices. 
        /// </summary>
        public DeviceUpdateTrigger UpdateTrigger { get; }

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
        public bool Initialize(RGBDeviceType loadFilter = RGBDeviceType.All, bool throwExceptions = false)
        {
            if (IsInitialized)
                TryUnInit();

            IsInitialized = false;

            try
            {
                UpdateTrigger.Stop();

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

                            RazerKeyboardRGBDevice device = new(new RazerKeyboardRGBDeviceInfo(guid, model));
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

                            RazerMouseRGBDevice device = new(new RazerMouseRGBDeviceInfo(guid, model));
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

                            RazerHeadsetRGBDevice device = new(new RazerHeadsetRGBDeviceInfo(guid, model));
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

                            RazerMousepadRGBDevice device = new(new RazerMousepadRGBDeviceInfo(guid, model));
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

                            RazerKeypadRGBDevice device = new(new RazerKeypadRGBDeviceInfo(guid, model));
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

                            RazerChromaLinkRGBDevice device = new(new RazerChromaLinkRGBDeviceInfo(guid, model));
                            device.Initialize(UpdateTrigger);
                            devices.Add(device);
                        }
                        catch { if (throwExceptions) throw; }

                UpdateTrigger.Start();
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
        
        private void ThrowRazerError(RazerError errorCode) => throw new RazerException(errorCode);

        private void TryUnInit()
        {
            try { _RazerSDK.UnInit(); }
            catch { /* We tried our best */ }
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

            TryUnInit();

            // DarthAffe 03.03.2020: Fails with an access-violation - verify if an unload is already triggered by uninit
            //try { _RazerSDK.UnloadRazerSDK(); }
            //catch { /* at least we tried */ }
        }

        #endregion
    }
}
