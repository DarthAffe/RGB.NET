// ReSharper disable InconsistentNaming

namespace RGB.NET.Devices.Asus;

internal enum AsusDeviceType : uint
{
    ALL = 0,
    MB_RGB = 0x10000,
    MB_ADDRESABLE = 0x11000,
    DESKTOP_RGB = 0x12000,
    VGA_RGB = 0x20000,
    DISPLAY_RGB = 0x30000,
    HEADSET_RGB = 0x40000,
    MICROPHONE_RGB = 0x50000,
    EXTERNAL_HARD_DRIVER_RGB = 0x60000,
    EXTERNAL_BLUE_RAY_RGB = 0x61000,
    DRAM_RGB = 0x70000,
    KEYBOARD_RGB = 0x80000,
    NB_KB_RGB = 0x81000,
    NB_KB_4ZONE_RGB = 0x81001,
    MOUSE_RGB = 0x90000,
    CHASSIS_RGB = 0xB0000,
    PROJECTOR_RGB = 0xC0000
}