using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace RGB.NET.Core
{
    public partial class RGBSurface
    {
        #region Methods

        // ReSharper disable once UnusedMember.Global
        /// <summary>
        /// Loads all devices the given <see cref="IRGBDeviceProvider"/> is able to provide.
        /// </summary>
        /// <param name="deviceProvider"></param>
        public void LoadDevices(IRGBDeviceProvider deviceProvider)
        {
            if (_deviceProvider.Contains(deviceProvider) || _deviceProvider.Any(x => x.GetType() == deviceProvider.GetType())) return;

            List<IRGBDevice> addedDevices = new List<IRGBDevice>();
            if (deviceProvider.IsInitialized || deviceProvider.Initialize())
            {
                _deviceProvider.Add(deviceProvider);

                foreach (IRGBDevice device in deviceProvider.Devices)
                {
                    if (_devices.Contains(device)) continue;

                    addedDevices.Add(device);

                    device.PropertyChanged += DeviceOnPropertyChanged;
                    device.Location.PropertyChanged += DeviceLocationOnPropertyChanged;
                    _devices.Add(device);
                }
            }

            if (addedDevices.Any())
            {
                UpdateSurfaceRectangle();
                SurfaceLayoutChanged?.Invoke(new SurfaceLayoutChangedEventArgs(addedDevices, true, false));
            }
        }

        private void DeviceOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (string.Equals(propertyChangedEventArgs.PropertyName, nameof(IRGBDevice.Location)))
            {
                UpdateSurfaceRectangle();
                SurfaceLayoutChanged?.Invoke(new SurfaceLayoutChangedEventArgs(new[] { sender as IRGBDevice }, false, true));

                ((IRGBDevice)sender).Location.PropertyChanged += DeviceLocationOnPropertyChanged;
            }
        }

        private void DeviceLocationOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            UpdateSurfaceRectangle();
            SurfaceLayoutChanged?.Invoke(new SurfaceLayoutChangedEventArgs(new[] { sender as IRGBDevice }, false, true));
        }

        #endregion
    }
}
