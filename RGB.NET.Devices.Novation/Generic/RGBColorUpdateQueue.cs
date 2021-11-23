using System;
using RGB.NET.Core;
using Sanford.Multimedia.Midi;

namespace RGB.NET.Devices.Novation;

/// <summary>
/// Represents the update-queue performing updates for a RGB-color novation device.
/// </summary>
public class RGBColorUpdateQueue : MidiUpdateQueue
{
    #region Properties & Fields

    private static readonly (Color, int)[] COLOR_PALETTE =
    {
        (new Color(0, 0, 0), 0),
        (new Color(28, 28, 28), 1),
        (new Color(124, 124, 124), 2),
        (new Color(252, 252, 252), 3),
        (new Color(255, 77, 71), 4),
        (new Color(255, 10, 0), 5),
        (new Color(90, 1, 0), 6),
        (new Color(25, 0, 0), 7),
        (new Color(255, 189, 98), 8),
        (new Color(255, 86, 0), 9),
        (new Color(90, 29, 0), 10),
        (new Color(36, 24, 0), 11),
        (new Color(253, 253, 33), 12),
        (new Color(253, 253, 0), 13),
        (new Color(88, 88, 0), 14),
        (new Color(24, 24, 0), 15),
        (new Color(128, 253, 42), 16),
        (new Color(64, 253, 0), 17),
        (new Color(22, 88, 0), 18),
        (new Color(19, 40, 0), 19),
        (new Color(52, 253, 43), 20),
        (new Color(0, 253, 0), 21),
        (new Color(0, 88, 0), 22),
        (new Color(0, 24, 0), 23),
        (new Color(51, 253, 70), 24),
        (new Color(50, 253, 126), 28),
        (new Color(0, 253, 58), 29),
        (new Color(0, 88, 20), 30),
        (new Color(0, 28, 15), 31),
        (new Color(47, 252, 176), 32),
        (new Color(0, 252, 145), 33),
        (new Color(0, 88, 49), 34),
        (new Color(0, 24, 15), 35),
        (new Color(57, 191, 255), 36),
        (new Color(0, 167, 255), 37),
        (new Color(0, 64, 81), 38),
        (new Color(0, 16, 24), 39),
        (new Color(65, 134, 255), 40),
        (new Color(0, 80, 255), 41),
        (new Color(0, 26, 90), 42),
        (new Color(0, 7, 25), 43),
        (new Color(70, 71, 255), 44),
        (new Color(0, 0, 255), 45),
        (new Color(0, 0, 91), 46),
        (new Color(0, 0, 25), 47),
        (new Color(131, 71, 255), 48),
        (new Color(80, 0, 255), 49),
        (new Color(22, 0, 103), 50),
        (new Color(11, 0, 50), 51),
        (new Color(255, 73, 255), 52),
        (new Color(255, 0, 255), 53),
        (new Color(90, 0, 90), 54),
        (new Color(25, 0, 25), 55),
        (new Color(255, 77, 132), 56),
        (new Color(255, 7, 82), 57),
        (new Color(90, 1, 27), 58),
        (new Color(33, 0, 16), 59),
        (new Color(255, 25, 0), 60),
        (new Color(155, 53, 0), 61),
        (new Color(122, 81, 0), 62),
        (new Color(62, 100, 0), 63),
        (new Color(0, 56, 0), 64),
        (new Color(0, 84, 50), 65),
        (new Color(0, 83, 126), 66),
        (new Color(0, 68, 77), 68),
        (new Color(27, 0, 210), 69),
        (new Color(32, 32, 32), 71),
        (new Color(186, 253, 0), 73),
        (new Color(170, 237, 0), 74),
        (new Color(86, 253, 0), 75),
        (new Color(0, 136, 0), 76),
        (new Color(0, 252, 122), 77),
        (new Color(0, 27, 255), 79),
        (new Color(53, 0, 255), 80),
        (new Color(119, 0, 255), 81),
        (new Color(180, 23, 126), 82),
        (new Color(65, 32, 0), 83),
        (new Color(255, 74, 0), 84),
        (new Color(131, 225, 0), 85),
        (new Color(101, 253, 0), 86),
        (new Color(69, 253, 97), 89),
        (new Color(0, 252, 202), 90),
        (new Color(80, 134, 255), 91),
        (new Color(39, 77, 201), 92),
        (new Color(130, 122, 237), 93),
        (new Color(211, 12, 255), 94),
        (new Color(255, 6, 90), 95),
        (new Color(255, 125, 0), 96),
        (new Color(185, 177, 0), 97),
        (new Color(138, 253, 0), 98),
        (new Color(130, 93, 0), 99),
        (new Color(57, 40, 0), 100),
        (new Color(13, 76, 5), 101),
        (new Color(0, 80, 55), 102),
        (new Color(19, 19, 41), 103),
        (new Color(16, 31, 90), 104),
        (new Color(106, 60, 23), 105),
        (new Color(172, 4, 0), 106),
        (new Color(225, 81, 53), 107),
        (new Color(220, 105, 0), 108),
        (new Color(255, 255, 0), 109),
        (new Color(153, 225, 0), 110),
        (new Color(95, 181, 0), 111),
        (new Color(27, 27, 49), 112),
        (new Color(220, 253, 84), 113),
        (new Color(118, 252, 184), 114),
        (new Color(150, 151, 255), 115),
        (new Color(139, 97, 255), 116),
        (new Color(64, 64, 64), 117),
        (new Color(116, 116, 116), 118),
        (new Color(222, 252, 252), 119),
        (new Color(164, 4, 0), 120),
        (new Color(53, 0, 0), 121),
        (new Color(0, 209, 0), 122),
        (new Color(0, 64, 0), 123),
        (new Color(61, 48, 0), 125),
        (new Color(180, 93, 0), 126),
        (new Color(74, 20, 0), 127),
    };

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="RGBColorUpdateQueue"/> class.
    /// </summary>
    /// <param name="updateTrigger">The update trigger used by this queue.</param>
    /// <param name="deviceId">The device-id of the device this queue is performing updates for.</param>
    public RGBColorUpdateQueue(IDeviceUpdateTrigger updateTrigger, int deviceId)
        : base(updateTrigger, deviceId)
    { }

    #endregion

    #region Methods

    /// <inheritdoc />
    protected override ShortMessage? CreateMessage(object key, in Color color)
    {
        (byte mode, byte id) = ((byte, byte))key;
        if (mode == 0x00) return null;

        return new ShortMessage(mode, id, Convert.ToByte(ConvertColor(color)));
    }

    /// <summary>
    /// Convert a <see cref="Color"/> to its novation-representation depending on the <see cref="NovationColorCapabilities"/> of the <see cref="NovationRGBDevice{TDeviceInfo}"/>.
    /// Source: http://www.launchpadfun.com/downloads_de/velocity-colors/
    /// </summary>
    /// <param name="color">The <see cref="Color"/> to convert.</param>
    /// <returns>The novation-representation of the <see cref="Color"/>.</returns>
    protected virtual int ConvertColor(in Color color)
    {
        int bestVelocity = 0;
        double bestMatchDistance = double.MaxValue;
        foreach ((Color c, int velocity) in COLOR_PALETTE)
        {
            double distance = c.DistanceTo(color);
            if (distance < bestMatchDistance)
            {
                bestVelocity = velocity;
                bestMatchDistance = distance;
            }
        }

        return bestVelocity;
    }

    /// <inheritdoc />
    public override void Reset()
    {
        base.Reset();
        SendMessage(new ShortMessage(Convert.ToByte(0xB0), Convert.ToByte(0), Convert.ToByte(0)));
    }

    #endregion
}