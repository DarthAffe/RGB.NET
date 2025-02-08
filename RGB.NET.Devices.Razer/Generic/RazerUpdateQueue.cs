﻿using System;
using RGB.NET.Core;
using RGB.NET.Devices.Razer.Native;

namespace RGB.NET.Devices.Razer;

/// <summary>
/// Represents a basic update-queue performing updates for razer devices.
/// </summary>
public abstract class RazerUpdateQueue : UpdateQueue
{
    #region Properties & Fields

    private Guid? _lastEffect;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="RazerUpdateQueue" /> class.
    /// </summary>
    /// <param name="updateTrigger">The update trigger used to update this queue.</param>
    protected RazerUpdateQueue(IDeviceUpdateTrigger updateTrigger)
        : base(updateTrigger)
    { }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override bool Update(ReadOnlySpan<(object key, Color color)> dataSet)
    {
        try
        {
            nint effectParams = CreateEffectParams(dataSet);
            Guid effectId = Guid.NewGuid();
            CreateEffect(effectParams, ref effectId);

            _RazerSDK.SetEffect(effectId);

            if (_lastEffect.HasValue)
                _RazerSDK.DeleteEffect(_lastEffect.Value);

            _lastEffect = effectId;

            return true;
        }
        catch (Exception ex)
        {
            RazerDeviceProvider.Instance.Throw(ex);
        }

        return false;
    }

    /// <summary>
    /// Creates the effect used to update this device.
    /// </summary>
    /// <param name="effectParams">The parameters of the effect.</param>
    /// <param name="effectId">The id this effect is created with.</param>
    protected abstract void CreateEffect(nint effectParams, ref Guid effectId);

    /// <inheritdoc />
    public override void Reset()
    {
        if (_lastEffect.HasValue)
        {
            _RazerSDK.DeleteEffect(_lastEffect.Value);
            _lastEffect = null;
        }
    }

    /// <summary>
    /// Creates the device-specific effect parameters for the led-update.
    /// </summary>
    /// <param name="dataSet">The data to be updated.</param>
    /// <returns>An <see cref="IntPtr"/> pointing to the effect parameter struct.</returns>
    protected abstract nint CreateEffectParams(ReadOnlySpan<(object key, Color color)> dataSet);

    #endregion
}