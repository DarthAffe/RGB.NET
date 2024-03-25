using System;
using Windows.Devices.Lights;
using RGB.NET.Core;
using System.Linq;

namespace RGB.NET.Devices.DynamicLighting;

/// <inheritdoc />
/// <summary>
/// Represents the update-queue performing updates for dynamic lightning devices.
/// </summary>
public sealed class DynamicLightingDeviceUpdateQueue : UpdateQueue
{
    #region Properties & Fields

    private bool _isDisposed = false;

    private readonly LampArray _lampArray;
    private readonly int[] _indices;
    private readonly Windows.UI.Color[] _colors;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="DynamicLightingDeviceUpdateQueue"/> class.
    /// </summary>
    /// <param name="updateTrigger">The update trigger used by this queue.</param>
    /// <param name="lampArray">The lamp array to update.</param>
    internal DynamicLightingDeviceUpdateQueue(IDeviceUpdateTrigger updateTrigger, LampArray lampArray)
        : base(updateTrigger)
    {
        this._lampArray = lampArray;
        
        _colors = new Windows.UI.Color[lampArray.LampCount];
        _indices = Enumerable.Range(0, lampArray.LampCount).ToArray();
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override bool Update(in ReadOnlySpan<(object key, Color color)> dataSet)
    {
        try
        {
            if (_isDisposed) throw new ObjectDisposedException(nameof(DynamicLightingDeviceUpdateQueue));
            if (!_lampArray.IsConnected) return false;

            // ReSharper disable once ForCanBeConvertedToForeach - Prevent a possible allocation of an enumerator
            for (int i = 0; i < dataSet.Length; i++)
            {
                (object key, Color color) = dataSet[i];
                (byte a, byte r, byte g, byte b) = color.GetRGBBytes();
                _colors[(int)key] = Windows.UI.Color.FromArgb(a, r, g, b);
            }

            _lampArray.SetColorsForIndices(_colors, _indices);

            return true;
        }
        catch (Exception ex)
        {
            DynamicLightingDeviceProvider.Instance.Throw(ex);
        }

        return false;
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        base.Dispose();

        _isDisposed = true;
    }

    #endregion
}