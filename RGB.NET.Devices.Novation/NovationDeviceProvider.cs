using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RGB.NET.Core;

namespace RGB.NET.Devices.Novation
{
    public class NovationDeviceProvider : IRGBDeviceProvider
    {
        #region Properties & Fields

        private static NovationDeviceProvider _instance;
        /// <summary>
        /// Gets the singleton <see cref="NovationDeviceProvider"/> instance.
        /// </summary>
        public static NovationDeviceProvider Instance => _instance ?? new NovationDeviceProvider();

        /// <summary>
        /// Indicates if the SDK is initialized and ready to use.
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// Gets whether the application has exclusive access to the SDK or not.
        /// </summary>
        public bool HasExclusiveAccess => false;

        /// <inheritdoc />
        public IEnumerable<IRGBDevice> Devices { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NovationDeviceProvider"/> class.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if this constructor is called even if there is already an instance of this class.</exception>
        private NovationDeviceProvider()
        {
            if (_instance != null) throw new InvalidOperationException($"There can be only one instanc of type {nameof(NovationDeviceProvider)}");
            _instance = this;
        }

        #endregion

        #region Methods

        public bool Initialize(bool exclusiveAccessIfPossible = false, bool throwExceptions = false)
        {
            IsInitialized = false;

            try
            {
                IList<IRGBDevice> devices = new List<IRGBDevice>();

                //TODO DarthAffe 15.08.2017: Get devices
                // foreach ...
                try
                {
                    NovationRGBDevice device = null;
                    device = new NovationLaunchpadRGBDevice(new NovationLaunchpadRGBDeviceInfo("Launchpad S"));
                    device.Initialize();
                    devices.Add(device);
                }
                catch
                {
                    if (throwExceptions)
                        throw;
                    //else
                    //continue;
                }

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

        public void ResetDevices()
        {
            //TODO DarthAffe 15.08.2017: Is this possible?
        }

        #endregion
    }
}
