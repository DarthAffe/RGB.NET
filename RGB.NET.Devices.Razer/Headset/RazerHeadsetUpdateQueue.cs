using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using RGB.NET.Core;
using RGB.NET.Devices.Razer.Native;

namespace RGB.NET.Devices.Razer
{
    /// <summary>
    /// Represents the update-queue performing updates for razer headset devices.
    /// </summary>
    public class RazerHeadsetUpdateQueue : RazerUpdateQueue
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RazerHeadsetUpdateQueue" /> class.
        /// </summary>
        /// <param name="updateTrigger">The update trigger used to update this queue.</param>
        public RazerHeadsetUpdateQueue(IDeviceUpdateTrigger updateTrigger)
            : base(updateTrigger)
        { }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override IntPtr CreateEffectParams(Dictionary<object, Color> dataSet)
        {
            _Color[] colors = new _Color[_Defines.HEADSET_MAX_LEDS];

            foreach (KeyValuePair<object, Color> data in dataSet)
                colors[(int)data.Key] = new _Color(data.Value);

            _HeadsetCustomEffect effectParams = new()
                                                { Color = colors };
          
            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(effectParams));
            Marshal.StructureToPtr(effectParams, ptr, false);

            return ptr;
        }

        /// <inheritdoc />
        protected override void CreateEffect(IntPtr effectParams, ref Guid effectId) => _RazerSDK.CreateHeadsetEffect(_Defines.HEADSET_EFFECT_ID, effectParams, ref effectId);

        #endregion
    }
}
