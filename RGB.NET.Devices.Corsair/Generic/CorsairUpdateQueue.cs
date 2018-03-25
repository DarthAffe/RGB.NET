using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using RGB.NET.Core;
using RGB.NET.Devices.Corsair.Native;

namespace RGB.NET.Devices.Corsair
{
    public class CorsairUpdateQueue : UpdateQueue
    {
        #region Constructors

        public CorsairUpdateQueue(IUpdateTrigger updateTrigger)
            : base(updateTrigger)
        { }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void Update(Dictionary<object, Color> dataSet)
        {
            int structSize = Marshal.SizeOf(typeof(_CorsairLedColor));
            IntPtr ptr = Marshal.AllocHGlobal(structSize * dataSet.Count);
            IntPtr addPtr = new IntPtr(ptr.ToInt64());
            foreach (KeyValuePair<object, Color> data in dataSet)
            {
                _CorsairLedColor color = new _CorsairLedColor
                {
                    ledId = (int)data.Key,
                    r = data.Value.R,
                    g = data.Value.G,
                    b = data.Value.B
                };

                Marshal.StructureToPtr(color, addPtr, false);
                addPtr = new IntPtr(addPtr.ToInt64() + structSize);
            }
            _CUESDK.CorsairSetLedsColors(dataSet.Count, ptr);
            Marshal.FreeHGlobal(ptr);
        }

        #endregion
    }
}
