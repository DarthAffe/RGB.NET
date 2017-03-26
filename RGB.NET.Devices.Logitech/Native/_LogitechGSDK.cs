// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

using System;
using System.IO;
using System.Runtime.InteropServices;
using RGB.NET.Core.Exceptions;

namespace RGB.NET.Devices.Logitech.Native
{
    // ReSharper disable once InconsistentNaming
    internal class _LogitechGSDK
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
            UnloadLogitechGSDK();
            LoadLogitechGSDK();
        }

        private static void LoadLogitechGSDK()
        {
            if (_dllHandle != IntPtr.Zero) return;

            // HACK: Load library at runtime to support both, x86 and x64 with one managed dll
            string dllPath = (LoadedArchitecture = Environment.Is64BitProcess ? "x64" : "x86") + "/LogitechLedEnginesWrapper.dll";
            if (!File.Exists(dllPath))
                throw new RGBDeviceException($"Can't find the Logitech-SDK at the expected location '{Path.GetFullPath(dllPath)}'");

            _dllHandle = LoadLibrary(dllPath);

            _logiLedInitPointer = (LogiLedInitPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "LogiLedInit"), typeof(LogiLedInitPointer));
            _logiLedShutdownPointer = (LogiLedShutdownPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "LogiLedShutdown"), typeof(LogiLedShutdownPointer));
            _logiLedSetTargetDevicePointer = (LogiLedSetTargetDevicePointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "LogiLedSetTargetDevice"), typeof(LogiLedSetTargetDevicePointer));
            _logiLedGetSdkVersionPointer = (LogiLedGetSdkVersionPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "LogiLedGetSdkVersion"), typeof(LogiLedGetSdkVersionPointer));
            _lgiLedSaveCurrentLightingPointer = (LogiLedSaveCurrentLightingPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "LogiLedSaveCurrentLighting"), typeof(LogiLedSaveCurrentLightingPointer));
            _logiLedRestoreLightingPointer = (LogiLedRestoreLightingPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "LogiLedRestoreLighting"), typeof(LogiLedRestoreLightingPointer));
            _logiLedSetLightingForKeyWithKeyNamePointer = (LogiLedSetLightingForKeyWithKeyNamePointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "LogiLedSetLightingForKeyWithKeyName"), typeof(LogiLedSetLightingForKeyWithKeyNamePointer));
            _logiLedSetLightingFromBitmapPointer = (LogiLedSetLightingFromBitmapPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "LogiLedSetLightingFromBitmap"), typeof(LogiLedSetLightingFromBitmapPointer));
        }

        private static void UnloadLogitechGSDK()
        {
            if (_dllHandle == IntPtr.Zero) return;

            LogiLedShutdown();

            // ReSharper disable once EmptyEmbeddedStatement - DarthAffe 20.02.2016: We might need to reduce the internal reference counter more than once to set the library free
            while (FreeLibrary(_dllHandle)) ;
            _dllHandle = IntPtr.Zero;
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        private static extern bool FreeLibrary(IntPtr dllHandle);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetProcAddress(IntPtr dllHandle, string name);

        #endregion

        #region SDK-METHODS

        #region Pointers

        private static LogiLedInitPointer _logiLedInitPointer;
        private static LogiLedShutdownPointer _logiLedShutdownPointer;
        private static LogiLedSetTargetDevicePointer _logiLedSetTargetDevicePointer;
        private static LogiLedGetSdkVersionPointer _logiLedGetSdkVersionPointer;
        private static LogiLedSaveCurrentLightingPointer _lgiLedSaveCurrentLightingPointer;
        private static LogiLedRestoreLightingPointer _logiLedRestoreLightingPointer;
        private static LogiLedSetLightingForKeyWithKeyNamePointer _logiLedSetLightingForKeyWithKeyNamePointer;
        private static LogiLedSetLightingFromBitmapPointer _logiLedSetLightingFromBitmapPointer;

        #endregion

        #region Delegates

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool LogiLedInitPointer();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void LogiLedShutdownPointer();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool LogiLedSetTargetDevicePointer(int targetDevice);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool LogiLedGetSdkVersionPointer(ref int majorNum, ref int minorNum, ref int buildNum);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool LogiLedSaveCurrentLightingPointer();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool LogiLedRestoreLightingPointer();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool LogiLedSetLightingForKeyWithKeyNamePointer(int keyCode, int redPercentage, int greenPercentage, int bluePercentage);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool LogiLedSetLightingFromBitmapPointer(byte[] bitmap);

        #endregion

        // ReSharper disable EventExceptionNotDocumented

        internal static bool LogiLedInit()
        {
            return _logiLedInitPointer();
        }

        internal static void LogiLedShutdown()
        {
            _logiLedShutdownPointer();
        }

        internal static bool LogiLedSetTargetDevice(LogitechDeviceCaps targetDevice)
        {
            return _logiLedSetTargetDevicePointer((int)targetDevice);
        }

        internal static string LogiLedGetSdkVersion()
        {
            int major = 0;
            int minor = 0;
            int build = 0;
            LogiLedGetSdkVersion(ref major, ref minor, ref build);

            return $"{major}.{minor}.{build}";
        }

        internal static bool LogiLedGetSdkVersion(ref int majorNum, ref int minorNum, ref int buildNum)
        {
            return _logiLedGetSdkVersionPointer(ref majorNum, ref minorNum, ref buildNum);
        }

        internal static bool LogiLedSaveCurrentLighting()
        {
            return _lgiLedSaveCurrentLightingPointer();
        }

        internal static bool LogiLedRestoreLighting()
        {
            return _logiLedRestoreLightingPointer();
        }

        internal static bool LogiLedSetLightingForKeyWithKeyName(int keyCode,
            int redPercentage, int greenPercentage, int bluePercentage)
        {
            return _logiLedSetLightingForKeyWithKeyNamePointer(keyCode, redPercentage, greenPercentage, bluePercentage);
        }

        internal static bool LogiLedSetLightingFromBitmap(byte[] bitmap)
        {
            return _logiLedSetLightingFromBitmapPointer(bitmap);
        }

        // ReSharper restore EventExceptionNotDocumented

        #endregion
    }
}
