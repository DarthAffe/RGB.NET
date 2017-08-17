// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

#pragma warning disable 1591 // Missing XML comment for publicly visible type or member

namespace RGB.NET.Devices.Novation
{
    /// <summary>
    /// Contains list of all LEDs available for all Novation devices.
    /// They are represented as Hex 0x[00][11] where [00] is the status flag, [1] the id of the led.
    /// </summary>
    //TODO DarthAffe 15.08.2017: Check if this is really correct for all devices
    public enum NovationLedIds
    {
        Invalid = -1,

        Grid1 = 0x9000,
        Grid2 = 0x9001,
        Grid3 = 0x9002,
        Grid4 = 0x9003,
        Grid5 = 0x9004,
        Grid6 = 0x9005,
        Grid7 = 0x9006,
        Grid8 = 0x9007,

        Grid9  = 0x9010,
        Grid10 = 0x9011,
        Grid11 = 0x9012,
        Grid12 = 0x9013,
        Grid13 = 0x9014,
        Grid14 = 0x9015,
        Grid15 = 0x9016,
        Grid16 = 0x9017,

        Grid17 = 0x9020,
        Grid18 = 0x9021,
        Grid19 = 0x9022,
        Grid20 = 0x9023,
        Grid21 = 0x9024,
        Grid22 = 0x9025,
        Grid23 = 0x9026,
        Grid24 = 0x9027,

        Grid25 = 0x9030,
        Grid26 = 0x9031,
        Grid27 = 0x9032,
        Grid28 = 0x9033,
        Grid29 = 0x9034,
        Grid30 = 0x9035,
        Grid31 = 0x9036,
        Grid32 = 0x9037,

        Grid33 = 0x9040,
        Grid34 = 0x9041,
        Grid35 = 0x9042,
        Grid36 = 0x9043,
        Grid37 = 0x9044,
        Grid38 = 0x9045,
        Grid39 = 0x9046,
        Grid40 = 0x9047,

        Grid41 = 0x9050,
        Grid42 = 0x9051,
        Grid43 = 0x9052,
        Grid44 = 0x9053,
        Grid45 = 0x9054,
        Grid46 = 0x9055,
        Grid47 = 0x9056,
        Grid48 = 0x9057,

        Grid49 = 0x9060,
        Grid50 = 0x9061,
        Grid51 = 0x9062,
        Grid52 = 0x9063,
        Grid53 = 0x9064,
        Grid54 = 0x9065,
        Grid55 = 0x9066,
        Grid56 = 0x9067,

        Grid57 = 0x9070,
        Grid58 = 0x9071,
        Grid59 = 0x9072,
        Grid60 = 0x9073,
        Grid61 = 0x9074,
        Grid62 = 0x9075,
        Grid63 = 0x9076,
        Grid64 = 0x9077,

        Up      = 0xB068,
        Down    = 0xB069,
        Left    = 0xB06A,
        Right   = 0xB06B,
        Session = 0xB06C,
        User1   = 0xB06D,
        User2   = 0xB06E,
        Mix     = 0xB06F,
        
        Scene1 = 0x9009,
        Scene2 = 0x9019,
        Scene3 = 0x9029,
        Scene4 = 0x9039,
        Scene5 = 0x9049,
        Scene6 = 0x9059,
        Scene7 = 0x9069,
        Scene8 = 0x9079
    }
}
