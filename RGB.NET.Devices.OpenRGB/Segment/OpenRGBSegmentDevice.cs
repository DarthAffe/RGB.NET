using OpenRGB.NET;
using RGB.NET.Core;

namespace RGB.NET.Devices.OpenRGB;

/// <inheritdoc />
public class OpenRGBSegmentDevice : AbstractOpenRGBDevice<OpenRGBDeviceInfo>
{
    #region Properties & Fields

    private readonly int _initialLed;
    private readonly Segment _segment;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="OpenRGBZoneDevice"/> class.
    /// </summary>
    /// <param name="info">The information provided by OpenRGB</param>
    /// <param name="initialLed">The ledId of the first led in the device that belongs to this zone.</param>
    /// <param name="segment">The Segment information provided by OpenRGB.</param>
    /// <param name="updateQueue">The queue used to update this zone.</param>
    public OpenRGBSegmentDevice(OpenRGBDeviceInfo info, int initialLed, Segment segment, IUpdateQueue updateQueue)
        : base(info, updateQueue)
    {
        _initialLed = initialLed;
        _segment = segment;

        InitializeLayout();
    }

    #endregion

    #region Methods

    private void InitializeLayout()
    {
        Size ledSize = new(19);
        const int LED_SPACING = 20;
        LedId initialId = Helper.GetInitialLedIdForDeviceType(DeviceInfo.DeviceType);
        
        for (int i = 0; i < _segment.LedCount; i++)
        {
            LedId ledId = initialId++;

            // ReSharper disable once HeapView.BoxingAllocation
            while (AddLed(ledId, new Point(LED_SPACING * i, 0), ledSize, _initialLed + i) == null)
                ledId = initialId++;
        }
    }

    #endregion
}