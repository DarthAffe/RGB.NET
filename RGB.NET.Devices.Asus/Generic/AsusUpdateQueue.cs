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
                if ((Device.Type == (uint)AsusDeviceType.KEYBOARD_RGB) || (Device.Type == (uint)AsusDeviceType.NB_KB_RGB))
                {
                    foreach (KeyValuePair<object, Color> data in dataSet)
                    {
                        AsusLedId index = (AsusLedId)data.Key;
                        IAuraSyncKeyboard keyboard = (IAuraSyncKeyboard)Device;
                        if (keyboard != null)
                        {
                            IAuraRgbLight light = index switch
                            {
                                //UK keyboard Layout
                                AsusLedId.KEY_OEM_102 => keyboard.Lights[(int)((3 * keyboard.Width) + 13)],
                                AsusLedId.UNDOCUMENTED_1 => keyboard.Lights[(int)((4 * keyboard.Width) + 1)],
                                _ => keyboard.Key[(ushort)index]
                            };

                            // Asus Strix Scope
                            if (keyboard.Name == "Charm")
                                light = index switch
                                {
                                    AsusLedId.KEY_LWIN => keyboard.Lights[(int)((5 * keyboard.Width) + 2)],
                                    AsusLedId.KEY_LMENU => keyboard.Lights[(int)((5 * keyboard.Width) + 3)],
                                    _ => light
                                };

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
            catch
            { /* "The server threw an exception." seems to be a thing here ... */ }
        }
        #endregion
    }
}
