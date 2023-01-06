using System;
using System.Collections.Generic;
using System.Linq;
using RGB.NET.Core;

namespace RGB.NET.Layout;

/// <summary>
/// Offers some extensions and helper-methods for layout related things.
/// </summary>
public static class LayoutExtension
{
    /// <summary>
    /// Applies the specified layout to the specified device.
    /// </summary>
    /// <param name="layout">The layout to apply.</param>
    /// <param name="device">The device to apply the layout to.</param>
    /// <param name="createMissingLeds">Indicates if LEDs that are in the layout but not on the device should be created.</param>
    /// <param name="removeExcessiveLeds">Indicates if LEDS that are on the device but not in the layout should be removed.</param>
    public static void ApplyTo(this IDeviceLayout layout, IRGBDevice device, bool createMissingLeds = false, bool removeExcessiveLeds = false)
    {
        device.Size = new Size(layout.Width, layout.Height);
        device.DeviceInfo.LayoutMetadata = layout.CustomData;

        HashSet<LedId> ledIds = new();
        foreach (ILedLayout layoutLed in layout.Leds)
        {
            if (Enum.TryParse(layoutLed.Id, true, out LedId ledId))
            {
                ledIds.Add(ledId);

                Led? led = device[ledId];
                if ((led == null) && createMissingLeds)
                    led = device.AddLed(ledId, new Point(), new Size());

                if (led != null)
                {
                    led.Location = new Point(layoutLed.X, layoutLed.Y);
                    led.Size = new Size(layoutLed.Width, layoutLed.Height);
                    led.Shape = layoutLed.Shape;
                    led.ShapeData = layoutLed.ShapeData;
                    led.LayoutMetadata = layoutLed.CustomData;
                }
            }
        }

        if (removeExcessiveLeds)
        {
            List<LedId> ledsToRemove = device.Select(led => led.Id).Where(id => !ledIds.Contains(id)).ToList();
            foreach (LedId led in ledsToRemove)
                device.RemoveLed(led);
        }
    }
}