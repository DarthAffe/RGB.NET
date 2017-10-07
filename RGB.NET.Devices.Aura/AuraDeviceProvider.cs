// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Runtime.InteropServices;
using RGB.NET.Core;
using RGB.NET.Devices.Aura.Native;

namespace RGB.NET.Devices.Aura
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a device provider responsible for Cooler Master devices.
    /// </summary>
    public class AuraDeviceProvider : IRGBDeviceProvider
    {
        #region Properties & Fields

        private static AuraDeviceProvider _instance;
        /// <summary>
        /// Gets the singleton <see cref="AuraDeviceProvider"/> instance.
        /// </summary>
        public static AuraDeviceProvider Instance => _instance ?? new AuraDeviceProvider();

        /// <summary>
        /// Gets a modifiable list of paths used to find the native SDK-dlls for x86 applications.
        /// The first match will be used.
        /// </summary>
        public static List<string> PossibleX86NativePaths { get; } = new List<string> { "x86/AURA_SDK.dll" };

        /// <summary>
        /// Gets a modifiable list of paths used to find the native SDK-dlls for x64 applications.
        /// The first match will be used.
        /// </summary>
        public static List<string> PossibleX64NativePaths { get; } = new List<string> { };

        /// <inheritdoc />
        /// <summary>
        /// Indicates if the SDK is initialized and ready to use.
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// Gets the loaded architecture (x64/x86).
        /// </summary>
        public string LoadedArchitecture => _AuraSDK.LoadedArchitecture;

        /// <inheritdoc />
        /// <summary>
        /// Gets whether the application has exclusive access to the SDK or not.
        /// </summary>
        public bool HasExclusiveAccess { get; private set; }

        /// <inheritdoc />
        public IEnumerable<IRGBDevice> Devices { get; private set; }

        /// <summary>
        /// Gets or sets a function to get the culture for a specific device.
        /// </summary>
        public Func<CultureInfo> GetCulture { get; set; } = CultureHelper.GetCurrentCulture;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AuraDeviceProvider"/> class.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if this constructor is called even if there is already an instance of this class.</exception>
        public AuraDeviceProvider()
        {
            if (_instance != null) throw new InvalidOperationException($"There can be only one instanc of type {nameof(AuraDeviceProvider)}");
            _instance = this;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public bool Initialize(bool exclusiveAccessIfPossible = false, bool throwExceptions = false)
        {
            IsInitialized = false;

            try
            {
                _AuraSDK.Reload();

                IList<IRGBDevice> devices = new List<IRGBDevice>();

                #region Mainboard

                int mainboardCount = _AuraSDK.EnumerateMbController(IntPtr.Zero, 0);
                if (mainboardCount > 0)
                {
                    IntPtr mainboardHandles = Marshal.AllocHGlobal(mainboardCount * IntPtr.Size);
                    _AuraSDK.EnumerateMbController(mainboardHandles, mainboardCount);

                    for (int i = 0; i < mainboardCount; i++)
                    {
                        IntPtr handle = Marshal.ReadIntPtr(mainboardHandles, i);
                        _AuraSDK.SetMbMode(handle, 1);
                        AuraMainboardRGBDevice device = new AuraMainboardRGBDevice(new AuraMainboardRGBDeviceInfo(RGBDeviceType.Mainboard, handle));
                        device.Initialize();
                        devices.Add(device);
                    }
                }

                #endregion

                #region Graphics cards

                //TODO DarthAffe 07.10.2017: GPU works but causes huge lags on update
                //int graphicCardCount = _AuraSDK.EnumerateGPU(IntPtr.Zero, 0);
                //if (graphicCardCount > 0)
                //{
                //    IntPtr grapicsCardHandles = Marshal.AllocHGlobal(graphicCardCount * IntPtr.Size);
                //    _AuraSDK.EnumerateGPU(grapicsCardHandles, graphicCardCount);

                //    for (int i = 0; i < graphicCardCount; i++)
                //    {
                //        IntPtr handle = Marshal.ReadIntPtr(grapicsCardHandles, i);
                //        _AuraSDK.SetGPUMode(handle, 1);
                //        AuraGraphicsCardRGBDevice device = new AuraGraphicsCardRGBDevice(new AuraGraphicsCardRGBDeviceInfo(RGBDeviceType.GraphicsCard, handle));
                //        device.Initialize();
                //        devices.Add(device);
                //    }
                //}

                #endregion

                #region Keyboard

                IntPtr keyboardHandle = Marshal.AllocHGlobal(IntPtr.Size);
                if (_AuraSDK.CreateClaymoreKeyboard(keyboardHandle))
                {
                    _AuraSDK.SetClaymoreKeyboardMode(keyboardHandle, 1);
                    AuraKeyboardRGBDevice device = new AuraKeyboardRGBDevice(new AuraKeyboardRGBDeviceInfo(RGBDeviceType.Keyboard, keyboardHandle, GetCulture()));
                    device.Initialize();
                    devices.Add(device);
                }

                #endregion

                #region Mouse

                IntPtr mouseHandle = Marshal.AllocHGlobal(IntPtr.Size);
                if (_AuraSDK.CreateRogMouse(mouseHandle))
                {
                    _AuraSDK.SetRogMouseMode(mouseHandle, 1);
                    AuraMouseRGBDevice device = new AuraMouseRGBDevice(new AuraMouseRGBDeviceInfo(RGBDeviceType.Mouse, mouseHandle));
                    device.Initialize();
                    devices.Add(device);
                }

                #endregion

                Devices = new ReadOnlyCollection<IRGBDevice>(devices);
            }
            catch
            {
                if (throwExceptions)
                    throw;
                else
                    return false;
            }

            IsInitialized = true;

            return true;
        }

        /// <inheritdoc />
        public void ResetDevices()
        {
            //TODO DarthAffe 07.10.2017: This seems to be impossible right now
        }

        #endregion
    }
}
