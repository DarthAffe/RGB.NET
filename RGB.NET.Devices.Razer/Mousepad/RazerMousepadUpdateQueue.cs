﻿using System;
using System.Runtime.InteropServices;
using RGB.NET.Core;
using RGB.NET.Devices.Razer.Native;

namespace RGB.NET.Devices.Razer;

/// <summary>
/// Represents the update-queue performing updates for razer mousepad devices.
/// </summary>
public sealed class RazerMousepadUpdateQueue : RazerUpdateQueue
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="RazerMousepadUpdateQueue" /> class.
    /// </summary>
    /// <param name="updateTrigger">The update trigger used to update this queue.</param>
    public RazerMousepadUpdateQueue(IDeviceUpdateTrigger updateTrigger)
        : base(updateTrigger)
    { }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override nint CreateEffectParams(ReadOnlySpan<(object key, Color color)> dataSet)
    {
        _Color[] colors = new _Color[_Defines.MOUSEPAD_MAX_LEDS];

        foreach ((object key, Color color) in dataSet)
            colors[(int)key] = new _Color(color);

        _MousepadCustomEffect effectParams = new() { Color = colors };

        nint ptr = Marshal.AllocHGlobal(Marshal.SizeOf(effectParams));
        Marshal.StructureToPtr(effectParams, ptr, false);

        return ptr;
    }

    /// <inheritdoc />
    protected override void CreateEffect(nint effectParams, ref Guid effectId) => _RazerSDK.CreateMousepadEffect(_Defines.MOUSEPAD_EFFECT_ID, effectParams, ref effectId);

    #endregion
}