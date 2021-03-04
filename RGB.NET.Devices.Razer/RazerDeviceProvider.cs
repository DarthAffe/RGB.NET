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
    public class RazerDeviceProvider : AbstractRGBDeviceProvider
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

        /// <summary>
        /// Forces to load the devices represented by the emulator even if they aren't reported to exist.
        /// </summary>
        public bool LoadEmulatorDevices { get; set; } = false;

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
        }

        #endregion

        #region Methods

        protected override void InitializeSDK()
        {
            TryUnInit();

            _RazerSDK.Reload();

            RazerError error;
            if (((error = _RazerSDK.Init()) != RazerError.Success)
             && Enum.IsDefined(typeof(RazerError), error)) //HACK DarthAffe 08.02.2018: The x86-SDK seems to have a problem here ...
                ThrowRazerError(error);
        }

        protected override IEnumerable<IRGBDevice> LoadDevices()
        {
            foreach ((Guid guid, string model) in Razer.Devices.KEYBOARDS)
            {
                if (((_RazerSDK.QueryDevice(guid, out _DeviceInfo deviceInfo) != RazerError.Success) || (deviceInfo.Connected < 1))
                 && (!LoadEmulatorDevices || (Razer.Devices.KEYBOARDS.FirstOrDefault().guid != guid))) continue;

                yield return new RazerKeyboardRGBDevice(new RazerKeyboardRGBDeviceInfo(guid, model), GetUpdateTrigger());
            }

            foreach ((Guid guid, string model) in Razer.Devices.MICE)
            {
                if (((_RazerSDK.QueryDevice(guid, out _DeviceInfo deviceInfo) != RazerError.Success) || (deviceInfo.Connected < 1))
                 && (!LoadEmulatorDevices || (Razer.Devices.MICE.FirstOrDefault().guid != guid))) continue;

                yield return new RazerMouseRGBDevice(new RazerMouseRGBDeviceInfo(guid, model), GetUpdateTrigger());
            }

            foreach ((Guid guid, string model) in Razer.Devices.HEADSETS)
            {
                if (((_RazerSDK.QueryDevice(guid, out _DeviceInfo deviceInfo) != RazerError.Success) || (deviceInfo.Connected < 1))
                 && (!LoadEmulatorDevices || (Razer.Devices.HEADSETS.FirstOrDefault().guid != guid))) continue;

                yield return new RazerHeadsetRGBDevice(new RazerHeadsetRGBDeviceInfo(guid, model), GetUpdateTrigger());
            }

            foreach ((Guid guid, string model) in Razer.Devices.MOUSEMATS)
            {
                if (((_RazerSDK.QueryDevice(guid, out _DeviceInfo deviceInfo) != RazerError.Success) || (deviceInfo.Connected < 1))
                 && (!LoadEmulatorDevices || (Razer.Devices.MOUSEMATS.FirstOrDefault().guid != guid))) continue;

                yield return new RazerMousepadRGBDevice(new RazerMousepadRGBDeviceInfo(guid, model), GetUpdateTrigger());
            }

            foreach ((Guid guid, string model) in Razer.Devices.KEYPADS)
            {
                if (((_RazerSDK.QueryDevice(guid, out _DeviceInfo deviceInfo) != RazerError.Success) || (deviceInfo.Connected < 1))
                 && (!LoadEmulatorDevices || (Razer.Devices.KEYPADS.FirstOrDefault().guid != guid))) continue;

                yield return new RazerKeypadRGBDevice(new RazerKeypadRGBDeviceInfo(guid, model), GetUpdateTrigger());
            }

            foreach ((Guid guid, string model) in Razer.Devices.CHROMALINKS)
            {
                if (((_RazerSDK.QueryDevice(guid, out _DeviceInfo deviceInfo) != RazerError.Success) || (deviceInfo.Connected < 1))
                 && (!LoadEmulatorDevices || (Razer.Devices.CHROMALINKS.FirstOrDefault().guid != guid))) continue;

                yield return new RazerChromaLinkRGBDevice(new RazerChromaLinkRGBDeviceInfo(guid, model), GetUpdateTrigger());
            }
        }

        private void ThrowRazerError(RazerError errorCode) => throw new RazerException(errorCode);

        private void TryUnInit()
        {
            try { _RazerSDK.UnInit(); }
            catch { /* We tried our best */ }
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            base.Dispose();

            TryUnInit();

            // DarthAffe 03.03.2020: Fails with an access-violation - verify if an unload is already triggered by uninit
            //try { _RazerSDK.UnloadRazerSDK(); }
            //catch { /* at least we tried */ }
        }

        #endregion
    }
}
