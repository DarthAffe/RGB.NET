using System;
using System.Collections.Generic;
using AuraServiceLib;
using RGB.NET.Core;

namespace RGB.NET.Devices.Asus
{
    /// <inheritdoc />
    /// <summary>
    /// Represents the update-queue performing updates for asus devices.
    /// </summary>
    public class AsusUpdateQueue : UpdateQueue
    {
        #region Properties & Fields

        /// <summary>
        /// The device to be updated.
        /// </summary>
        protected IAuraSyncDevice Device { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AsusUpdateQueue"/> class.
        /// </summary>
        /// <param name="updateTrigger">The update trigger used by this queue.</param>
        public AsusUpdateQueue(IDeviceUpdateTrigger updateTrigger)
            : base(updateTrigger)
        { }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the queue.
        /// </summary>
        /// <param name="device">The device to be updated.</param>
        public void Initialize(IAuraSyncDevice device)
        {
            Device = device;
        }

        /// <inheritdoc />
        protected override void Update(Dictionary<object, Color> dataSet)
        {
            try
            {
                if (Device.Type == 0x00080000 || Device.Type == 0x00081000)//Keyboard
                {
                    foreach (KeyValuePair<object, Color> data in dataSet)
                    {
                        ushort index = (ushort)data.Key;
                        IAuraSyncKeyboard keyboard = (IAuraSyncKeyboard)Device;
                        if (keyboard != null)
                        {
                            IAuraRgbLight light;
                            //UK keyboard Layout
                            if (index == 0x56)
                            {
                                light = keyboard.Lights[(int)(3 * keyboard.Width + 13)];
                            }
                            else if (index == 0x59)
                            {
                                light = keyboard.Lights[(int)(4 * keyboard.Width + 1)];
                            }
                            else
                            {
                                light = keyboard.Key[index];
                            }
                            // Asus Strix Scope
                            if (keyboard.Name == "Charm")
                            {
                                if (index == 0XDB)
                                {
                                    light = keyboard.Lights[(int)(5 * keyboard.Width + 2)];
                                }
                                else if (index == 0x38)
                                {
                                    light = keyboard.Lights[(int)(5 * keyboard.Width + 3)];
                                }
                            }
                            (_, byte r, byte g, byte b) = data.Value.GetRGBBytes();
                            light.Red = r;
                            light.Green = g;
                            light.Blue = b;
                        }
                    }
                }
                else
                {
                    foreach (KeyValuePair<object, Color> data in dataSet)
                    {
                        int index = (int)data.Key;
                        IAuraRgbLight light = Device.Lights[index];
                        (_, byte r, byte g, byte b) = data.Value.GetRGBBytes();
                        light.Red = r;
                        light.Green = g;
                        light.Blue = b;
                    }
                }

                Device.Apply();
            }
            catch (Exception ex)
            { /* "The server threw an exception." seems to be a thing here ... */ }
        }
        #endregion
    }
}
