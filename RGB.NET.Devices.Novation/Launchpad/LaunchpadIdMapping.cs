using System.Collections.Generic;
using RGB.NET.Core;

namespace RGB.NET.Devices.Novation;

internal static class LaunchpadIdMapping
{
    internal static readonly Dictionary<LedId, (byte mode, byte id, int x, int y)> LEGACY = new()
                                                                                            {
                                                                                                { LedId.Invalid, (0x00, 0xFF, 8, 0) },

                                                                                                { LedId.LedMatrix1, (0x90, 0x00, 0, 1) },
                                                                                                { LedId.LedMatrix2, (0x90, 0x01, 1, 1) },
                                                                                                { LedId.LedMatrix3, (0x90, 0x02, 2, 1) },
                                                                                                { LedId.LedMatrix4, (0x90, 0x03, 3, 1) },
                                                                                                { LedId.LedMatrix5, (0x90, 0x04, 4, 1) },
                                                                                                { LedId.LedMatrix6, (0x90, 0x05, 5, 1) },
                                                                                                { LedId.LedMatrix7, (0x90, 0x06, 6, 1) },
                                                                                                { LedId.LedMatrix8, (0x90, 0x07, 7, 1) },
                                                                                                { LedId.LedMatrix9, (0x90, 0x10, 0, 2) },
                                                                                                { LedId.LedMatrix10, (0x90, 0x11, 1, 2) },
                                                                                                { LedId.LedMatrix11, (0x90, 0x12, 2, 2) },
                                                                                                { LedId.LedMatrix12, (0x90, 0x13, 3, 2) },
                                                                                                { LedId.LedMatrix13, (0x90, 0x14, 4, 2) },
                                                                                                { LedId.LedMatrix14, (0x90, 0x15, 5, 2) },
                                                                                                { LedId.LedMatrix15, (0x90, 0x16, 6, 2) },
                                                                                                { LedId.LedMatrix16, (0x90, 0x17, 7, 2) },
                                                                                                { LedId.LedMatrix17, (0x90, 0x20, 0, 3) },
                                                                                                { LedId.LedMatrix18, (0x90, 0x21, 1, 3) },
                                                                                                { LedId.LedMatrix19, (0x90, 0x22, 2, 3) },
                                                                                                { LedId.LedMatrix20, (0x90, 0x23, 3, 3) },
                                                                                                { LedId.LedMatrix21, (0x90, 0x24, 4, 3) },
                                                                                                { LedId.LedMatrix22, (0x90, 0x25, 5, 3) },
                                                                                                { LedId.LedMatrix23, (0x90, 0x26, 6, 3) },
                                                                                                { LedId.LedMatrix24, (0x90, 0x27, 7, 3) },
                                                                                                { LedId.LedMatrix25, (0x90, 0x30, 0, 4) },
                                                                                                { LedId.LedMatrix26, (0x90, 0x31, 1, 4) },
                                                                                                { LedId.LedMatrix27, (0x90, 0x32, 2, 4) },
                                                                                                { LedId.LedMatrix28, (0x90, 0x33, 3, 4) },
                                                                                                { LedId.LedMatrix29, (0x90, 0x34, 4, 4) },
                                                                                                { LedId.LedMatrix30, (0x90, 0x35, 5, 4) },
                                                                                                { LedId.LedMatrix31, (0x90, 0x36, 6, 4) },
                                                                                                { LedId.LedMatrix32, (0x90, 0x37, 7, 4) },
                                                                                                { LedId.LedMatrix33, (0x90, 0x40, 0, 5) },
                                                                                                { LedId.LedMatrix34, (0x90, 0x41, 1, 5) },
                                                                                                { LedId.LedMatrix35, (0x90, 0x42, 2, 5) },
                                                                                                { LedId.LedMatrix36, (0x90, 0x43, 3, 5) },
                                                                                                { LedId.LedMatrix37, (0x90, 0x44, 4, 5) },
                                                                                                { LedId.LedMatrix38, (0x90, 0x45, 5, 5) },
                                                                                                { LedId.LedMatrix39, (0x90, 0x46, 6, 5) },
                                                                                                { LedId.LedMatrix40, (0x90, 0x47, 7, 5) },
                                                                                                { LedId.LedMatrix41, (0x90, 0x50, 0, 6) },
                                                                                                { LedId.LedMatrix42, (0x90, 0x51, 1, 6) },
                                                                                                { LedId.LedMatrix43, (0x90, 0x52, 2, 6) },
                                                                                                { LedId.LedMatrix44, (0x90, 0x53, 3, 6) },
                                                                                                { LedId.LedMatrix45, (0x90, 0x54, 4, 6) },
                                                                                                { LedId.LedMatrix46, (0x90, 0x55, 5, 6) },
                                                                                                { LedId.LedMatrix47, (0x90, 0x56, 6, 6) },
                                                                                                { LedId.LedMatrix48, (0x90, 0x57, 7, 6) },
                                                                                                { LedId.LedMatrix49, (0x90, 0x60, 0, 7) },
                                                                                                { LedId.LedMatrix50, (0x90, 0x61, 1, 7) },
                                                                                                { LedId.LedMatrix51, (0x90, 0x62, 2, 7) },
                                                                                                { LedId.LedMatrix52, (0x90, 0x63, 3, 7) },
                                                                                                { LedId.LedMatrix53, (0x90, 0x64, 4, 7) },
                                                                                                { LedId.LedMatrix54, (0x90, 0x65, 5, 7) },
                                                                                                { LedId.LedMatrix55, (0x90, 0x66, 6, 7) },
                                                                                                { LedId.LedMatrix56, (0x90, 0x67, 7, 7) },
                                                                                                { LedId.LedMatrix57, (0x90, 0x70, 0, 8) },
                                                                                                { LedId.LedMatrix58, (0x90, 0x71, 1, 8) },
                                                                                                { LedId.LedMatrix59, (0x90, 0x72, 2, 8) },
                                                                                                { LedId.LedMatrix60, (0x90, 0x73, 3, 8) },
                                                                                                { LedId.LedMatrix61, (0x90, 0x74, 4, 8) },
                                                                                                { LedId.LedMatrix62, (0x90, 0x75, 5, 8) },
                                                                                                { LedId.LedMatrix63, (0x90, 0x76, 6, 8) },
                                                                                                { LedId.LedMatrix64, (0x90, 0x77, 7, 8) },

                                                                                                { LedId.Custom1, (0xB0, 0x68, 0, 0) }, // Up
                                                                                                { LedId.Custom2, (0xB0, 0x69, 1, 0) }, // Down
                                                                                                { LedId.Custom3, (0xB0, 0x6A, 2, 0) }, // Left
                                                                                                { LedId.Custom4, (0xB0, 0x6B, 3, 0) }, // Right
                                                                                                { LedId.Custom5, (0xB0, 0x6C, 4, 0) }, // Session
                                                                                                { LedId.Custom6, (0xB0, 0x6D, 5, 0) }, // User 1
                                                                                                { LedId.Custom7, (0xB0, 0x6E, 6, 0) }, // User 2
                                                                                                { LedId.Custom8, (0xB0, 0x6F, 7, 0) }, // Mix

                                                                                                { LedId.Custom9, (0x90, 0x08, 8, 1) }, //Scene1
                                                                                                { LedId.Custom10, (0x90, 0x18, 8, 2) }, //Scene2
                                                                                                { LedId.Custom11, (0x90, 0x28, 8, 3) }, //Scene3
                                                                                                { LedId.Custom12, (0x90, 0x38, 8, 4) }, //Scene4
                                                                                                { LedId.Custom13, (0x90, 0x48, 8, 5) }, //Scene5
                                                                                                { LedId.Custom14, (0x90, 0x58, 8, 6) }, //Scene6
                                                                                                { LedId.Custom15, (0x90, 0x68, 8, 7) }, //Scene7
                                                                                                { LedId.Custom16, (0x90, 0x78, 8, 8) }, //Scene8
                                                                                            };

