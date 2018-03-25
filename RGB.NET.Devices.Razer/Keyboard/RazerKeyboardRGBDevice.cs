// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using RGB.NET.Core;
using RGB.NET.Devices.Razer.Native;

namespace RGB.NET.Devices.Razer
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a razer keyboard.
    /// </summary>
    public class RazerKeyboardRGBDevice : RazerRGBDevice<RazerKeyboardRGBDeviceInfo>
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Razer.RazerKeyboardRGBDevice" /> class.
        /// </summary>
        /// <param name="info">The specific information provided by CUE for the keyboard.</param>
        internal RazerKeyboardRGBDevice(RazerKeyboardRGBDeviceInfo info)
            : base(info)
        { }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void InitializeLayout()
        {
            //string model = DeviceInfo.Model.Replace(" ", string.Empty).ToUpper();
            //ApplyLayoutFromFile(PathHelper.GetAbsolutePath(
            //    $@"Layouts\Razer\Keyboards\{model}\{DeviceInfo.PhysicalLayout.ToString().ToUpper()}.xml"),
            //    DeviceInfo.LogicalLayout.ToString(), PathHelper.GetAbsolutePath(@"Images\Razer\Keyboards"));

            //TODO DarthAffe 13.12.2017: Correctly select ids
            if (LedMapping.Count == 0)
            {
                for (int i = 0; i < _Defines.KEYBOARD_MAX_ROW; i++)
                    for (int j = 0; j < _Defines.KEYBOARD_MAX_COLUMN; j++)
                        InitializeLed(LedId.Keyboard_Escape + ((i * _Defines.KEYBOARD_MAX_COLUMN) + j), new Rectangle(j * 20, i * 20, 19, 19));
            }
        }

        /// <inheritdoc />
        protected override object CreateLedCustomData(LedId ledId) => (int)ledId - (int)LedId.Keyboard_Escape;

        /// <inheritdoc />
        protected override RazerUpdateQueue CreateUpdateQueue(IUpdateTrigger updateTrigger) => new RazerKeyboardUpdateQueue(updateTrigger, DeviceInfo.DeviceId);

        #endregion
    }
}
