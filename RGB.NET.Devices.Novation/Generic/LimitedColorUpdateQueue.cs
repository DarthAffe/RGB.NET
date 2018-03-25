using System;
using System.Collections.Generic;
using RGB.NET.Core;
using Sanford.Multimedia.Midi;

namespace RGB.NET.Devices.Novation
{
    public class LimitedColorUpdateQueue : MidiUpdateQueue
    {
        #region Constructors

        public LimitedColorUpdateQueue(IUpdateTrigger updateTrigger, int deviceId)
            : base(updateTrigger, deviceId)
        { }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override ShortMessage CreateMessage(KeyValuePair<object, Color> data)
        {
            NovationLedId ledId = (NovationLedId)data.Key;
            return new ShortMessage(Convert.ToByte(ledId.GetStatus()), Convert.ToByte(ledId.GetId()), Convert.ToByte(ConvertColor(data.Value)));
        }

        /// <summary>
        /// Convert a <see cref="Color"/> to its novation-representation depending on the <see cref="NovationColorCapabilities"/> of the <see cref="NovationRGBDevice{TDeviceInfo}"/>.
        /// The conversion uses only a limited amount of colors (3 red, 3 yellow, 3 green).
        /// </summary>
        /// <param name="color">The <see cref="Color"/> to convert.</param>
        /// <returns>The novation-representation of the <see cref="Color"/>.</returns>
        protected virtual int ConvertColor(Color color)
        {
            if ((color.Hue >= 330) || (color.Hue < 30))
                return (int)Math.Ceiling(color.Value * 3); // red with brightness 1, 2 or 3

            if ((color.Hue >= 30) && (color.Hue < 90)) // yellow with brightness 17, 34 or 51
                return (int)Math.Ceiling(color.Value * 3) * 17;

            if ((color.Hue >= 90) && (color.Hue < 150)) // green with brightness 16, 32 or 48
                return (int)Math.Ceiling(color.Value * 3) * 16;

            return 0;
        }

        /// <inheritdoc />
        public override void Reset()
        {
            base.Reset();
            SendMessage(new ShortMessage(Convert.ToByte(0xB0), Convert.ToByte(0), Convert.ToByte(0)));
        }

        #endregion
    }
}
