using System.Diagnostics;
using RGB.NET.Core;
using RGB.NET.Presets.Decorators;
using RGB.NET.Presets.Textures;
using RGB.NET.Presets.Textures.Gradients;

namespace RGB.NET.Sandbox;

internal class Program
{
    public static void Main(string[] args)
    {
        long before = Stopwatch.GetTimestamp();

        AbstractRGBDeviceProvider[] deviceProviders = [
            Devices.Wooting.WootingDeviceProvider.Instance,
            //Add more device providers here
        ];

        RGBSurface surface = new();
        TimerUpdateTrigger timer = new() { UpdateFrequency = 1d / 60 };

        surface.RegisterUpdateTrigger(timer);

        surface.Exception += a => { Console.WriteLine(a.Exception.Message); };

        foreach (AbstractRGBDeviceProvider dp in deviceProviders)
        {
            dp.Exception += (a, b) => { Console.WriteLine(b.Exception.Message); };
            dp.Initialize();
            foreach (IRGBDevice device in dp.Devices)
            {
                Console.WriteLine(device.DeviceInfo.DeviceName);
            }

            surface.Attach(dp.Devices);
        }

        TimeSpan after = Stopwatch.GetElapsedTime(before);
        Console.WriteLine($"Initialized in {after.TotalMilliseconds} ms");

        ILedGroup group = new ListLedGroup(surface, surface.Leds);
        IGradient gradient = new RainbowGradient();
        gradient.AddDecorator(new MoveGradientDecorator(surface, 500));
        group.Brush = new TextureBrush(new LinearGradientTexture(new Size(1, 1), gradient));

        Console.ReadLine();

        foreach (AbstractRGBDeviceProvider dp in deviceProviders)
        {
            dp.Dispose();
        }

        surface.Dispose();
    }
}
