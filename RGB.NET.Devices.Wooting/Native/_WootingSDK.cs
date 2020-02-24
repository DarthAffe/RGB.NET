// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using RGB.NET.Core;

namespace RGB.NET.Devices.Wooting.Native
{
    // ReSharper disable once InconsistentNaming
    public class _WootingSDK
    {
        #region Library management

        private static IntPtr _dllHandle = IntPtr.Zero;

        /// <summary>
        /// Gets the loaded architecture (x64/x86).
        /// </summary>
        internal static string LoadedArchitecture { get; private set; }

        /// <summary>
        /// Reloads the SDK.
        /// </summary>
        internal static void Reload()
        {
            UnloadWootingSDK();
            LoadWootingSDK();
        }

        private static void LoadWootingSDK()
        {
            if (_dllHandle != IntPtr.Zero) return;

            // HACK: Load library at runtime to support both, x86 and x64 with one managed dll
            List<string> possiblePathList = Environment.Is64BitProcess ? WootingDeviceProvider.PossibleX64NativePaths : WootingDeviceProvider.PossibleX86NativePaths;
            string dllPath = possiblePathList.FirstOrDefault(File.Exists);
            if (dllPath == null) throw new RGBDeviceException($"Can't find the Wooting-SDK at one of the expected locations:\r\n '{string.Join("\r\n", possiblePathList.Select(Path.GetFullPath))}'");

            SetDllDirectory(Path.GetDirectoryName(Path.GetFullPath(dllPath)));

            _dllHandle = LoadLibrary(dllPath);

            _getDeviceInfoPointer = (GetDeviceInfoPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "wooting_rgb_device_info"), typeof(GetDeviceInfoPointer));
            _keyboardConnectedPointer = (KeyboardConnectedPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "wooting_rgb_kbd_connected"), typeof(KeyboardConnectedPointer));
            _resetPointer = (ResetPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "wooting_rgb_reset"), typeof(ResetPointer));
            _arrayUpdateKeyboardPointer = (ArrayUpdateKeyboardPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "wooting_rgb_array_update_keyboard"), typeof(ArrayUpdateKeyboardPointer));
            _arraySetSinglePointer = (ArraySetSinglePointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "wooting_rgb_array_set_single"), typeof(ArraySetSinglePointer));
        }

        private static void UnloadWootingSDK()
        {
            if (_dllHandle == IntPtr.Zero) return;

            // ReSharper disable once EmptyEmbeddedStatement - DarthAffe 20.02.2016: We might need to reduce the internal reference counter more than once to set the library free
            while (FreeLibrary(_dllHandle)) ;
            _dllHandle = IntPtr.Zero;
        }

        [DllImport("kernel32.dll")]
        private static extern bool SetDllDirectory(string lpPathName);

        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        private static extern bool FreeLibrary(IntPtr dllHandle);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetProcAddress(IntPtr dllHandle, string name);

        #endregion

        #region SDK-METHODS

        #region Pointers

        private static GetDeviceInfoPointer _getDeviceInfoPointer;
        private static KeyboardConnectedPointer _keyboardConnectedPointer;
        private static ResetPointer _resetPointer;
        private static ArrayUpdateKeyboardPointer _arrayUpdateKeyboardPointer;
        private static ArraySetSinglePointer _arraySetSinglePointer;

        #endregion

        #region Delegates

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr GetDeviceInfoPointer();
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool KeyboardConnectedPointer();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool ResetPointer();
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool ArrayUpdateKeyboardPointer();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool ArraySetSinglePointer(byte row, byte column, byte red, byte green, byte blue);

        #endregion

        internal static IntPtr GetDeviceInfo() => _getDeviceInfoPointer();
        internal static bool KeyboardConnected() => _keyboardConnectedPointer();
        internal static bool Reset() => _resetPointer();
        internal static bool ArrayUpdateKeyboard() => _arrayUpdateKeyboardPointer();
        internal static bool ArraySetSingle(byte row, byte column, byte red, byte green, byte blue) => _arraySetSinglePointer(row, column, red, green, blue);

        #endregion
    }
}
