// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using RGB.NET.Core.Exceptions;

namespace RGB.NET.Devices.Msi.Native
{
    // ReSharper disable once InconsistentNaming
    internal static class _MsiSDK
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
            UnloadMsiSDK();
            LoadMsiSDK();
        }

        private static void LoadMsiSDK()
        {
            if (_dllHandle != IntPtr.Zero) return;

            // HACK: Load library at runtime to support both, x86 and x64 with one managed dll
            List<string> possiblePathList = Environment.Is64BitProcess ? MsiDeviceProvider.PossibleX64NativePaths : MsiDeviceProvider.PossibleX86NativePaths;
            string dllPath = possiblePathList.FirstOrDefault(File.Exists);
            if (dllPath == null) throw new RGBDeviceException($"Can't find the Msi-SDK at one of the expected locations:\r\n '{string.Join("\r\n", possiblePathList.Select(Path.GetFullPath))}'");

            SetDllDirectory(Path.GetDirectoryName(Path.GetFullPath(dllPath)));

            _dllHandle = LoadLibrary(dllPath);

            _initializePointer = (InitializePointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "MLAPI_Initialize"), typeof(InitializePointer));
            _getDeviceInfoPointer = (GetDeviceInfoPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "MLAPI_GetDeviceInfo"), typeof(GetDeviceInfoPointer));
            _getLedInfoPointer = (GetLedInfoPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "MLAPI_GetLedInfo"), typeof(GetLedInfoPointer));
            _getLedColorPointer = (GetLedColorPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "MLAPI_GetLedColor"), typeof(GetLedColorPointer));
            _getLedStylePointer = (GetLedStylePointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "MLAPI_GetLedStyle"), typeof(GetLedStylePointer));
            _getLedMaxBrightPointer = (GetLedMaxBrightPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "MLAPI_GetLedMaxBright"), typeof(GetLedMaxBrightPointer));
            _getLedBrightPointer = (GetLedBrightPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "MLAPI_GetLedBright"), typeof(GetLedBrightPointer));
            _getLedMaxSpeedPointer = (GetLedMaxSpeedPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "MLAPI_GetLedMaxSpeed"), typeof(GetLedMaxSpeedPointer));
            _getLedSpeedPointer = (GetLedSpeedPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "MLAPI_GetLedSpeed"), typeof(GetLedSpeedPointer));
            _setLedColorPointer = (SetLedColorPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "MLAPI_SetLedColor"), typeof(SetLedColorPointer));
            _setLedStylePointer = (SetLedStylePointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "MLAPI_SetLedStyle"), typeof(SetLedStylePointer));
            _setLedBrightPointer = (SetLedBrightPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "MLAPI_SetLedBright"), typeof(SetLedBrightPointer));
            _setLedSpeedPointer = (SetLedSpeedPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "MLAPI_SetLedSpeed"), typeof(SetLedSpeedPointer));
            _getErrorMessagePointer = (GetErrorMessagePointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "MLAPI_GetErrorMessage"), typeof(GetErrorMessagePointer));
        }

        private static void UnloadMsiSDK()
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

        private static InitializePointer _initializePointer;
        private static GetDeviceInfoPointer _getDeviceInfoPointer;
        private static GetLedInfoPointer _getLedInfoPointer;
        private static GetLedColorPointer _getLedColorPointer;
        private static GetLedStylePointer _getLedStylePointer;
        private static GetLedMaxBrightPointer _getLedMaxBrightPointer;
        private static GetLedBrightPointer _getLedBrightPointer;
        private static GetLedMaxSpeedPointer _getLedMaxSpeedPointer;
        private static GetLedSpeedPointer _getLedSpeedPointer;
        private static SetLedColorPointer _setLedColorPointer;
        private static SetLedStylePointer _setLedStylePointer;
        private static SetLedBrightPointer _setLedBrightPointer;
        private static SetLedSpeedPointer _setLedSpeedPointer;
        private static GetErrorMessagePointer _getErrorMessagePointer;

        #endregion

        #region Delegates

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int InitializePointer();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int GetDeviceInfoPointer(out string[] pDevType, out int[] pLedCount);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int GetLedInfoPointer(string type, int index, out string pName, out string[] pLedStyles);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int GetLedColorPointer(string type, int index, out int r, out int g, out int b);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int GetLedStylePointer(string type, int index, out int style);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int GetLedMaxBrightPointer(string type, int index, out int maxLevel);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int GetLedBrightPointer(string type, int index, out int currentLevel);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int GetLedMaxSpeedPointer(string type, int index, out int maxSpeed);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int GetLedSpeedPointer(string type, int index, out int currentSpeed);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int SetLedColorPointer(string type, int index, int r, int g, int b);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int SetLedStylePointer(string type, int index, string style);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int SetLedBrightPointer(string type, int index, int level);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int SetLedSpeedPointer(string type, int index, int speed);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int GetErrorMessagePointer(int errorCode, out string pDesc);

        #endregion

        internal static int Initialize() => _initializePointer();
        internal static int GetDeviceInfo(out string[] pDevType, out int[] pLedCount) => _getDeviceInfoPointer(out pDevType, out pLedCount);
        internal static int GetLedInfo(string type, int index, out string pName, out string[] pLedStyles) => _getLedInfoPointer(type, index, out pName, out pLedStyles);
        internal static int GetLedColor(string type, int index, out int r, out int g, out int b) => _getLedColorPointer(type, index, out r, out g, out b);
        internal static int GetLedStyle(string type, int index, out int style) => _getLedStylePointer(type, index, out style);
        internal static int GetLedMaxBright(string type, int index, out int maxLevel) => _getLedMaxBrightPointer(type, index, out maxLevel);
        internal static int GetLedBright(string type, int index, out int currentLevel) => _getLedBrightPointer(type, index, out currentLevel);
        internal static int GetLedMaxSpeed(string type, int index, out int maxSpeed) => _getLedMaxSpeedPointer(type, index, out maxSpeed);
        internal static int GetLedSpeed(string type, int index, out int currentSpeed) => _getLedSpeedPointer(type, index, out currentSpeed);
        internal static int SetLedColor(string type, int index, int r, int g, int b) => _setLedColorPointer(type, index, r, g, b);
        internal static int SetLedStyle(string type, int index, string style) => _setLedStylePointer(type, index, style);
        internal static int SetLedBright(string type, int index, int level) => _setLedBrightPointer(type, index, level);
        internal static int SetLedSpeed(string type, int index, int speed) => _setLedSpeedPointer(type, index, speed);

        internal static string GetErrorMessage(int errorCode)
        {
            _getErrorMessagePointer(errorCode, out string description);
            return description;
        }

        #endregion
    }
}
