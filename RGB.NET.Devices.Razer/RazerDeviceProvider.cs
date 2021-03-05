// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

using System;
using System.Collections.Generic;
using System.Linq;
using RGB.NET.Core;
using RGB.NET.Devices.Razer.HID;
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
            if (((error = _RazerSDK.Init()) != RazerError.Success) && Enum.IsDefined(typeof(RazerError), error)) //HACK DarthAffe 08.02.2018: The x86-SDK seems to have a problem here ...
                ThrowRazerError(error);
        }

        protected override IEnumerable<IRGBDevice> GetLoadedDevices(RGBDeviceType loadFilter)
        {
            DeviceChecker.LoadDeviceList(loadFilter);

            IList<IRGBDevice> devices = base.GetLoadedDevices(loadFilter).ToList();

            if (LoadEmulatorDevices)
            {
                if (loadFilter.HasFlag(RGBDeviceType.Keyboard) && devices.All(d => d.DeviceInfo.DeviceType != RGBDeviceType.Keyboard))
                    devices.Add(new RazerKeyboardRGBDevice(new RazerKeyboardRGBDeviceInfo("Emulator Keyboard"), GetUpdateTrigger()));
                if (loadFilter.HasFlag(RGBDeviceType.Mouse) && devices.All(d => d.DeviceInfo.DeviceType != RGBDeviceType.Mouse))
                    devices.Add(new RazerMouseRGBDevice(new RazerRGBDeviceInfo(RGBDeviceType.Mouse, "Emulator Mouse"), GetUpdateTrigger()));
                if (loadFilter.HasFlag(RGBDeviceType.Headset) && devices.All(d => d.DeviceInfo.DeviceType != RGBDeviceType.Headset))
                    devices.Add(new RazerHeadsetRGBDevice(new RazerRGBDeviceInfo(RGBDeviceType.Headset, "Emulator Headset"), GetUpdateTrigger()));
                if (loadFilter.HasFlag(RGBDeviceType.Mousepad) && devices.All(d => d.DeviceInfo.DeviceType != RGBDeviceType.Mousepad))
                    devices.Add(new RazerMousepadRGBDevice(new RazerRGBDeviceInfo(RGBDeviceType.Mousepad, "Emulator Mousepad"), GetUpdateTrigger()));
                if (loadFilter.HasFlag(RGBDeviceType.Keypad) && devices.All(d => d.DeviceInfo.DeviceType != RGBDeviceType.Keypad))
                    devices.Add(new RazerKeypadRGBDevice(new RazerRGBDeviceInfo(RGBDeviceType.Keypad, "Emulator Keypad"), GetUpdateTrigger()));
                if (loadFilter.HasFlag(RGBDeviceType.Unknown) && devices.All(d => d.DeviceInfo.DeviceType != RGBDeviceType.Unknown))
                    devices.Add(new RazerChromaLinkRGBDevice(new RazerRGBDeviceInfo(RGBDeviceType.Unknown, "Emulator Chroma Link"), GetUpdateTrigger()));
            }

            return devices;
        }

        protected override IEnumerable<IRGBDevice> LoadDevices()
        {
            foreach ((var model, RGBDeviceType deviceType, int _) in DeviceChecker.ConnectedDevices)
            {
                yield return deviceType switch
                {
                    RGBDeviceType.Keyboard => new RazerKeyboardRGBDevice(new RazerKeyboardRGBDeviceInfo(model), GetUpdateTrigger()),
                    RGBDeviceType.Mouse => new RazerMouseRGBDevice(new RazerRGBDeviceInfo(deviceType, model), GetUpdateTrigger()),
                    RGBDeviceType.Headset => new RazerHeadsetRGBDevice(new RazerRGBDeviceInfo(deviceType, model), GetUpdateTrigger()),
                    RGBDeviceType.Mousepad => new RazerMousepadRGBDevice(new RazerRGBDeviceInfo(deviceType, model), GetUpdateTrigger()),
                    RGBDeviceType.Keypad => new RazerKeypadRGBDevice(new RazerRGBDeviceInfo(deviceType, model), GetUpdateTrigger()),
                    RGBDeviceType.Unknown => new RazerChromaLinkRGBDevice(new RazerRGBDeviceInfo(deviceType, model), GetUpdateTrigger()),
                    _ => throw new RGBDeviceException($"Razer SDK does not support {deviceType} devices")
                };
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
