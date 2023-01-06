using System;
using System.Runtime.InteropServices;
using RGB.NET.Core;
using RGB.NET.Devices.Razer.Native;

namespace RGB.NET.Devices.Razer;

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
    public RazerKeyboardUpdateQueue(IDeviceUpdateTrigger updateTrigger)
        : base(updateTrigger)
    { }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override IntPtr CreateEffectParams(in ReadOnlySpan<(object key, Color color)> dataSet)
    {
        _Color[] colors = new _Color[_Defines.KEYBOARD_MAX_LEDS];

        foreach ((object key, Color color) in dataSet)
            colors[(int)key] = new _Color(color);

        _KeyboardCustomEffect effectParams = new() { Color = colors };

        IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(effectParams));
        Marshal.StructureToPtr(effectParams, ptr, false);

        return ptr;
    }

    /// <inheritdoc />
    protected override void CreateEffect(IntPtr effectParams, ref Guid effectId) => _RazerSDK.CreateKeyboardEffect(_Defines.KEYBOARD_EFFECT_ID, effectParams, ref effectId);

    #endregion
}