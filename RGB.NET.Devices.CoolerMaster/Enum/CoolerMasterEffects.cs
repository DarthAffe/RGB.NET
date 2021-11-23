// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

namespace RGB.NET.Devices.CoolerMaster;

/// <summary>
/// Contains a list of available effects.
/// </summary>
public enum CoolerMasterEffects
{
    FullOn = 0,
    Breath = 1,
    BreathCycle = 2,
    Single = 3,
    Wave = 4,
    Ripple = 5,
    Cross = 6,
    Rain = 7,
    Star = 8,
    Snake = 9,
    Rec = 10,

    Spectrum = 11,
    RapidFire = 12,
    Indicator = 13, //mouse Effect
    FireBall = 14,
    WaterRipple = 15,
    ReactivePunch = 16,
    Snowing = 17,
    HeartBeat = 18,
    ReactiveTornade = 19,

    Multi1 = 0xE0,
    Multi2 = 0xE1,
    Multi3 = 0xE2,
    Multi4 = 0xE3,
    Off = 0xFE
}