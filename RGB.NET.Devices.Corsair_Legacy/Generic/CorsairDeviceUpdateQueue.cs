using System;
using System.Runtime.InteropServices;
using RGB.NET.Core;
using RGB.NET.Devices.CorsairLegacy.Native;

namespace RGB.NET.Devices.CorsairLegacy;

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
    protected override bool Update(in ReadOnlySpan<(object key, Color color)> dataSet)
    {
        try
        {
            int structSize = Marshal.SizeOf(typeof(_CorsairLedColor));
            IntPtr ptr = Marshal.AllocHGlobal(structSize * dataSet.Length);
            IntPtr addPtr = new(ptr.ToInt64());
            try
            {
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
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }

            return true;
        }
        catch (Exception ex)
        {
            CorsairLegacyDeviceProvider.Instance.Throw(ex);
        }

        return false;
    }

    #endregion
}