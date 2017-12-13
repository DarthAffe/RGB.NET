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
    /// Represents a razer mouse.
    /// </summary>
    public class RazerMouseRGBDevice : RazerRGBDevice<RazerMouseRGBDeviceInfo>
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Razer.RazerMouseRGBDevice" /> class.
        /// </summary>
        /// <param name="info">The specific information provided by CUE for the mouse.</param>
        internal RazerMouseRGBDevice(RazerMouseRGBDeviceInfo info)
            : base(info)
        { }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void InitializeLayout()
        {
            string model = DeviceInfo.Model.Replace(" ", string.Empty).ToUpper();
            ApplyLayoutFromFile(PathHelper.GetAbsolutePath(
                $@"Layouts\Razer\Mice\{model}.xml"), null, PathHelper.GetAbsolutePath(@"Images\Razer\Mice"));

            if (LedMapping.Count == 0)
            {
                for (int i = 0; i < _Defines.MOUSE_MAX_ROW; i++)
                    for (int j = 0; j < _Defines.MOUSE_MAX_COLUMN; j++)
                        InitializeLed(LedId.Mouse1 + ((i * _Defines.MOUSE_MAX_COLUMN) + j), new Rectangle(j * 11, i * 11, 10, 10));
            }
        }

        /// <inheritdoc />
        protected override object CreateLedCustomData(LedId ledId) => (int)ledId - (int)LedId.Mouse1;

        /// <inheritdoc />
        protected override IntPtr CreateEffectParams(IEnumerable<Led> leds)
        {
            _Color[] colors = new _Color[_Defines.MOUSE_MAX_LEDS];

            foreach (Led led in leds)
                colors[(int)led.CustomData] = new _Color(led.Color.R, led.Color.G, led.Color.B);

            _MouseCustomEffect effectParams = new _MouseCustomEffect { Color = colors };

            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(effectParams));
            Marshal.StructureToPtr(effectParams, ptr, false);

            return ptr;
        }

        #endregion
    }
}
