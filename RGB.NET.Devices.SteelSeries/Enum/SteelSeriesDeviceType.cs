#pragma warning disable 1591

namespace RGB.NET.Devices.SteelSeries;

// DarthAffe 09.07.2020: Review the LISP-Handler in SteelSeriesSDK after adding new device-types! They need to be initialized.
public enum SteelSeriesDeviceType
{
    [APIName("rgb-per-key-zones")]
    PerKey,

    [APIName("rgb-1-zone")]
    OneZone,

    [APIName("rgb-2-zone")]
    TwoZone,

    [APIName("rgb-3-zone")]
    ThreeZone,

    [APIName("rgb-4-zone")]
    FourZone,

    [APIName("rgb-5-zone")]
    FiveZone,

    [APIName("rgb-6-zone")]
    SixZone,

    [APIName("rgb-7-zone")]
    SevenZone,

    [APIName("rgb-8-zone")]
    EightZone,

    [APIName("rgb-12-zone")]
    TwelveZone,

    [APIName("rgb-17-zone")]
    SeventeenZone,

    [APIName("rgb-24-zone")]
    TwentyfourZone,

    [APIName("rgb-103-zone")]
    OneHundredAndThreeZone
}