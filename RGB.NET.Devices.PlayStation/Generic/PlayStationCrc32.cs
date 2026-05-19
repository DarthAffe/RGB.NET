namespace RGB.NET.Devices.PlayStation;

// Sony's BT main output reports for both DualShock 4 (0x11) and DualSense (0x31)
// end in a little-endian CRC-32 over the report contents, prefixed with a magic
// seed byte 0xA2 ("output report" tag — 0xA1 input, 0xA3 feature). Algorithm is
// standard CRC-32/zlib (poly 0xEDB88320, init 0xFFFFFFFF, reflected, XOR-out
// 0xFFFFFFFF). Mirrors crc32_le in the Linux hid-playstation driver.
//
// Newer PS5 firmware silently drops malformed reports — bad CRC means the
// lightbar simply doesn't change, no transport-level error. Test on real
// hardware once any byte layout shifts.
internal static class PlayStationCrc32
{
    // Output-report seed prepended to the CRC input. 0xA1=input, 0xA2=output, 0xA3=feature.
    public const byte OutputReportSeed = 0xA2;

    private static readonly uint[] Table = BuildTable();

    private static uint[] BuildTable()
    {
        const uint poly = 0xEDB88320u;
        uint[] table = new uint[256];
        for (uint i = 0; i < 256; i++)
        {
            uint c = i;
            for (int j = 0; j < 8; j++)
                c = (c & 1) != 0 ? (poly ^ (c >> 1)) : (c >> 1);
            table[i] = c;
        }
        return table;
    }

    // Compute the CRC32 to write into the last 4 bytes of a BT output report.
    // `data` is the buffer including the report ID at index 0; `payloadLength`
    // is the count of bytes that participate in the CRC (everything before the
    // 4-byte CRC tail, i.e. typically buffer.Length - 4).
    public static uint ComputeOutputCrc(byte[] data, int payloadLength)
    {
        uint crc = 0xFFFFFFFFu;
        // Seed byte first (matches Linux: crc32_le(0xFFFFFFFF, &seed, 1))
        crc = (crc >> 8) ^ Table[(crc ^ OutputReportSeed) & 0xFF];
        for (int i = 0; i < payloadLength; i++)
            crc = (crc >> 8) ^ Table[(crc ^ data[i]) & 0xFF];
        return ~crc;
    }

    // Convenience: compute and write the CRC into the last 4 bytes of `buffer`.
    // Caller is responsible for sizing `buffer` correctly (DS4 BT = 78, DS5 BT = 78).
    public static void AppendOutputCrc(byte[] buffer)
    {
        uint crc = ComputeOutputCrc(buffer, buffer.Length - 4);
        int o = buffer.Length - 4;
        buffer[o + 0] = (byte)(crc & 0xFF);
        buffer[o + 1] = (byte)((crc >> 8) & 0xFF);
        buffer[o + 2] = (byte)((crc >> 16) & 0xFF);
        buffer[o + 3] = (byte)((crc >> 24) & 0xFF);
    }
}
