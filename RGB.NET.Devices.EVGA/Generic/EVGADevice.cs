using RGB.NET.Core;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace RGB.NET.Devices.EVGA.Generic
{
    public class EVGADevice : AbstractRGBDevice<EVGADeviceInfo>
    {
        private uint _deviceId;
        private uint _ledCount;

        private Action<int, int, byte, byte, byte, byte> _ledSetter;

        public EVGADevice(EVGADeviceInfo deviceInfo, Action<int, int, byte, byte, byte, byte> ledSetter, uint deviceId, uint ledCount)
        {
            _deviceInfo = deviceInfo;
            _ledSetter = ledSetter;
            _deviceId = deviceId;
            _ledCount = ledCount;


            InitLeds();
        }

        private void InitLeds()
        {
            for (uint i =0; i < _ledCount; i++)
            {
                InitializeLed((LedId)((uint)LedId.GraphicsCard1 + i), new Rectangle(new Point(i, 1), new Size(1, 1)));
            }
        }

        private EVGADeviceInfo _deviceInfo;
        public override EVGADeviceInfo DeviceInfo => _deviceInfo;

        protected override void UpdateLeds(IEnumerable<Led> ledsToUpdate)
        {
            foreach (var led in ledsToUpdate)
            {
                if (led.Device != this)
                {
                    //not sure if this is needed
                    continue;
                }
                _ledSetter((int)_deviceId, (int)((uint)led.Id - (uint)LedId.GraphicsCard1), (byte)(255 * led.Color.A), (byte)(255 * led.Color.R), (byte)(255 * led.Color.G), (byte)(255 * led.Color.B));
            }
            
        }
    }
}