    internal static readonly Dictionary<LedId, (byte mode, byte id, int x, int y)> CURRENT = new()
        {
            { LedId.Invalid, (0x00, 0xFF, 8, 0) },

            { LedId.LedMatrix1, (0x90, 81, 0, 1) },
            { LedId.LedMatrix2, (0x90, 82, 1, 1) },
            { LedId.LedMatrix3, (0x90, 83, 2, 1) },
            { LedId.LedMatrix4, (0x90, 84, 3, 1) },
            { LedId.LedMatrix5, (0x90, 85, 4, 1) },
            { LedId.LedMatrix6, (0x90, 86, 5, 1) },
            { LedId.LedMatrix7, (0x90, 87, 6, 1) },
            { LedId.LedMatrix8, (0x90, 88, 7, 1) },
            { LedId.LedMatrix9, (0x90, 71, 0, 2) },
            { LedId.LedMatrix10, (0x90, 72, 1, 2) },
            { LedId.LedMatrix11, (0x90, 73, 2, 2) },
            { LedId.LedMatrix12, (0x90, 74, 3, 2) },
            { LedId.LedMatrix13, (0x90, 75, 4, 2) },
            { LedId.LedMatrix14, (0x90, 76, 5, 2) },
            { LedId.LedMatrix15, (0x90, 77, 6, 2) },
            { LedId.LedMatrix16, (0x90, 78, 7, 2) },
            { LedId.LedMatrix17, (0x90, 61, 0, 3) },
            { LedId.LedMatrix18, (0x90, 62, 1, 3) },
            { LedId.LedMatrix19, (0x90, 63, 2, 3) },
            { LedId.LedMatrix20, (0x90, 64, 3, 3) },
            { LedId.LedMatrix21, (0x90, 65, 4, 3) },
            { LedId.LedMatrix22, (0x90, 66, 5, 3) },
            { LedId.LedMatrix23, (0x90, 67, 6, 3) },
            { LedId.LedMatrix24, (0x90, 68, 7, 3) },
            { LedId.LedMatrix25, (0x90, 51, 0, 4) },
            { LedId.LedMatrix26, (0x90, 52, 1, 4) },
            { LedId.LedMatrix27, (0x90, 53, 2, 4) },
            { LedId.LedMatrix28, (0x90, 54, 3, 4) },
            { LedId.LedMatrix29, (0x90, 55, 4, 4) },
            { LedId.LedMatrix30, (0x90, 56, 5, 4) },
            { LedId.LedMatrix31, (0x90, 57, 6, 4) },
            { LedId.LedMatrix32, (0x90, 58, 7, 4) },
            { LedId.LedMatrix33, (0x90, 41, 0, 5) },
            { LedId.LedMatrix34, (0x90, 42, 1, 5) },
            { LedId.LedMatrix35, (0x90, 43, 2, 5) },
            { LedId.LedMatrix36, (0x90, 44, 3, 5) },
            { LedId.LedMatrix37, (0x90, 45, 4, 5) },
            { LedId.LedMatrix38, (0x90, 46, 5, 5) },
            { LedId.LedMatrix39, (0x90, 47, 6, 5) },
            { LedId.LedMatrix40, (0x90, 48, 7, 5) },
            { LedId.LedMatrix41, (0x90, 31, 0, 6) },
            { LedId.LedMatrix42, (0x90, 32, 1, 6) },
            { LedId.LedMatrix43, (0x90, 33, 2, 6) },
            { LedId.LedMatrix44, (0x90, 34, 3, 6) },
            { LedId.LedMatrix45, (0x90, 35, 4, 6) },
            { LedId.LedMatrix46, (0x90, 36, 5, 6) },
            { LedId.LedMatrix47, (0x90, 37, 6, 6) },
            { LedId.LedMatrix48, (0x90, 38, 7, 6) },
            { LedId.LedMatrix49, (0x90, 21, 0, 7) },
            { LedId.LedMatrix50, (0x90, 22, 1, 7) },
            { LedId.LedMatrix51, (0x90, 23, 2, 7) },
            { LedId.LedMatrix52, (0x90, 24, 3, 7) },
            { LedId.LedMatrix53, (0x90, 25, 4, 7) },
            { LedId.LedMatrix54, (0x90, 26, 5, 7) },
            { LedId.LedMatrix55, (0x90, 27, 6, 7) },
            { LedId.LedMatrix56, (0x90, 28, 7, 7) },
            { LedId.LedMatrix57, (0x90, 11, 0, 8) },
            { LedId.LedMatrix58, (0x90, 12, 1, 8) },
            { LedId.LedMatrix59, (0x90, 13, 2, 8) },
            { LedId.LedMatrix60, (0x90, 14, 3, 8) },
            { LedId.LedMatrix61, (0x90, 15, 4, 8) },
            { LedId.LedMatrix62, (0x90, 16, 5, 8) },
            { LedId.LedMatrix63, (0x90, 17, 6, 8) },
            { LedId.LedMatrix64, (0x90, 18, 7, 8) },

            { LedId.Custom1, (0xB0, 104, 0, 0) }, // Up
            { LedId.Custom2, (0xB0, 105, 1, 0) }, // Down
            { LedId.Custom3, (0xB0, 106, 2, 0) }, // Left
            { LedId.Custom4, (0xB0, 107, 3, 0) }, // Right
            { LedId.Custom5, (0xB0, 108, 4, 0) }, // Session
            { LedId.Custom6, (0xB0, 109, 5, 0) }, // User 1
            { LedId.Custom7, (0xB0, 110, 6, 0) }, // User 2
            { LedId.Custom8, (0xB0, 111, 7, 0) }, // Mix

            { LedId.Custom9, (0x90, 89, 8, 1) }, //Scene1
            { LedId.Custom10, (0x90, 79, 8, 2) }, //Scene2
            { LedId.Custom11, (0x90, 69, 8, 3) }, //Scene3
            { LedId.Custom12, (0x90, 59, 8, 4) }, //Scene4
            { LedId.Custom13, (0x90, 49, 8, 5) }, //Scene5
            { LedId.Custom14, (0x90, 39, 8, 6) }, //Scene6
            { LedId.Custom15, (0x90, 29, 8, 7) }, //Scene7
            { LedId.Custom16, (0x90, 19, 8, 8) }, //Scene8
        };
}