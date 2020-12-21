using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using RGB.NET.Core;
using RGB.NET.Devices.Razer.Native;

namespace RGB.NET.Devices.Razer
{
    /// <summary>
    /// Represents the update-queue performing updates for razer keyboard devices.
    /// </summary>
    public class RazerKeyboardUpdateQueue : RazerUpdateQueue
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RazerKeyboardUpdateQueue" /> class.
        /// </summary>
        /// <param name="updateTrigger">The update trigger used to update this queue.</param>
        /// <param name="deviceId">The id of the device updated by this queue.</param>
        public RazerKeyboardUpdateQueue(IDeviceUpdateTrigger updateTrigger, Guid deviceId)
            : base(updateTrigger, deviceId)
        { }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override IntPtr CreateEffectParams(Dictionary<object, Color> dataSet)
        {
            _Color[] colors = new _Color[_Defines.KEYBOARD_MAX_LEDS];

            foreach (KeyValuePair<object, Color> data in dataSet)
                colors[(int)data.Key] = new _Color(data.Value);

            _KeyboardCustomEffect effectParams = new _KeyboardCustomEffect { Color = colors };

            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(effectParams));
            Marshal.StructureToPtr(effectParams, ptr, false);

            return ptr;
        }

        /// <inheritdoc />
        protected override void CreateEffect(IntPtr effectParams, ref Guid effectId) => _RazerSDK.CreateKeyboardEffect(_Defines.KEYBOARD_EFFECT_ID, effectParams, ref effectId);

        #endregion
    }
}
