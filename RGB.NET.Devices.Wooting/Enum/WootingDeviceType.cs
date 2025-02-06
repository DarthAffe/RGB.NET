﻿// ReSharper disable InconsistentNaming

namespace RGB.NET.Devices.Wooting.Enum;

/// <summary>
/// Represents the type of a wooting device
/// </summary>
public enum WootingDeviceType
{
    /// <summary>
    /// 10 Keyless Keyboard. E.g. Wooting One
    /// </summary>
    KeyboardTKL = 1,

    /// <summary>
    /// Full Size keyboard. E.g. Wooting Two
    /// </summary>
    Keyboard = 2,

    /// <summary>
    /// 60 percent keyboard, E.g. Wooting 60HE
    /// </summary>
    KeyboardSixtyPercent = 3,
    
    /// <summary>
    /// Three key keypad. E.g. Wooting Uwu
    /// </summary>
    Keypad3Keys = 4,
    
    /// <summary>
    /// 80 percent keyboard. E.g. Wooting 80HE
    /// </summary>
    KeyboardEightyPercent = 5,
}