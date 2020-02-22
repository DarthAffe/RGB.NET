using AuraServiceLib;
using RGB.NET.Core;

namespace RGB.NET.Devices.Asus
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a Asus keyboard.
    /// </summary>
    public class AsusKeyboardRGBDevice : AsusRGBDevice<AsusKeyboardRGBDeviceInfo>
    {
        #region Constructors

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RGB.NET.Devices.Asus.AsusKeyboardRGBDevice" /> class.
        /// </summary>
        /// <param name="info">The specific information provided by Asus for the keyboard.</param>
        internal AsusKeyboardRGBDevice(AsusKeyboardRGBDeviceInfo info)
            : base(info)
        { }

        #endregion

        #region Methods
        private ushort asusCode;
        /// <inheritdoc />
        protected override void InitializeLayout()
        {
            if (DeviceInfo.Device.Type != 0x00081001)
            {
                int pos = 0;
                foreach (IAuraRgbKey key in ((IAuraSyncKeyboard)DeviceInfo.Device).Keys)
                {
                    asusCode = key.Code;
                    InitializeLed(AsusLedIdMapper(key.Code), new Rectangle(pos++ * 19, 0, 19, 19));
                }
                //UK Layout
                asusCode = 0x56;
                InitializeLed(AsusLedIdMapper(asusCode), new Rectangle(pos++ * 19, 0, 19, 19));
                asusCode = 0x59;
                InitializeLed(AsusLedIdMapper(asusCode), new Rectangle(pos++ * 19, 0, 19, 19));
            }
            else
            {
                int ledCount = DeviceInfo.Device.Lights.Count;
                for (int i = 0; i < ledCount; i++)
                {
                    asusCode = (ushort)i;
                    InitializeLed(LedId.Keyboard_Custom1 + i, new Rectangle(i * 19, 0, 19, 19));
                }

            }

            string model = DeviceInfo.Model.Replace(" ", string.Empty).ToUpper();
            ApplyLayoutFromFile(PathHelper.GetAbsolutePath(this, $@"Layouts\Asus\Keyboards\{model}", $"{DeviceInfo.PhysicalLayout.ToString().ToUpper()}.xml"), DeviceInfo.LogicalLayout.ToString());
        }

        /// <inheritdoc />
        protected override object CreateLedCustomData(LedId ledId) => asusCode;

        private LedId AsusLedIdMapper(ushort asusKey)
        {
            switch (asusKey)
            {
                case 0x01:
                    return LedId.Keyboard_Escape;
                case 0x3B:
                    return LedId.Keyboard_F1;
                case 0x3C:
                    return LedId.Keyboard_F2;
                case 0x3D:
                    return LedId.Keyboard_F3;
                case 0x3E:
                    return LedId.Keyboard_F4;
                case 0x3F:
                    return LedId.Keyboard_F5;
                case 0x40:
                    return LedId.Keyboard_F6;
                case 0x41:
                    return LedId.Keyboard_F7;
                case 0x42:
                    return LedId.Keyboard_F8;
                case 0x43:
                    return LedId.Keyboard_F9;
                case 0x44:
                    return LedId.Keyboard_F10;
                case 0x57:
                    return LedId.Keyboard_F11;
                case 0x58:
                    return LedId.Keyboard_F12;
                case 0x02:
                    return LedId.Keyboard_1;
                case 0x03:
                    return LedId.Keyboard_2;
                case 0x04:
                    return LedId.Keyboard_3;
                case 0x05:
                    return LedId.Keyboard_4;
                case 0x06:
                    return LedId.Keyboard_5;
                case 0x07:
                    return LedId.Keyboard_6;
                case 0x08:
                    return LedId.Keyboard_7;
                case 0x09:
                    return LedId.Keyboard_8;
                case 0x0A:
                    return LedId.Keyboard_9;
                case 0x0B:
                    return LedId.Keyboard_0;
                case 0x0C:
                    return LedId.Keyboard_MinusAndUnderscore;
                case 0x0D:
                    return LedId.Keyboard_EqualsAndPlus;
                case 0x0E:
                    return LedId.Keyboard_Backspace;
                case 0x0F:
                    return LedId.Keyboard_Tab;
                case 0x10:
                    return LedId.Keyboard_Q;
                case 0x11:
                    return LedId.Keyboard_W;
                case 0x12:
                    return LedId.Keyboard_E;
                case 0x13:
                    return LedId.Keyboard_R;
                case 0x14:
                    return LedId.Keyboard_T;
                case 0x15:
                    return LedId.Keyboard_Y;
                case 0x16:
                    return LedId.Keyboard_U;
                case 0x17:
                    return LedId.Keyboard_I;
                case 0x18:
                    return LedId.Keyboard_O;
                case 0x19:
                    return LedId.Keyboard_P;
                case 0x1A:
                    return LedId.Keyboard_BracketLeft;
                case 0x1B:
                    return LedId.Keyboard_BracketRight;
                case 0x1C:
                    return LedId.Keyboard_Enter;
                case 0x3A:
                    return LedId.Keyboard_CapsLock;
                case 0x1E:
                    return LedId.Keyboard_A;
                case 0x1F:
                    return LedId.Keyboard_S;
                case 0x20:
                    return LedId.Keyboard_D;
                case 0x21:
                    return LedId.Keyboard_F;
                case 0x22:
                    return LedId.Keyboard_G;
                case 0x23:
                    return LedId.Keyboard_H;
                case 0x24:
                    return LedId.Keyboard_J;
                case 0x25:
                    return LedId.Keyboard_K;
                case 0x26:
                    return LedId.Keyboard_L;
                case 0x27:
                    return LedId.Keyboard_SemicolonAndColon;
                case 0x28:
                    return LedId.Keyboard_ApostropheAndDoubleQuote;
                case 0x29:
                    return LedId.Keyboard_GraveAccentAndTilde;
                case 0x2A:
                    return LedId.Keyboard_LeftShift;
                case 0x2B:
                    return LedId.Keyboard_Backslash;
                case 0x2C:
                    return LedId.Keyboard_Z;
                case 0x2D:
                    return LedId.Keyboard_X;
                case 0x2E:
                    return LedId.Keyboard_C;
                case 0x2F:
                    return LedId.Keyboard_V;
                case 0x30:
                    return LedId.Keyboard_B;
                case 0x31:
                    return LedId.Keyboard_N;
                case 0x32:
                    return LedId.Keyboard_M;
                case 0x33:
                    return LedId.Keyboard_CommaAndLessThan;
                case 0x34:
                    return LedId.Keyboard_PeriodAndBiggerThan;
                case 0x35:
                    return LedId.Keyboard_SlashAndQuestionMark;
                case 0x36:
                    return LedId.Keyboard_RightShift;
                case 0x1D:
                    return LedId.Keyboard_LeftCtrl;
                case 0xDB:
                    return LedId.Keyboard_LeftGui;
                case 0x38:
                    return LedId.Keyboard_LeftAlt;
                case 0x39:
                    return LedId.Keyboard_Space;
                case 0xB8:
                    return LedId.Keyboard_RightAlt;
                case 0x100:
                    return LedId.Keyboard_RightGui;
                case 0xDD:
                    return LedId.Keyboard_Application;
                case 0x9D:
                    return LedId.Keyboard_RightCtrl;
                case 0xB7:
                    return LedId.Keyboard_PrintScreen;
                case 0x46:
                    return LedId.Keyboard_ScrollLock;
                case 0xC5:
                    return LedId.Keyboard_PauseBreak;
                case 0xD2:
                    return LedId.Keyboard_Insert;
                case 0xC7:
                    return LedId.Keyboard_Home;
                case 0xC9:
                    return LedId.Keyboard_PageUp;
                case 0xD3:
                    return LedId.Keyboard_Delete;
                case 0xCF:
                    return LedId.Keyboard_End;
                case 0xD1:
                    return LedId.Keyboard_PageDown;
                case 0xC8:
                    return LedId.Keyboard_ArrowUp;
                case 0xCB:
                    return LedId.Keyboard_ArrowLeft;
                case 0xD0:
                    return LedId.Keyboard_ArrowDown;
                case 0xCD:
                    return LedId.Keyboard_ArrowRight;
                case 0x45:
                    return LedId.Keyboard_NumLock;
                case 0xB5:
                    return LedId.Keyboard_NumSlash;
                case 0x37:
                    return LedId.Keyboard_NumAsterisk;
                case 0x4A:
                    return LedId.Keyboard_NumMinus;
                case 0x47:
                    return LedId.Keyboard_Num7;
                case 0x48:
                    return LedId.Keyboard_Num8;
                case 0x49:
                    return LedId.Keyboard_Num9;
                case 0x53:
                    return LedId.Keyboard_NumPeriodAndDelete;
                case 0x4E:
                    return LedId.Keyboard_NumPlus;
                case 0x4B:
                    return LedId.Keyboard_Num4;
                case 0x4C:
                    return LedId.Keyboard_Num5;
                case 0x4D:
                    return LedId.Keyboard_Num6;
                case 0x4F:
                    return LedId.Keyboard_Num1;
                case 0x50:
                    return LedId.Keyboard_Num2;
                case 0x51:
                    return LedId.Keyboard_Num3;
                case 0x52:
                    return LedId.Keyboard_Num0;
                case 0x9C:
                    return LedId.Keyboard_NumEnter;
                case 0x59:
                    return LedId.Keyboard_NonUsBackslash;
                case 0x56:
                    return LedId.Keyboard_NonUsTilde;
                case 0xB3:
                    return LedId.Keyboard_NumComma;
                case 0x101:
                    return LedId.Logo;
                case 0x102:
                    return LedId.Keyboard_Custom1;
                case 0x103:
                    return LedId.Keyboard_Custom2;
                default:
                    return LedId.Invalid;
            }
        }
        #endregion
    }
}
