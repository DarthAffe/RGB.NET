using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace RGB.NET.Core
{
    public partial class RGBSurface
    {
        #region Methods

        // ReSharper disable UnusedMember.Global
        /// <summary>
        /// Loads all devices the given by the <see cref="IRGBDeviceProvider"/> provided by the give <see cref="IRGBDeviceProviderLoader"/>.
        /// </summary>
        /// <param name="deviceProviderLoader">The <see cref="IRGBDeviceProviderLoader"/> which provides the <see cref="IRGBDeviceProvider"/> to load the devices from.</param>
        /// <param name="loadFilter">Specifies which types of devices to load.</param>
        /// <param name="exclusiveAccessIfPossible">Specifies whether the application should request exclusive access of possible or not.</param>
        /// <param name="throwExceptions">Specifies whether exception during the initialization sequence should be thrown or not.</param>
        public void LoadDevices(IRGBDeviceProviderLoader deviceProviderLoader, RGBDeviceType loadFilter = RGBDeviceType.All,
                                  bool exclusiveAccessIfPossible = false, bool throwExceptions = false)
            => LoadDevices(deviceProviderLoader.GetDeviceProvider(), loadFilter, exclusiveAccessIfPossible, throwExceptions);

        /// <summary>
        /// Loads all devices the given <see cref="IRGBDeviceProvider"/> is able to provide.
        /// </summary>
        /// <param name="deviceProvider">The <see cref="IRGBDeviceProvider"/> to load the devices from.</param>
        /// <param name="loadFilter">Specifies which types of devices to load.</param>
        /// <param name="exclusiveAccessIfPossible">Specifies whether the application should request exclusive access of possible or not.</param>
        /// <param name="throwExceptions">Specifies whether exception during the initialization sequence should be thrown or not.</param>
        public void LoadDevices(IRGBDeviceProvider deviceProvider, RGBDeviceType loadFilter = RGBDeviceType.All, bool exclusiveAccessIfPossible = false, bool throwExceptions = false)
        {
            if (_deviceProvider.Contains(deviceProvider) || _deviceProvider.Any(x => x.GetType() == deviceProvider.GetType())) return;

            List<IRGBDevice> addedDevices = new List<IRGBDevice>();
            if (deviceProvider.IsInitialized || deviceProvider.Initialize(loadFilter, exclusiveAccessIfPossible, throwExceptions))
            {
                _deviceProvider.Add(deviceProvider);

                foreach (IRGBDevice device in deviceProvider.Devices)
                {
                    if (_devices.Contains(device)) continue;

                    addedDevices.Add(device);

                    device.PropertyChanged += DeviceOnPropertyChanged;
                    _devices.Add(device);
                }
            }

            if (addedDevices.Any())
            {
                UpdateSurfaceRectangle();
                SurfaceLayoutChanged?.Invoke(new SurfaceLayoutChangedEventArgs(addedDevices, true, false));
            }
        }

        /// <summary>
        /// Automatically aligns all devices to prevent overlaps.
        /// </summary>
        public void AlignDevices()
        {
            double posX = 0;
            foreach (IRGBDevice device in Devices)
            {
                device.Location += new Point(posX, 0);
                posX += device.Size.Width + 1;
            }
        }

        // ReSharper restore UnusedMember.Global

        private void DeviceOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            UpdateSurfaceRectangle();
            SurfaceLayoutChanged?.Invoke(new SurfaceLayoutChangedEventArgs(new[] { sender as IRGBDevice }, false, true));
        }

        #endregion
    }
}
