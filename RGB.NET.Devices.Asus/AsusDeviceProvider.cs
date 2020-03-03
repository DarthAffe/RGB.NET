// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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

        private static AsusDeviceProvider _instance;
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

        /// <summary>
        /// The <see cref="DeviceUpdateTrigger"/> used to trigger the updates for asus devices. 
        /// </summary>
        public DeviceUpdateTrigger UpdateTrigger { get; private set; }

        private IAuraSdk2 _sdk;

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
                UpdateTrigger?.Stop();

                // ReSharper disable once SuspiciousTypeConversion.Global
                _sdk = (IAuraSdk2)new AuraSdk();
                _sdk.SwitchMode();

                IList<IRGBDevice> devices = new List<IRGBDevice>();
                foreach (IAuraSyncDevice device in _sdk.Enumerate(0))
                {
                    try
                    {
                        IAsusRGBDevice rgbDevice = null;
                        switch (device.Type)
                        {
                            case 0x00010000: //Motherboard
                                rgbDevice = new AsusMainboardRGBDevice(new AsusRGBDeviceInfo(RGBDeviceType.Mainboard, device, WMIHelper.GetMainboardInfo()?.model ?? device.Name));
                                break;

                            case 0x00011000: //Motherboard LED Strip
                                rgbDevice = new AsusUnspecifiedRGBDevice(new AsusRGBDeviceInfo(RGBDeviceType.LedStripe, device), LedId.LedStripe1);
                                break;

                            case 0x00020000: //VGA
                                rgbDevice = new AsusGraphicsCardRGBDevice(new AsusRGBDeviceInfo(RGBDeviceType.GraphicsCard, device));
                                break;

                            case 0x00040000: //Headset
                                rgbDevice = new AsusHeadsetRGBDevice(new AsusRGBDeviceInfo(RGBDeviceType.Headset, device));
                                break;

                            case 0x00070000: //DRAM
                                rgbDevice = new AsusDramRGBDevice(new AsusRGBDeviceInfo(RGBDeviceType.DRAM, device));
                                break;

                            case 0x00080000: //Keyboard
                            case 0x00081000: //Notebook Keyboard
                            case 0x00081001: //Notebook Keyboard(4 - zone type)
                                rgbDevice = new AsusKeyboardRGBDevice(new AsusKeyboardRGBDeviceInfo(device, CultureInfo.CurrentCulture));
                                break;

                            case 0x00090000: //Mouse
                                rgbDevice = new AsusMouseRGBDevice(new AsusRGBDeviceInfo(RGBDeviceType.Mouse, device));
                                break;

                            case 0x00000000: //All
                            case 0x00012000: //All - In - One PC
                            case 0x00030000: //Display
                            case 0x00050000: //Microphone
                            case 0x00060000: //External HDD
                            case 0x00061000: //External BD Drive
                            case 0x000B0000: //Chassis
                            case 0x000C0000: //Projector
                                rgbDevice = new AsusUnspecifiedRGBDevice(new AsusRGBDeviceInfo(RGBDeviceType.Unknown, device), LedId.Custom1);
                                break;
                        }

                        if ((rgbDevice != null) && loadFilter.HasFlag(rgbDevice.DeviceInfo.DeviceType))
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
