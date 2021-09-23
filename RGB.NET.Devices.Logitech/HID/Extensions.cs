using HidSharp;
using HidSharp.Reports.Encodings;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Collections.Generic;

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
                byte[]? descriptor = device.GetRawReportDescriptor();
                IEnumerable<EncodedItem>? decodedItems = EncodedItem.DecodeItems(descriptor, 0, descriptor.Length);
                IEnumerable<EncodedItem>? usefulItems = decodedItems.Where(de => de.TagForLocal == LocalItemTag.Usage && de.TagForGlobal == GlobalItemTag.UsagePage);
                EncodedItem? usagePage = usefulItems.FirstOrDefault(de => de.ItemType == ItemType.Global);
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
                byte[]? descriptor = device.GetRawReportDescriptor();
                IEnumerable<EncodedItem>? decodedItems = EncodedItem.DecodeItems(descriptor, 0, descriptor.Length);
                IEnumerable<EncodedItem>? usefulItems = decodedItems.Where(de => de.TagForLocal == LocalItemTag.Usage && de.TagForGlobal == GlobalItemTag.UsagePage);
                EncodedItem? usage = usefulItems.FirstOrDefault(de => de.ItemType == ItemType.Local);
                return usage.DataValue;
            }
            catch
            {
                return uint.MaxValue;
            }
        }
    }
}
