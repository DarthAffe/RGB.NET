// ReSharper disable InconsistentNaming
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RGB.NET.Devices.WLED;

public class WledInfo
{
    [JsonPropertyName("ver")]
    public string Version { get; set; } = "";

    [JsonPropertyName("vid")]
    public uint BuildId { get; set; }

    [JsonPropertyName("leds")]
    public LedsInfo Leds { get; set; } = new();

    [JsonPropertyName("str")]
    public bool SyncReceive { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("udpport")]
    public ushort UDPPort { get; set; }

    [JsonPropertyName("live")]
    public bool IsLive { get; set; }

    [JsonPropertyName("liveseg")]
    public short MainSegment { get; set; }

    [JsonPropertyName("lm")]
    public string RealtimeDataSource { get; set; } = "";

    [JsonPropertyName("lip")]
    public string RealtimeDataSourceIpAddress { get; set; } = "";

    [JsonPropertyName("ws")]
    public byte ConnectedWebSocketCount { get; set; }

    [JsonPropertyName("fxcount")]
    public byte EffectCount { get; set; }

    [JsonPropertyName("palcount")]
    public short PaletteCount { get; set; }

    [JsonPropertyName("maps")]
    public List<Map> LedMaps { get; set; } = [];

    [JsonPropertyName("wifi")]
    public Wifi WifiInfo { get; set; } = new();

    [JsonPropertyName("fs")]
    public Fs FilesystemInfo { get; set; } = new();

    [JsonPropertyName("ndc")]
    public short DiscoveredDeviceCount { get; set; }

    [JsonPropertyName("arch")]
    public string PlatformName { get; set; } = "";

    [JsonPropertyName("core")]
    public string ArduinoCoreVersion { get; set; } = "";

    [JsonPropertyName("freeheap")]
    public uint FreeHeap { get; set; }

    [JsonPropertyName("uptime")]
    public uint Uptime { get; set; }

    [JsonPropertyName("time")]
    public string Time { get; set; } = "";

    [JsonPropertyName("brand")]
    public string Brand { get; set; } = "";

    [JsonPropertyName("product")]
    public string Product { get; set; } = "";

    [JsonPropertyName("ip")]
    public string IpAddress { get; set; } = "";


    public class LedsInfo
    {
        [JsonPropertyName("count")]
        public ushort Count { get; set; }

        [JsonPropertyName("pwr")]
        public ushort PowerUsage { get; set; }

        [JsonPropertyName("fps")]
        public ushort RefreshRate { get; set; }

        [JsonPropertyName("maxpwr")]
        public ushort MaxPower { get; set; }

        [JsonPropertyName("maxseg")]
        public byte MaxSegments { get; set; }

        [JsonPropertyName("matrix")]
        public MatrixDims? Matrix { get; set; }

        [JsonPropertyName("seglc")]
        public List<byte> SegmentLightCapabilities { get; set; } = [];

        [JsonPropertyName("lc")]
        public byte CombinedSegmentLightCapabilities { get; set; }
    }

    public class Map
    {
        [JsonPropertyName("id")]
        public byte Id { get; set; }

        [JsonPropertyName("n")]
        public string Name { get; set; } = "";
    }

    public class Wifi
    {
        [JsonPropertyName("bssid")]
        public string BSSID { get; set; } = "";

        [JsonPropertyName("rssi")]
        public long RSSI { get; set; }

        [JsonPropertyName("signal")]
        public byte SignalQuality { get; set; }

        [JsonPropertyName("channel")]
        public int Channel { get; set; }
    }

    public class Fs
    {
        [JsonPropertyName("u")]
        public uint UsedSpace { get; set; }

        [JsonPropertyName("t")]
        public uint TotalSpace { get; set; }

        [JsonPropertyName("pmt")]
        public ulong LastPresetsJsonModificationTime { get; set; }
    }

    public class MatrixDims
    {
        [JsonPropertyName("w")]
        public ushort Width { get; set; }

        [JsonPropertyName("h")]
        public ushort Height { get; set; }
    }
}
