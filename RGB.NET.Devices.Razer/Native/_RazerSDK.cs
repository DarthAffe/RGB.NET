// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using RGB.NET.Core;

namespace RGB.NET.Devices.Razer.Native
{
    // ReSharper disable once InconsistentNaming
    internal static class _RazerSDK
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
            UnloadRazerSDK();
            LoadRazerSDK();
        }

        private static void LoadRazerSDK()
        {
            if (_dllHandle != IntPtr.Zero) return;

            // HACK: Load library at runtime to support both, x86 and x64 with one managed dll
            List<string> possiblePathList = Environment.Is64BitProcess ? RazerDeviceProvider.PossibleX64NativePaths : RazerDeviceProvider.PossibleX86NativePaths;
            string? dllPath = possiblePathList.Select(Environment.ExpandEnvironmentVariables).FirstOrDefault(File.Exists);
            if (dllPath == null) throw new RGBDeviceException($"Can't find the Razer-SDK at one of the expected locations:\r\n '{string.Join("\r\n", possiblePathList.Select(Path.GetFullPath))}'");

            _dllHandle = LoadLibrary(dllPath);

            _initPointer = (InitPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "Init"), typeof(InitPointer));
            _unInitPointer = (UnInitPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "UnInit"), typeof(UnInitPointer));
            _queryDevicePointer = (QueryDevicePointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "QueryDevice"), typeof(QueryDevicePointer));
            _createEffectPointer = (CreateEffectPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "CreateEffect"), typeof(CreateEffectPointer));
            _createHeadsetEffectPointer = (CreateHeadsetEffectPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "CreateHeadsetEffect"), typeof(CreateHeadsetEffectPointer));
            _createChromaLinkEffectPointer = (CreateChromaLinkEffectPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "CreateChromaLinkEffect"), typeof(CreateChromaLinkEffectPointer));
            _createKeyboardEffectPointer = (CreateKeyboardEffectPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "CreateKeyboardEffect"), typeof(CreateKeyboardEffectPointer));
            _createMousepadEffectPointer = (CreateMousepadEffectPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "CreateMousepadEffect"), typeof(CreateMousepadEffectPointer));
            _setEffectPointer = (SetEffectPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "SetEffect"), typeof(SetEffectPointer));
            _deleteEffectPointer = (DeleteEffectPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "DeleteEffect"), typeof(DeleteEffectPointer));
        }

        internal static void UnloadRazerSDK()
        {
            if (_dllHandle == IntPtr.Zero) return;

            // ReSharper disable once EmptyEmbeddedStatement - DarthAffe 09.11.2017: We might need to reduce the internal reference counter more than once to set the library free
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

        private static InitPointer _initPointer;
        private static UnInitPointer _unInitPointer;
        private static QueryDevicePointer _queryDevicePointer;
        private static CreateEffectPointer _createEffectPointer;
        private static CreateHeadsetEffectPointer _createHeadsetEffectPointer;
        private static CreateChromaLinkEffectPointer _createChromaLinkEffectPointer;
        private static CreateKeyboardEffectPointer _createKeyboardEffectPointer;
        private static CreateMousepadEffectPointer _createMousepadEffectPointer;
        private static SetEffectPointer _setEffectPointer;
        private static DeleteEffectPointer _deleteEffectPointer;

        #endregion

        #region Delegates

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate RazerError InitPointer();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate RazerError UnInitPointer();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate RazerError QueryDevicePointer(Guid deviceId, IntPtr deviceInfo);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate RazerError CreateEffectPointer(Guid deviceId, int effectType, IntPtr param, ref Guid effectId);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate RazerError CreateHeadsetEffectPointer(int effectType, IntPtr param, ref Guid effectId);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate RazerError CreateChromaLinkEffectPointer(int effectType, IntPtr param, ref Guid effectId);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate RazerError CreateKeyboardEffectPointer(int effectType, IntPtr param, ref Guid effectId);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate RazerError CreateMousepadEffectPointer(int effectType, IntPtr param, ref Guid effectId);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate RazerError SetEffectPointer(Guid effectId);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate RazerError DeleteEffectPointer(Guid effectId);

        #endregion

        // ReSharper disable EventExceptionNotDocumented

        /// <summary>
        /// Razer-SDK: Initialize Chroma SDK.
        /// </summary>
        internal static RazerError Init() => _initPointer();

        /// <summary>
        /// Razer-SDK: UnInitialize Chroma SDK.
        /// </summary>
        internal static RazerError UnInit() => _unInitPointer();

        /// <summary>
        /// Razer-SDK: Query for device information. 
        /// </summary>
        internal static RazerError QueryDevice(Guid deviceId, out _DeviceInfo deviceInfo)
        {
            int structSize = Marshal.SizeOf(typeof(_DeviceInfo));
            IntPtr deviceInfoPtr = Marshal.AllocHGlobal(structSize);

            RazerError error = _queryDevicePointer(deviceId, deviceInfoPtr);

            deviceInfo = (_DeviceInfo)Marshal.PtrToStructure(deviceInfoPtr, typeof(_DeviceInfo));
            Marshal.FreeHGlobal(deviceInfoPtr);

            return error;
        }

        internal static RazerError CreateEffect(Guid deviceId, int effectType, IntPtr param, ref Guid effectId) => _createEffectPointer(deviceId, effectType, param, ref effectId);

        internal static RazerError CreateHeadsetEffect(int effectType, IntPtr param, ref Guid effectId) => _createHeadsetEffectPointer(effectType, param, ref effectId);

        internal static RazerError CreateChromaLinkEffect(int effectType, IntPtr param, ref Guid effectId) => _createChromaLinkEffectPointer(effectType, param, ref effectId);

        internal static RazerError CreateKeyboardEffect(int effectType, IntPtr param, ref Guid effectId) => _createKeyboardEffectPointer(effectType, param, ref effectId);

        internal static RazerError CreateMousepadEffect(int effectType, IntPtr param, ref Guid effectId) => _createMousepadEffectPointer(effectType, param, ref effectId);

        internal static RazerError SetEffect(Guid effectId) => _setEffectPointer(effectId);

        internal static RazerError DeleteEffect(Guid effectId) => _deleteEffectPointer(effectId);

        // ReSharper restore EventExceptionNotDocumented

        #endregion
    }
}
