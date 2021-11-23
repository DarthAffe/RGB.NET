using System;
using System.Runtime.InteropServices;
using RGB.NET.Core;
using RGB.NET.Devices.Corsair.Native;

namespace RGB.NET.Devices.Corsair;

/// <inheritdoc />
/// <summary>
/// Represents the update-queue performing updates for corsair devices.
/// </summary>
public class CorsairDeviceUpdateQueue : UpdateQueue
{
    #region Properties & Fields

    private int _deviceIndex;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CorsairDeviceUpdateQueue"/> class.
    /// </summary>
    /// <param name="updateTrigger">The update trigger used by this queue.</param>
    /// <param name="deviceIndex">The index used to identify the device.</param>
    public CorsairDeviceUpdateQueue(IDeviceUpdateTrigger updateTrigger, int deviceIndex)
        : base(updateTrigger)
    {
        this._deviceIndex = deviceIndex;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override void Update(in ReadOnlySpan<(object key, Color color)> dataSet)
    {
        int structSize = Marshal.SizeOf(typeof(_CorsairLedColor));
        IntPtr ptr = Marshal.AllocHGlobal(structSize * dataSet.Length);
        IntPtr addPtr = new(ptr.ToInt64());
        foreach ((object key, Color color) in dataSet)
        {
            _CorsairLedColor corsairColor = new()
                                            {
                                                ledId = (int)key,
                                                r = color.GetR(),
                                                g = color.GetG(),
                                                b = color.GetB()
                                            };

            Marshal.StructureToPtr(corsairColor, addPtr, false);
            addPtr = new IntPtr(addPtr.ToInt64() + structSize);
        }

        _CUESDK.CorsairSetLedsColorsBufferByDeviceIndex(_deviceIndex, dataSet.Length, ptr);
        _CUESDK.CorsairSetLedsColorsFlushBuffer();
        Marshal.FreeHGlobal(ptr);
    }

    #endregion
}