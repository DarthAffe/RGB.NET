// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using AuraServiceLib;
using RGB.NET.Core;

namespace RGB.NET.Devices.Asus
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a device provider responsible for Cooler Master devices.
    /// </summary>
    public class AsusDeviceProvider : IRGBDeviceProvider
    {
        #region Properties & Fields

        private static AsusDeviceProvider? _instance;
        /// <summary>
        /// Gets the singleton <see cref="AsusDeviceProvider"/> instance.
        /// </summary>
        public static AsusDeviceProvider Instance => _instance ?? new AsusDeviceProvider();

        /// <inheritdoc />
        /// <summary>
        /// Indicates if the SDK is initialized and ready to use.
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <inheritdoc />
        public IEnumerable<IRGBDevice> Devices { get; private set; } = Enumerable.Empty<IRGBDevice>();

        /// <summary>
        /// The <see cref="DeviceUpdateTrigger"/> used to trigger the updates for asus devices. 
        /// </summary>
        public DeviceUpdateTrigger UpdateTrigger { get; private set; }

        private IAuraSdk2? _sdk;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AsusDeviceProvider"/> class.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if this constructor is called even if there is already an instance of this class.</exception>
        public AsusDeviceProvider()
        {
            if (_instance != null) throw new InvalidOperationException($"There can be only one instance of type {nameof(AsusDeviceProvider)}");
            _instance = this;

            UpdateTrigger = new DeviceUpdateTrigger();
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public bool Initialize(RGBDeviceType loadFilter = RGBDeviceType.All, bool exclusiveAccessIfPossible = false, bool throwExceptions = false)
        {
            IsInitialized = false;

            try
            {
                UpdateTrigger.Stop();

                // ReSharper disable once SuspiciousTypeConversion.Global
                _sdk = (IAuraSdk2)new AuraSdk();
                _sdk.SwitchMode();

                IList<IRGBDevice> devices = new List<IRGBDevice>();
                foreach (IAuraSyncDevice device in _sdk.Enumerate(0))
                {
                    try
                    {
                        IAsusRGBDevice rgbDevice;
                        switch ((AsusDeviceType)device.Type)
                        {
                            case AsusDeviceType.MB_RGB:
                                rgbDevice = new AsusMainboardRGBDevice(new AsusRGBDeviceInfo(RGBDeviceType.Mainboard, device, WMIHelper.GetMainboardInfo()?.model ?? device.Name));
                                break;

                            case AsusDeviceType.MB_ADDRESABLE:
                                rgbDevice = new AsusUnspecifiedRGBDevice(new AsusRGBDeviceInfo(RGBDeviceType.LedStripe, device), LedId.LedStripe1);
                                break;

                            case AsusDeviceType.VGA_RGB:
                                rgbDevice = new AsusGraphicsCardRGBDevice(new AsusRGBDeviceInfo(RGBDeviceType.GraphicsCard, device));
                                break;

                            case AsusDeviceType.HEADSET_RGB:
                                rgbDevice = new AsusHeadsetRGBDevice(new AsusRGBDeviceInfo(RGBDeviceType.Headset, device));
                                break;

                            case AsusDeviceType.DRAM_RGB:
                                rgbDevice = new AsusDramRGBDevice(new AsusRGBDeviceInfo(RGBDeviceType.DRAM, device));
                                break;

                            case AsusDeviceType.KEYBOARD_RGB:
                            case AsusDeviceType.NB_KB_RGB:
                            case AsusDeviceType.NB_KB_4ZONE_RGB:
                                rgbDevice = new AsusKeyboardRGBDevice(new AsusKeyboardRGBDeviceInfo(device, CultureInfo.CurrentCulture));
                                break;

                            case AsusDeviceType.MOUSE_RGB:
                                rgbDevice = new AsusMouseRGBDevice(new AsusRGBDeviceInfo(RGBDeviceType.Mouse, device));
                                break;

                            default:
                                rgbDevice = new AsusUnspecifiedRGBDevice(new AsusRGBDeviceInfo(RGBDeviceType.Unknown, device), LedId.Custom1);
                                break;
                        }

                        if (loadFilter.HasFlag(rgbDevice.DeviceInfo.DeviceType))
                        {
                            rgbDevice.Initialize(UpdateTrigger);
                            devices.Add(rgbDevice);
                        }
                    }
                    catch
                    {
                        if (throwExceptions)
                            throw;
                    }
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
        public void ResetDevices()
        {
            _sdk?.ReleaseControl(0);
            _sdk?.SwitchMode();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            try { UpdateTrigger?.Dispose(); }
            catch { /* at least we tried */ }

            try { _sdk?.ReleaseControl(0); }
            catch { /* at least we tried */ }

            _sdk = null;
        }

        #endregion
    }
}
