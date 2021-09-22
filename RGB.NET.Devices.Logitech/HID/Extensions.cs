using HidSharp;
using HidSharp.Reports.Encodings;
using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace RGB.NET.Devices.Logitech.HID
{
    public static class Extensions
    {
        public static Span<byte> AsSpan<T>(this ref T val) where T : unmanaged
        {
            Span<T> valSpan = MemoryMarshal.CreateSpan(ref val, 1);
            return MemoryMarshal.Cast<T, byte>(valSpan);
        }

        public static uint GetUsagePage(this HidDevice device)
        {
            try
            {
                var descriptor = device.GetRawReportDescriptor();
                var decodedItems = EncodedItem.DecodeItems(descriptor, 0, descriptor.Length);
                var usefulItems = decodedItems.Where(de => de.TagForLocal == LocalItemTag.Usage && de.TagForGlobal == GlobalItemTag.UsagePage);
                var usagePage = usefulItems.FirstOrDefault(de => de.ItemType == ItemType.Global);
                return usagePage.DataValue;
            }
            catch
            {
                return uint.MaxValue;
            }
        }

        public static uint GetUsage(this HidDevice device)
        {
            try
            {
                var descriptor = device.GetRawReportDescriptor();
                var decodedItems = EncodedItem.DecodeItems(descriptor, 0, descriptor.Length);
                var usefulItems = decodedItems.Where(de => de.TagForLocal == LocalItemTag.Usage && de.TagForGlobal == GlobalItemTag.UsagePage);
                var usage = usefulItems.FirstOrDefault(de => de.ItemType == ItemType.Local);
                return usage.DataValue;
            }
            catch
            {
                return uint.MaxValue;
            }
        }
    }
}
