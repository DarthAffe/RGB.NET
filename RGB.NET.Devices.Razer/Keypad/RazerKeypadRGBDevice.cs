// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using RGB.NET.Core;
using RGB.NET.Devices.Razer.Native;

namespace RGB.NET.Devices.Razer
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a razer keypad.
    /// </summary>
    public class RazerKeypadRGBDevice : RazerRGBDevice<RazerKeypadRGBDeviceInfo>
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Razer.RazerKeypadRGBDevice" /> class.
        /// </summary>
        /// <param name="info">The specific information provided by CUE for the keypad.</param>
        internal RazerKeypadRGBDevice(RazerKeypadRGBDeviceInfo info)
            : base(info)
        { }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void InitializeLayout()
        {
            string model = DeviceInfo.Model.Replace(" ", string.Empty).ToUpper();
            ApplyLayoutFromFile(PathHelper.GetAbsolutePath($@"Layouts\Razer\Keypad\{model}.xml"), null);

            if (LedMapping.Count == 0)
            {
                for (int i = 0; i < _Defines.KEYPAD_MAX_ROW; i++)
                    for (int j = 0; j < _Defines.KEYPAD_MAX_COLUMN; j++)
                        InitializeLed(LedId.Keypad1 + ((i * _Defines.KEYPAD_MAX_COLUMN) + j), new Rectangle(j * 20, i * 20, 19, 19));
            }
        }

        /// <inheritdoc />
        protected override object CreateLedCustomData(LedId ledId) => (int)ledId - (int)LedId.Keypad1;

        /// <inheritdoc />
        protected override IntPtr CreateEffectParams(IEnumerable<Led> leds)
        {
            _Color[] colors = new _Color[_Defines.KEYPAD_MAX_LEDS];

            foreach (Led led in leds)
                colors[(int)led.CustomData] = new _Color(led.Color.R, led.Color.G, led.Color.B);

            _KeypadCustomEffect effectParams = new _KeypadCustomEffect { Color = colors };

            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(effectParams));
            Marshal.StructureToPtr(effectParams, ptr, false);

            return ptr;
        }

        #endregion
    }
}
