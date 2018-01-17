// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using RGB.NET.Core;

namespace RGB.NET.Devices.Asus.Native
{
    // ReSharper disable once InconsistentNaming
    internal static class _AsusSDK
    {
        #region Libary Management

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
            UnloadAsusSDK();
            LoadAsusSDK();
        }

        private static void LoadAsusSDK()
        {
            if (_dllHandle != IntPtr.Zero) return;

            // HACK: Load library at runtime to support both, x86 and x64 with one managed dll
            List<string> possiblePathList = Environment.Is64BitProcess ? AsusDeviceProvider.PossibleX64NativePaths : AsusDeviceProvider.PossibleX86NativePaths;
            string dllPath = possiblePathList.FirstOrDefault(File.Exists);
            if (dllPath == null) throw new RGBDeviceException($"Can't find the Asus-SDK at one of the expected locations:\r\n '{string.Join("\r\n", possiblePathList.Select(Path.GetFullPath))}'");

            SetDllDirectory(Path.GetDirectoryName(Path.GetFullPath(dllPath)));

            _dllHandle = LoadLibrary(dllPath);

            _enumerateMbControllerPointer = (EnumerateMbControllerPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "EnumerateMbController"), typeof(EnumerateMbControllerPointer));
            _getMbLedCountPointer = (GetMbLedCountPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "GetMbLedCount"), typeof(GetMbLedCountPointer));
            _setMbModePointer = (SetMbModePointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "SetMbMode"), typeof(SetMbModePointer));
            _setMbColorPointer = (SetMbColorPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "SetMbColor"), typeof(SetMbColorPointer));
            _getMbColorPointer = (GetMbColorPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "GetMbColor"), typeof(GetMbColorPointer));

            _enumerateGPUPointer = (EnumerateGPUPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "EnumerateGPU"), typeof(EnumerateGPUPointer));
            _getGPULedCountPointer = (GetGPULedCountPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "GetGPULedCount"), typeof(GetGPULedCountPointer));
            _setGPUModePointer = (SetGPUModePointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "SetGPUMode"), typeof(SetGPUModePointer));
            _setGPUColorPointer = (SetGPUColorPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "SetGPUColor"), typeof(SetGPUColorPointer));

            _createClaymoreKeyboardPointer = (CreateClaymoreKeyboardPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "CreateClaymoreKeyboard"), typeof(CreateClaymoreKeyboardPointer));
            _getClaymoreKeyboardLedCountPointer = (GetClaymoreKeyboardLedCountPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "GetClaymoreKeyboardLedCount"), typeof(GetClaymoreKeyboardLedCountPointer));
            _setClaymoreKeyboardModePointer = (SetClaymoreKeyboardModePointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "SetClaymoreKeyboardMode"), typeof(SetClaymoreKeyboardModePointer));
            _setClaymoreKeyboardColorPointer = (SetClaymoreKeyboardColorPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "SetClaymoreKeyboardColor"), typeof(SetClaymoreKeyboardColorPointer));

            _enumerateRogMousePointer = (CreateRogMousePointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "CreateRogMouse"), typeof(CreateRogMousePointer));
            _getRogMouseLedCountPointer = (GetRogMouseLedCountPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "RogMouseLedCount"), typeof(GetRogMouseLedCountPointer)); // DarthAffe 07.10.2017: Be careful with the naming here - i don't know why but there is no 'Get'!
            _setRogMouseModePointer = (SetRogMouseModePointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "SetRogMouseMode"), typeof(SetRogMouseModePointer));
            _setRogMouseColorPointer = (SetRogMouseColorPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "SetRogMouseColor"), typeof(SetRogMouseColorPointer));

            //TODO DarthAffe 29.10.2017: I don't know why they are even documented, but the asus guy said they aren't in the SDK right now.
            //_enumerateDramPointer = (EnumerateDramPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "EnumerateDram"), typeof(EnumerateDramPointer));
            //_getDramLedCountPointer = (GetDramLedCountPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "GetDramLedCount"), typeof(GetDramLedCountPointer));
            //_setDramModePointer = (SetDramModePointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "SetDramMode"), typeof(SetDramModePointer));
            //_setDramColorPointer = (SetDramColorPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "SetDramColor"), typeof(SetDramColorPointer));
            //_getDramColorPointer = (GetDramColorPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "GetDramColor"), typeof(GetDramColorPointer));
        }

        private static void UnloadAsusSDK()
        {
            if (_dllHandle == IntPtr.Zero) return;

            // ReSharper disable once EmptyEmbeddedStatement - DarthAffe 07.10.2017: We might need to reduce the internal reference counter more than once to set the library free
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

        private static EnumerateMbControllerPointer _enumerateMbControllerPointer;
        private static GetMbLedCountPointer _getMbLedCountPointer;
        private static SetMbModePointer _setMbModePointer;
        private static SetMbColorPointer _setMbColorPointer;
        private static GetMbColorPointer _getMbColorPointer;

        private static EnumerateGPUPointer _enumerateGPUPointer;
        private static GetGPULedCountPointer _getGPULedCountPointer;
        private static SetGPUModePointer _setGPUModePointer;
        private static SetGPUColorPointer _setGPUColorPointer;

        private static CreateClaymoreKeyboardPointer _createClaymoreKeyboardPointer;
        private static GetClaymoreKeyboardLedCountPointer _getClaymoreKeyboardLedCountPointer;
        private static SetClaymoreKeyboardModePointer _setClaymoreKeyboardModePointer;
        private static SetClaymoreKeyboardColorPointer _setClaymoreKeyboardColorPointer;

        private static CreateRogMousePointer _enumerateRogMousePointer;
        private static GetRogMouseLedCountPointer _getRogMouseLedCountPointer;
        private static SetRogMouseModePointer _setRogMouseModePointer;
        private static SetRogMouseColorPointer _setRogMouseColorPointer;

        //private static EnumerateDramPointer _enumerateDramPointer;
        //private static SetDramModePointer _setDramModePointer;
        //private static GetDramLedCountPointer _getDramLedCountPointer;
        //private static SetDramColorPointer _setDramColorPointer;
        //private static GetDramColorPointer _getDramColorPointer;

        #endregion

        #region Delegates

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int EnumerateMbControllerPointer(IntPtr handles, int size);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int GetMbLedCountPointer(IntPtr handle);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void SetMbModePointer(IntPtr handle, int mode);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void SetMbColorPointer(IntPtr handle, byte[] colors, int size);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int GetMbColorPointer(IntPtr handle, IntPtr colors, int size);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int EnumerateGPUPointer(IntPtr handles, int size);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int GetGPULedCountPointer(IntPtr handle);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void SetGPUModePointer(IntPtr handle, int mode);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void SetGPUColorPointer(IntPtr handle, byte[] colors, int size);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate bool CreateClaymoreKeyboardPointer(IntPtr handle);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int GetClaymoreKeyboardLedCountPointer(IntPtr handle);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void SetClaymoreKeyboardModePointer(IntPtr handle, int mode);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void SetClaymoreKeyboardColorPointer(IntPtr handle, byte[] colors, int size);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate bool CreateRogMousePointer(IntPtr handle);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int GetRogMouseLedCountPointer(IntPtr handle);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void SetRogMouseModePointer(IntPtr handle, int mode);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void SetRogMouseColorPointer(IntPtr handle, byte[] colors, int size);

        //[UnmanagedFunctionPointer(CallingConvention.StdCall)]
        //private delegate int EnumerateDramPointer(IntPtr handles, int size);
        //[UnmanagedFunctionPointer(CallingConvention.StdCall)]
        //private delegate int GetDramLedCountPointer(IntPtr handle);
        //[UnmanagedFunctionPointer(CallingConvention.StdCall)]
        //private delegate void SetDramModePointer(IntPtr handle, int mode);
        //[UnmanagedFunctionPointer(CallingConvention.StdCall)]
        //private delegate void SetDramColorPointer(IntPtr handle, byte[] colors, int size);
        //[UnmanagedFunctionPointer(CallingConvention.StdCall)]
        //private delegate int GetDramColorPointer(IntPtr handle, IntPtr colors, int size);

        #endregion

        // ReSharper disable EventExceptionNotDocumented

        internal static int EnumerateMbController(IntPtr handles, int size) => _enumerateMbControllerPointer(handles, size);
        internal static int GetMbLedCount(IntPtr handle) => _getMbLedCountPointer(handle);
        internal static void SetMbMode(IntPtr handle, int mode) => _setMbModePointer(handle, mode);
        internal static void SetMbColor(IntPtr handle, byte[] colors) => _setMbColorPointer(handle, colors, colors.Length);

        internal static byte[] GetMbColor(IntPtr handle)
        {
            int count = _getMbColorPointer(handle, IntPtr.Zero, 0);
            byte[] colors = new byte[count];
            IntPtr readColorsPtr = Marshal.AllocHGlobal(colors.Length);
            _getMbColorPointer(handle, readColorsPtr, colors.Length);
            Marshal.Copy(readColorsPtr, colors, 0, colors.Length);
            Marshal.FreeHGlobal(readColorsPtr);
            return colors;
        }

        internal static int EnumerateGPU(IntPtr handles, int size) => _enumerateGPUPointer(handles, size);
        internal static int GetGPULedCount(IntPtr handle) => _getGPULedCountPointer(handle);
        internal static void SetGPUMode(IntPtr handle, int mode) => _setGPUModePointer(handle, mode);
        internal static void SetGPUColor(IntPtr handle, byte[] colors) => _setGPUColorPointer(handle, colors, colors.Length);

        internal static bool CreateClaymoreKeyboard(IntPtr handle) => _createClaymoreKeyboardPointer(handle);
        internal static int GetClaymoreKeyboardLedCount(IntPtr handle) => _getClaymoreKeyboardLedCountPointer(handle);
        internal static void SetClaymoreKeyboardMode(IntPtr handle, int mode) => _setClaymoreKeyboardModePointer(handle, mode);
        internal static void SetClaymoreKeyboardColor(IntPtr handle, byte[] colors) => _setClaymoreKeyboardColorPointer(handle, colors, colors.Length);

        internal static bool CreateRogMouse(IntPtr handle) => _enumerateRogMousePointer(handle);
        internal static int GetRogMouseLedCount(IntPtr handle) => _getRogMouseLedCountPointer(handle);
        internal static void SetRogMouseMode(IntPtr handle, int mode) => _setRogMouseModePointer(handle, mode);
        internal static void SetRogMouseColor(IntPtr handle, byte[] colors) => _setRogMouseColorPointer(handle, colors, colors.Length);

        //internal static int EnumerateDram(IntPtr handles, int size) => _enumerateDramPointer(handles, size);
        //internal static int GetDramLedCount(IntPtr handle) => _getDramLedCountPointer(handle);
        //internal static void SetDramMode(IntPtr handle, int mode) => _setDramModePointer(handle, mode);
        //internal static void SetDramColor(IntPtr handle, byte[] colors) => _setDramColorPointer(handle, colors, colors.Length);

        //internal static byte[] GetDramColor(IntPtr handle)
        //{
        //    int count = _getDramColorPointer(handle, IntPtr.Zero, 0);
        //    byte[] colors = new byte[count];
        //    IntPtr readColorsPtr = Marshal.AllocHGlobal(colors.Length);
        //    _getDramColorPointer(handle, readColorsPtr, colors.Length);
        //    Marshal.Copy(readColorsPtr, colors, 0, colors.Length);
        //    Marshal.FreeHGlobal(readColorsPtr);
        //    return colors;
        //}

        // ReSharper restore EventExceptionNotDocumented

        #endregion
    }
}
