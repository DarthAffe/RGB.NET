using System;
using AuraServiceLib;
using RGB.NET.Core;

namespace RGB.NET.Devices.Asus;

/// <inheritdoc />
/// <summary>
/// Represents the update-queue performing updates for asus devices.
/// </summary>
public class AsusUpdateQueue : UpdateQueue
{
    #region Properties & Fields

    private readonly IAuraRgbLight[] _lights;

    /// <summary>
    /// The device to be updated.
    /// </summary>
    protected IAuraSyncDevice Device { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="AsusUpdateQueue"/> class.
    /// </summary>
    /// <param name="updateTrigger">The update trigger used by this queue.</param>
    /// <param name="device">The SDK-aura-device this device represents.</param>
    public AsusUpdateQueue(IDeviceUpdateTrigger updateTrigger, IAuraSyncDevice device)
        : base(updateTrigger)
    {
        this.Device = device;

        this._lights = new IAuraRgbLight[device.Lights.Count];
        for (int i = 0; i < device.Lights.Count; i++)
            _lights[i] = device.Lights[i];
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override void Update(in ReadOnlySpan<(object key, Color color)> dataSet)
    {
        try
        {
            if ((Device.Type == (uint)AsusDeviceType.KEYBOARD_RGB) || (Device.Type == (uint)AsusDeviceType.NB_KB_RGB))
            {
                if (Device is not IAuraSyncKeyboard keyboard)
                    return;

                foreach ((object customData, Color value) in dataSet)
                {
                    (AsusLedType ledType, int id) = (AsusKeyboardLedCustomData)customData;
                    if (ledType == AsusLedType.Key)
                    {
                        IAuraRgbLight light = keyboard.Key[(ushort)id];
                        (_, byte r, byte g, byte b) = value.GetRGBBytes();
                        light.Red = r;
                        light.Green = g;
                        light.Blue = b;
                    }
                    else
                    {
                        IAuraRgbLight light = _lights[id];
                        (_, byte r, byte g, byte b) = value.GetRGBBytes();
                        light.Red = r;
                        light.Green = g;
                        light.Blue = b;
                    }
                }
            }
            else
            {
                foreach ((object key, Color value) in dataSet)
                {
                    int index = (int)key;
                    IAuraRgbLight light = _lights[index];

                    (_, byte r, byte g, byte b) = value.GetRGBBytes();
                    light.Red = r;
                    light.Green = g;
                    light.Blue = b;
                }
            }

            Device.Apply();
        }
        catch
        { /* "The server threw an exception." seems to be a thing here ... */
        }
    }

    #endregion
}