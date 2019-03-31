namespace RGB.NET.Devices.SteelSeries
{
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
        FiveZone
    }
}
