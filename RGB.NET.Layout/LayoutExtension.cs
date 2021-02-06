using System;
using System.Collections.Generic;
using System.Linq;
using RGB.NET.Core;

namespace RGB.NET.Layout
{
    public static class LayoutExtension
    {
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
}
