// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using RGB.NET.Core;

namespace RGB.NET.Devices.Roccat.Native
{
    // ReSharper disable once InconsistentNaming
    internal static class _RoccatSDK
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
            UnloadCUESDK();
            LoadCUESDK();
        }

        private static void LoadCUESDK()
        {
            if (_dllHandle != IntPtr.Zero) return;

            // HACK: Load library at runtime to support both, x86 and x64 with one managed dll
            List<string> possiblePathList = Environment.Is64BitProcess ? RoccatDeviceProvider.PossibleX64NativePaths : RoccatDeviceProvider.PossibleX86NativePaths;
            string dllPath = possiblePathList.FirstOrDefault(File.Exists);
            if (dllPath == null) throw new RGBDeviceException($"Can't find the CUE-SDK at one of the expected locations:\r\n '{string.Join("\r\n", possiblePathList.Select(Path.GetFullPath))}'");

            _dllHandle = LoadLibrary(dllPath);

            _initSDKPointer = (InitSDKPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "InitSDK"), typeof(InitSDKPointer));
            _unloadSDKPointer = (UnloadSDKPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "UnloadSDK"), typeof(UnloadSDKPointer));
            _initRyosTalkPointer = (InitRyosTalkPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "init_ryos_talk"), typeof(InitRyosTalkPointer));
            _restoreLedRGBPointer = (RestoreLedRGBPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "RestoreLEDRGB"), typeof(RestoreLedRGBPointer));
            _setRyosKbSDKModePointer = (SetRyosKbSDKModePointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "set_ryos_kb_SDKmode"), typeof(SetRyosKbSDKModePointer));
            _turnOffAllLedsPointer = (TurnOffAllLedsPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "turn_off_all_LEDS"), typeof(TurnOffAllLedsPointer));
            _turnOnAllLedsPointer = (TurnOnAllLedsPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "turn_on_all_LEDS"), typeof(TurnOnAllLedsPointer));
            _setLedOnPointer = (SetLedOnPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "set_LED_on"), typeof(SetLedOnPointer));
            _setLedOffPointer = (SetLedOffPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "set_LED_off"), typeof(SetLedOffPointer));
            _setAllLedsPointer = (SetAllLedsPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "Set_all_LEDS"), typeof(SetAllLedsPointer));
            _allKeyblinkingPointer = (AllKeyblinkingPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "All_Key_Blinking"), typeof(AllKeyblinkingPointer));
            _setLedRGBPointer = (SetLedRGBPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "Set_LED_RGB"), typeof(SetLedRGBPointer));
            _setAllLedSfxPointer = (SetAllLedSfxPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "Set_all_LEDSFX"), typeof(SetAllLedSfxPointer));
        }

        private static void UnloadCUESDK()
        {
            if (_dllHandle == IntPtr.Zero) return;

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

        private static InitSDKPointer _initSDKPointer;
        private static UnloadSDKPointer _unloadSDKPointer;
        private static InitRyosTalkPointer _initRyosTalkPointer;
        private static RestoreLedRGBPointer _restoreLedRGBPointer;
        private static SetRyosKbSDKModePointer _setRyosKbSDKModePointer;
        private static TurnOffAllLedsPointer _turnOffAllLedsPointer;
        private static TurnOnAllLedsPointer _turnOnAllLedsPointer;
        private static SetLedOnPointer _setLedOnPointer;
        private static SetLedOffPointer _setLedOffPointer;
        private static SetAllLedsPointer _setAllLedsPointer;
        private static AllKeyblinkingPointer _allKeyblinkingPointer;
        private static SetLedRGBPointer _setLedRGBPointer;
        private static SetAllLedSfxPointer _setAllLedSfxPointer;

        #endregion

        #region Delegates

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr InitSDKPointer();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void UnloadSDKPointer(IntPtr handle);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool InitRyosTalkPointer(IntPtr handle);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void RestoreLedRGBPointer(IntPtr handle);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool SetRyosKbSDKModePointer(IntPtr handle, bool state);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void TurnOffAllLedsPointer(IntPtr handle);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void TurnOnAllLedsPointer(IntPtr handle);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SetLedOnPointer(IntPtr handle, byte position);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SetLedOffPointer(IntPtr handle, byte position);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SetAllLedsPointer(IntPtr handle, byte led, byte country);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void AllKeyblinkingPointer(IntPtr handle, int delayTime, int loopTime);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SetLedRGBPointer(IntPtr handle, byte zone, byte effect, byte speed, byte r, byte g, byte b);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SetAllLedSfxPointer(IntPtr handle, byte ledOnOff, byte r, byte g, byte b, byte layout);

        #endregion

        // ReSharper disable EventExceptionNotDocumented

        internal static IntPtr InitSDK() => _initSDKPointer();
        internal static void UnloadSDK(IntPtr handle) => _unloadSDKPointer(handle);
        internal static bool InitRyosTalk(IntPtr handle) => _initRyosTalkPointer(handle);
        internal static void RestoreLedRGB(IntPtr handle) => _restoreLedRGBPointer(handle);
        internal static bool SetRyosKbSDKMode(IntPtr handle, bool state) => _setRyosKbSDKModePointer(handle, state);
        internal static void TurnOffAllLeds(IntPtr handle) => _turnOffAllLedsPointer(handle);
        internal static void TurnOnAllLeds(IntPtr handle) => _turnOnAllLedsPointer(handle);
        internal static void SetLedOn(IntPtr handle, byte position) => _setLedOnPointer(handle, position);
        internal static void SetLedOff(IntPtr handle, byte position) => _setLedOffPointer(handle, position);
        internal static void SetAllLeds(IntPtr handle, byte led, byte country) => _setAllLedsPointer(handle, led, country);
        internal static void AllKeyblinking(IntPtr handle, int delayTime, int loopTime) => _allKeyblinkingPointer(handle, delayTime, loopTime);
        internal static void SetLedRGB(IntPtr handle, byte zone, byte effect, byte speed, byte r, byte g, byte b) => _setLedRGBPointer(handle, zone, effect, speed, r, g, b);
        internal static void SetAllLedSfx(IntPtr handle, byte ledOnOff, byte r, byte g, byte b, byte layout) => _setAllLedSfxPointer(handle, ledOnOff, r, g, b, layout);

        // ReSharper restore EventExceptionNotDocumented

        #endregion
    }
}
