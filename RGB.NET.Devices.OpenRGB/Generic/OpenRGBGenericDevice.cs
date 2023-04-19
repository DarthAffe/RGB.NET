using OpenRGB.NET;
using RGB.NET.Core;

namespace RGB.NET.Devices.OpenRGB;

/// <inheritdoc />
public sealed class OpenRGBGenericDevice : AbstractOpenRGBDevice<OpenRGBDeviceInfo>
{
    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="OpenRGBGenericDevice"/> class.
    /// </summary>
    /// <param name="info">Generic information for the device.</param>
    /// <param name="updateQueue">The queue used to update the device.</param>
    public OpenRGBGenericDevice(OpenRGBDeviceInfo info, IUpdateQueue updateQueue)
        : base(info, updateQueue)
    {
        InitializeLayout();
    }

    #endregion

    #region Methods

    /// <summary>
    /// Initializes the LEDs of the device based on the data provided by the SDK.
    /// </summary>
    private void InitializeLayout()
    {
        LedId initial = Helper.GetInitialLedIdForDeviceType(DeviceInfo.DeviceType);

        int y = 0;
        Size ledSize = new(19);
        int zoneLedIndex = 0;
        const int LED_SPACING = 20;

        foreach (Zone? zone in DeviceInfo.OpenRGBDevice.Zones)
        {
            if (zone.Type == ZoneType.Matrix)
            {
                for (int row = 0; row < zone.MatrixMap!.Height; row++)
                {
                    for (int column = 0; column < zone.MatrixMap!.Width; column++)
                    {
                        uint index = zone.MatrixMap!.Matrix[row, column];

                        //will be max value if the position does not have an associated key
                        if (index == uint.MaxValue)
                            continue;

                        LedId ledId = LedMappings.DEFAULT.TryGetValue(DeviceInfo.OpenRGBDevice.Leds[zoneLedIndex + index].Name, out LedId id)
                                          ? id
                                          : initial++;

                        //HACK: doing this because some different Led Names are mapped to the same LedId
                        //for example, "Enter" and "ISO Enter".
                        //this way, at least they'll be controllable as CustomX
                        while (AddLed(ledId, new Point(LED_SPACING * column, y + (LED_SPACING * row)), ledSize, zoneLedIndex + (int)index) == null)
                            ledId = initial++;
                    }
                }
                y += (int)(zone.MatrixMap!.Height * LED_SPACING);
            }
            else
            {
                for (int i = 0; i < zone.LedCount; i++)
                {
                    LedId ledId = initial++;

                    while (AddLed(ledId, new Point(LED_SPACING * i, y), ledSize, zoneLedIndex + i) == null)
                        ledId = initial++;
                }
            }

            //we'll just set each zone in its own row for now,
            //with each led for that zone being horizontally distributed
            y += LED_SPACING;
            zoneLedIndex += (int)zone.LedCount;
        }
    }

    #endregion
}
