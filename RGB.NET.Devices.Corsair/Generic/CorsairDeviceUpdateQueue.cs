using System;
using System.Runtime.InteropServices;
using RGB.NET.Core;
using RGB.NET.Devices.Corsair.Native;

namespace RGB.NET.Devices.Corsair;

/// <inheritdoc />
/// <summary>
/// Represents the update-queue performing updates for corsair devices.
/// </summary>
public sealed class CorsairDeviceUpdateQueue : UpdateQueue
{
    #region Properties & Fields

    private bool _isDisposed = false;

    private readonly _CorsairDeviceInfo _device;
    private readonly nint _colorPtr;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CorsairDeviceUpdateQueue"/> class.
    /// </summary>
    /// <param name="updateTrigger">The update trigger used by this queue.</param>
    /// <param name="deviceIndex">The index used to identify the device.</param>
    internal CorsairDeviceUpdateQueue(IDeviceUpdateTrigger updateTrigger, _CorsairDeviceInfo device)
        : base(updateTrigger)
    {
        this._device = device;

        _colorPtr = Marshal.AllocHGlobal(Marshal.SizeOf<_CorsairLedColor>() * device.ledCount);
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override unsafe bool Update(ReadOnlySpan<(object key, Color color)> dataSet)
    {
        try
        {
            if (_isDisposed) throw new ObjectDisposedException(nameof(CorsairDeviceUpdateQueue));
            if (!_CUESDK.IsConnected) return false;

            Span<_CorsairLedColor> colors = new((void*)_colorPtr, dataSet.Length);
            for (int i = 0; i < colors.Length; i++)
            {
                (object id, Color color) = dataSet[i];
                (byte a, byte r, byte g, byte b) = color.GetRGBBytes();
                colors[i] = new _CorsairLedColor((CorsairLedId)id, r, g, b, a);
            }

            CorsairError error = _CUESDK.CorsairSetLedColors(_device.id!, dataSet.Length, _colorPtr);
            if (error != CorsairError.Success)
                throw new RGBDeviceException($"Failed to update device '{_device.id}'. (ErrorCode: {error})");

            return true;
        }
        catch (Exception ex)
        {
            CorsairDeviceProvider.Instance.Throw(ex);
        }

        return false;
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        base.Dispose();

        _isDisposed = true;
        Marshal.FreeHGlobal(_colorPtr);
    }

    #endregion
}