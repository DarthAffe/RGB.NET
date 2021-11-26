using System;
using System.Linq;

namespace RGB.NET.Devices.DMX.E131;

internal static class E131DataPacketExtension
{
    #region Methods

    // ReSharper disable once InconsistentNaming
    internal static void SetCID(this byte[] data, byte[] cid) => Array.Copy(cid, 0, data, 22, 16);

    internal static void SetSequenceNumber(this byte[] data, byte sequenceNumber) => data[111] = sequenceNumber;

    internal static void SetUniverse(this byte[] data, short universe) => Array.Copy(ToBigEndian(BitConverter.GetBytes(universe)), 0, data, 113, 2);

    internal static void ClearColors(this byte[] data) => Array.Clear(data, 126, 512);

    internal static void SetChannel(this byte[] data, int channel, byte value) => data[126 + channel] = value;

    private static byte[] ToBigEndian(byte[] data) => BitConverter.IsLittleEndian ? data.Reverse().ToArray() : data;

    #endregion
}