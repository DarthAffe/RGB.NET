// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using AuraServiceLib;
using RGB.NET.Core;

namespace RGB.NET.Devices.Asus
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a device provider responsible for Cooler Master devices.
    /// </summary>
    public class AsusDeviceProvider : AbstractRGBDeviceProvider
    {
        #region Properties & Fields

        private static AsusDeviceProvider? _instance;
        /// <summary>
        /// Gets the singleton <see cref="AsusDeviceProvider"/> instance.
        /// </summary>
        public static AsusDeviceProvider Instance => _instance ?? new AsusDeviceProvider();

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
        }

        #endregion

        #region Methods

        protected override void InitializeSDK()
        {
            _sdk = (IAuraSdk2)new AuraSdk();
            _sdk.SwitchMode();
        }

        protected override IEnumerable<IRGBDevice> LoadDevices()
        {
            if (_sdk == null) yield break;

            foreach (IAuraSyncDevice device in _sdk.Enumerate(0))
            {
                yield return (AsusDeviceType)device.Type switch
                {
                    AsusDeviceType.MB_RGB => new AsusMainboardRGBDevice(new AsusRGBDeviceInfo(RGBDeviceType.Mainboard, device, WMIHelper.GetMainboardInfo()?.model ?? device.Name), GetUpdateTrigger()),
                    AsusDeviceType.MB_ADDRESABLE => new AsusUnspecifiedRGBDevice(new AsusRGBDeviceInfo(RGBDeviceType.LedStripe, device), LedId.LedStripe1, GetUpdateTrigger()),
                    AsusDeviceType.VGA_RGB => new AsusGraphicsCardRGBDevice(new AsusRGBDeviceInfo(RGBDeviceType.GraphicsCard, device), GetUpdateTrigger()),
                    AsusDeviceType.HEADSET_RGB => new AsusHeadsetRGBDevice(new AsusRGBDeviceInfo(RGBDeviceType.Headset, device), GetUpdateTrigger()),
                    AsusDeviceType.DRAM_RGB => new AsusDramRGBDevice(new AsusRGBDeviceInfo(RGBDeviceType.DRAM, device), GetUpdateTrigger()),
                    AsusDeviceType.KEYBOARD_RGB => new AsusKeyboardRGBDevice(new AsusKeyboardRGBDeviceInfo(device), GetUpdateTrigger()),
                    AsusDeviceType.NB_KB_RGB => new AsusKeyboardRGBDevice(new AsusKeyboardRGBDeviceInfo(device), GetUpdateTrigger()),
                    AsusDeviceType.NB_KB_4ZONE_RGB => new AsusKeyboardRGBDevice(new AsusKeyboardRGBDeviceInfo(device), GetUpdateTrigger()),
                    AsusDeviceType.MOUSE_RGB => new AsusMouseRGBDevice(new AsusRGBDeviceInfo(RGBDeviceType.Mouse, device), GetUpdateTrigger()),
                    _ => new AsusUnspecifiedRGBDevice(new AsusRGBDeviceInfo(RGBDeviceType.Unknown, device), LedId.Custom1, GetUpdateTrigger())
                };
            }
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            base.Dispose();

            try { _sdk?.ReleaseControl(0); }
            catch { /* at least we tried */ }

            _sdk = null;
        }

        #endregion
    }
}
