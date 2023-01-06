[RGB.NET](https://github.com/DarthAffe/RGB.NET) Core-Package.

Required to use RGB.NET

## Getting Started
```csharp
// Create a surface - this is where all devices belongs too
RGBSurface surface = new RGBSurface();

// Load your devices - check out the RGB.NET.Devices-packages for more information
// TODO: Load device-providers

// Automatically align devices to not overlap - you can ofc also move them by hand
surface.AlignDevices();

// Register an update-trigger
surface.RegisterUpdateTrigger(new TimerUpdateTrigger());
```

## Basis Rendering
```csharp
// Create a led-group containing all leds on the surface
ILedGroup allLeds = new ListLedGroup(surface, surface.Leds);

// Create a rainbow gradient
RainbowGradient rainbow = new RainbowGradient();

// Animate the gradient to steadily move
rainbow.AddDecorator(new MoveGradientDecorator(surface));

// Create a texture rendering that gradient
ITexture texture = new ConicalGradientTexture(new Size(10, 10), rainbow);

// Create a brush rendering the texture and assign it to the group
allLeds.Brush = new TextureBrush(texture);
```