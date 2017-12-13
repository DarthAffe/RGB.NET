using System;
using System.Collections.Generic;
using System.Linq;
using RGB.NET.Core;
using RGB.NET.Devices.Razer.Native;

namespace RGB.NET.Devices.Razer
{
    /// <inheritdoc cref="AbstractRGBDevice{TDeviceInfo}" />
    /// <inheritdoc cref="IRazerRGBDevice" />
    /// <summary>
    /// Represents a generic razer-device. (keyboard, mouse, headset, mousepad).
    /// </summary>
    public abstract class RazerRGBDevice<TDeviceInfo> : AbstractRGBDevice<TDeviceInfo>, IRazerRGBDevice
        where TDeviceInfo : RazerRGBDeviceInfo
    {
        #region Properties & Fields

        private Guid? _lastEffect;

        /// <inheritdoc />
        /// <summary>
        /// Gets information about the <see cref="T:RGB.NET.Devices.Razer.RazerRGBDevice" />.
        /// </summary>
        public override TDeviceInfo DeviceInfo { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RazerRGBDevice{TDeviceInfo}"/> class.
        /// </summary>
        /// <param name="info">The generic information provided by razer for the device.</param>
        protected RazerRGBDevice(TDeviceInfo info)
        {
            this.DeviceInfo = info;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the device.
        /// </summary>
        public void Initialize()
        {
            InitializeLayout();

            if (Size == Size.Invalid)
            {
                Rectangle ledRectangle = new Rectangle(this.Select(x => x.LedRectangle));
                Size = ledRectangle.Size + new Size(ledRectangle.Location.X, ledRectangle.Location.Y);
            }
        }

        /// <summary>
        /// Initializes the <see cref="Led"/> and <see cref="Size"/> of the device.
        /// </summary>
        protected abstract void InitializeLayout();
        
        /// <inheritdoc />
        protected override void UpdateLeds(IEnumerable<Led> ledsToUpdate)
        {
            List<Led> leds = ledsToUpdate.Where(x => x.Color.A > 0).ToList();

            if (leds.Count <= 0) return;

            IntPtr effectParams = CreateEffectParams(leds);
            Guid effectId = Guid.NewGuid();
            _RazerSDK.CreateEffect(DeviceInfo.DeviceId, _Defines.EFFECT_ID, effectParams, ref effectId);

            _RazerSDK.SetEffect(effectId);

            if (_lastEffect.HasValue)
                _RazerSDK.DeleteEffect(_lastEffect.Value);

            _lastEffect = effectId;
        }

        /// <summary>
        /// Creates the device-specific effect parameters for the led-update.
        /// </summary>
        /// <param name="leds">The leds to be updated.</param>
        /// <returns>An <see cref="IntPtr"/> pointing to the effect parameter struct.</returns>
        protected abstract IntPtr CreateEffectParams(IEnumerable<Led> leds);

        /// <summary>
        /// Resets the device.
        /// </summary>
        public void Reset()
        {
            if (_lastEffect.HasValue)
            {
                _RazerSDK.DeleteEffect(_lastEffect.Value);
                _lastEffect = null;
            }
        }

        #endregion
    }
}
