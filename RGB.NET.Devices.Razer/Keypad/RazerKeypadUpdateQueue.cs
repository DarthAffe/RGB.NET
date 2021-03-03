using System;
using System.Runtime.InteropServices;
using RGB.NET.Core;
using RGB.NET.Devices.Razer.Native;

namespace RGB.NET.Devices.Razer
{
    /// <summary>
    /// Represents the update-queue performing updates for razer keypad devices.
    /// </summary>
    public class RazerKeypadUpdateQueue : RazerUpdateQueue
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RazerKeypadUpdateQueue" /> class.
        /// </summary>
        /// <param name="updateTrigger">The update trigger used to update this queue.</param>
        /// <param name="deviceId">The id of the device updated by this queue.</param>
        public RazerKeypadUpdateQueue(IDeviceUpdateTrigger updateTrigger, Guid deviceId)
            : base(updateTrigger, deviceId)
        { }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override IntPtr CreateEffectParams(in ReadOnlySpan<(object key, Color color)> dataSet)
        {
            _Color[] colors = new _Color[_Defines.KEYPAD_MAX_LEDS];

            foreach ((object key, Color color) in dataSet)
                colors[(int)key] = new _Color(color);

            _KeypadCustomEffect effectParams = new() { Color = colors };

            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(effectParams));
            Marshal.StructureToPtr(effectParams, ptr, false);

            return ptr;
        }

        #endregion
    }
}
