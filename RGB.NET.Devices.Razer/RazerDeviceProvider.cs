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
                if (loadFilter.HasFlag(RGBDeviceType.Keyboard) && devices.All(d => d is not RazerKeyboardRGBDevice))
                    devices.Add(new RazerKeyboardRGBDevice(new RazerKeyboardRGBDeviceInfo("Emulator Keyboard", RazerEndpointType.Keyboard), GetUpdateTrigger()));
                if (loadFilter.HasFlag(RGBDeviceType.Mouse) && devices.All(d => d is not RazerMouseRGBDevice))
                    devices.Add(new RazerMouseRGBDevice(new RazerRGBDeviceInfo(RGBDeviceType.Mouse, RazerEndpointType.Mouse, "Emulator Mouse"), GetUpdateTrigger()));
                if (loadFilter.HasFlag(RGBDeviceType.Headset) && devices.All(d => d is not RazerHeadsetRGBDevice))
                    devices.Add(new RazerHeadsetRGBDevice(new RazerRGBDeviceInfo(RGBDeviceType.Headset, RazerEndpointType.Headset, "Emulator Headset"), GetUpdateTrigger()));
                if (loadFilter.HasFlag(RGBDeviceType.Mousepad) && devices.All(d => d is not RazerMousepadRGBDevice))
                    devices.Add(new RazerMousepadRGBDevice(new RazerRGBDeviceInfo(RGBDeviceType.Mousepad, RazerEndpointType.Mousepad, "Emulator Mousepad"), GetUpdateTrigger()));
                if (loadFilter.HasFlag(RGBDeviceType.Keypad) && devices.All(d => d is not RazerMousepadRGBDevice))
                    devices.Add(new RazerKeypadRGBDevice(new RazerRGBDeviceInfo(RGBDeviceType.Keypad, RazerEndpointType.Keypad, "Emulator Keypad"), GetUpdateTrigger()));
                if (loadFilter.HasFlag(RGBDeviceType.Unknown) && devices.All(d => d is not RazerChromaLinkRGBDevice))
                    devices.Add(new RazerChromaLinkRGBDevice(new RazerRGBDeviceInfo(RGBDeviceType.Unknown, RazerEndpointType.ChromaLink, "Emulator Chroma Link"), GetUpdateTrigger()));
            }

            return devices;
        }

        protected override IEnumerable<IRGBDevice> LoadDevices()
        {
            // Only take the first device of each endpoint type, the Razer SDK doesn't allow separate control over multiple devices using the same endpoint
            foreach ((var model, RGBDeviceType deviceType, RazerEndpointType endpointType, int _) in DeviceChecker.ConnectedDevices.GroupBy(GetEndpointDeviceType).Select(t => t.First()))
            {
                yield return endpointType switch
                {
                    RazerEndpointType.Keyboard => new RazerKeyboardRGBDevice(new RazerKeyboardRGBDeviceInfo(model, endpointType), GetUpdateTrigger()),
                    RazerEndpointType.LaptopKeyboard => new RazerKeyboardRGBDevice(new RazerKeyboardRGBDeviceInfo(model, endpointType), GetUpdateTrigger()),
                    RazerEndpointType.Mouse => new RazerMouseRGBDevice(new RazerRGBDeviceInfo(deviceType, endpointType, model), GetUpdateTrigger()),
                    RazerEndpointType.Headset => new RazerHeadsetRGBDevice(new RazerRGBDeviceInfo(deviceType, endpointType, model), GetUpdateTrigger()),
                    RazerEndpointType.Mousepad => new RazerMousepadRGBDevice(new RazerRGBDeviceInfo(deviceType, endpointType, model), GetUpdateTrigger()),
                    RazerEndpointType.Keypad => new RazerKeypadRGBDevice(new RazerRGBDeviceInfo(deviceType, endpointType, model), GetUpdateTrigger()),
                    RazerEndpointType.ChromaLink => new RazerChromaLinkRGBDevice(new RazerRGBDeviceInfo(deviceType, endpointType, model), GetUpdateTrigger()),
                    _ => throw new RGBDeviceException($"Razer SDK does not support endpoint '{endpointType}'")
                };
            }
        }

        private RazerEndpointType GetEndpointDeviceType((string model, RGBDeviceType deviceType, RazerEndpointType razerDeviceType, int id) device)
        {
            // Treat laptop keyboards as regular keyboards
            return device.razerDeviceType == RazerEndpointType.LaptopKeyboard ? RazerEndpointType.Keyboard : device.razerDeviceType;
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
