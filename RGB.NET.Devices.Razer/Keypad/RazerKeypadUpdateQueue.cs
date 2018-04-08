using System;
using System.Collections.Generic;
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
        protected override IntPtr CreateEffectParams(Dictionary<object, Color> dataSet)
        {
            _Color[] colors = new _Color[_Defines.KEYPAD_MAX_LEDS];

            foreach (KeyValuePair<object, Color> data in dataSet)
                colors[(int)data.Key] = new _Color(data.Value);

            _KeypadCustomEffect effectParams = new _KeypadCustomEffect { Color = colors };

            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(effectParams));
            Marshal.StructureToPtr(effectParams, ptr, false);

            return ptr;
        }

        #endregion
    }
}
