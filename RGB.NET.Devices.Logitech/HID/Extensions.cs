using HidSharp;
using HidSharp.Reports.Encodings;
using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace RGB.NET.Devices.Logitech.HID;

internal static class Extensions
{
    internal static Span<byte> AsSpan<T>(this ref T val)
        where T : unmanaged
    {
        Span<T> valSpan = MemoryMarshal.CreateSpan(ref val, 1);
        return MemoryMarshal.Cast<T, byte>(valSpan);
    }

    internal static uint GetUsagePage(this HidDevice device)
    {
        try
        {
            return device.GetItemByType(ItemType.Global)?.DataValue ?? uint.MaxValue;
        }
        catch
        {
            return uint.MaxValue;
        }
    }

    internal static uint GetUsage(this HidDevice device)
    {
        try
        {
            return device.GetItemByType(ItemType.Local)?.DataValue ?? uint.MaxValue;
        }
        catch
        {
            return uint.MaxValue;
        }
    }

    private static EncodedItem? GetItemByType(this HidDevice device, ItemType itemType)
    {
        byte[] descriptor = device.GetRawReportDescriptor();
        return EncodedItem.DecodeItems(descriptor, 0, descriptor.Length)
                          .Where(de => (de.TagForLocal == LocalItemTag.Usage) && (de.TagForGlobal == GlobalItemTag.UsagePage))
                          .FirstOrDefault(de => de.ItemType == itemType);
    }
}