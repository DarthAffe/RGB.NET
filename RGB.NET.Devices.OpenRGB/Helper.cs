using System;
using System.Security.Cryptography;
using System.Text;
using OpenRGB.NET;
using RGB.NET.Core;
using OpenRGBDevice = OpenRGB.NET.Device;

namespace RGB.NET.Devices.OpenRGB;

internal static class Helper
{
    public static LedId GetInitialLedIdForDeviceType(RGBDeviceType type)
        => type switch
        {
            RGBDeviceType.Mouse => LedId.Mouse1,
            RGBDeviceType.Headset => LedId.Headset1,
            RGBDeviceType.Mousepad => LedId.Mousepad1,
            RGBDeviceType.LedStripe => LedId.LedStripe1,
            RGBDeviceType.LedMatrix => LedId.LedMatrix1,
            RGBDeviceType.Mainboard => LedId.Mainboard1,
            RGBDeviceType.GraphicsCard => LedId.GraphicsCard1,
            RGBDeviceType.DRAM => LedId.DRAM1,
            RGBDeviceType.HeadsetStand => LedId.HeadsetStand1,
            RGBDeviceType.Keypad => LedId.Keypad1,
            RGBDeviceType.Fan => LedId.Fan1,
            RGBDeviceType.Speaker => LedId.Speaker1,
            RGBDeviceType.Cooler => LedId.Cooler1,
            RGBDeviceType.Keyboard => LedId.Keyboard_Custom1,
            _ => LedId.Custom1
        };

    public static RGBDeviceType GetRgbNetDeviceType(DeviceType type)
        => type switch
        {
            DeviceType.Motherboard => RGBDeviceType.Mainboard,
            DeviceType.Dram => RGBDeviceType.DRAM,
            DeviceType.Gpu => RGBDeviceType.GraphicsCard,
            DeviceType.Cooler => RGBDeviceType.Cooler,
            DeviceType.Ledstrip => RGBDeviceType.LedStripe,
            DeviceType.Keyboard => RGBDeviceType.Keyboard,
            DeviceType.Mouse => RGBDeviceType.Mouse,
            DeviceType.Mousemat => RGBDeviceType.Mousepad,
            DeviceType.Headset => RGBDeviceType.Headset,
            DeviceType.Speaker => RGBDeviceType.Speaker,
            DeviceType.HeadsetStand => RGBDeviceType.HeadsetStand,
            _ => RGBDeviceType.Unknown
        };

    public static LedId GetInitialLedIdForDeviceType(DeviceType type)
        => GetInitialLedIdForDeviceType(GetRgbNetDeviceType(type));

    public static string GetVendorName(OpenRGBDevice openRGBDevice) => string.IsNullOrWhiteSpace(openRGBDevice.Vendor)
                                                                           ? "OpenRGB"
                                                                           : openRGBDevice.Vendor;

    public static string GetModelName(OpenRGBDevice openRGBDevice) => string.IsNullOrWhiteSpace(openRGBDevice.Vendor)
                                                                          ? openRGBDevice.Name
                                                                          : openRGBDevice.Name.Replace(openRGBDevice.Vendor, "").Trim();
    
    internal static string HashAndShorten(string input)
    {
        using SHA256 sha256Hash = SHA256.Create();
        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
        // Take the first 4 bytes of the hash
        byte[] shortenedBytes = new byte[4];
        Array.Copy(bytes, shortenedBytes, 4);
        // Convert the bytes to a string
        StringBuilder shortenedHash = new();
        foreach (byte b in shortenedBytes)
        {
            shortenedHash.Append(b.ToString("X2"));
        }
        return shortenedHash.ToString();
    }
}
