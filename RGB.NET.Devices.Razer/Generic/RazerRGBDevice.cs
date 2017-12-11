using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RGB.NET.Core;
using RGB.NET.Core.Layout;
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

        /// <summary>
        /// Applies the given layout.
        /// </summary>
        /// <param name="layoutPath">The file containing the layout.</param>
        /// <param name="imageLayout">The name of the layout used to get the images of the leds.</param>
        /// <param name="imageBasePath">The path images for this device are collected in.</param>
        protected void ApplyLayoutFromFile(string layoutPath, string imageLayout, string imageBasePath)
        {
            DeviceLayout layout = DeviceLayout.Load(layoutPath);
            if (layout != null)
            {
                LedImageLayout ledImageLayout = layout.LedImageLayouts.FirstOrDefault(x => string.Equals(x.Layout, imageLayout, StringComparison.OrdinalIgnoreCase));

                Size = new Size(layout.Width, layout.Height);

                if (layout.Leds != null)
                    foreach (LedLayout layoutLed in layout.Leds)
                    {
                        if (Enum.TryParse(layoutLed.Id, true, out int ledIndex))
                        {
                            if (LedMapping.TryGetValue(new RazerLedId(this, ledIndex), out Led led))
                            {
                                led.LedRectangle.Location = new Point(layoutLed.X, layoutLed.Y);
                                led.LedRectangle.Size = new Size(layoutLed.Width, layoutLed.Height);

                                led.Shape = layoutLed.Shape;
                                led.ShapeData = layoutLed.ShapeData;

                                LedImage image = ledImageLayout?.LedImages.FirstOrDefault(x => x.Id == layoutLed.Id);
                                led.Image = (!string.IsNullOrEmpty(image?.Image))
                                    ? new Uri(Path.Combine(imageBasePath, image.Image), UriKind.Absolute)
                                    : new Uri(Path.Combine(imageBasePath, "Missing.png"), UriKind.Absolute);
                            }
                        }
                    }
            }
        }

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
