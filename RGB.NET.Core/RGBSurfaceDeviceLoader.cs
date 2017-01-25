using System.ComponentModel;
using System.Linq;

namespace RGB.NET.Core
{
    public static partial class RGBSurface
    {
        #region Methods

        // ReSharper disable once UnusedMember.Global
        /// <summary>
        /// Loads all devices the given <see cref="IRGBDeviceProvider"/> is able to provide.
        /// </summary>
        /// <param name="deviceProvider"></param>
        public static void LoadDevices(IRGBDeviceProvider deviceProvider)
        {
            if (_deviceProvider.Contains(deviceProvider) || _deviceProvider.Any(x => x.GetType() == deviceProvider.GetType())) return;

            if (deviceProvider.IsInitialized || deviceProvider.Initialize())
            {
                _deviceProvider.Add(deviceProvider);

                foreach (IRGBDevice device in deviceProvider.Devices)
                {
                    if (_devices.Contains(device)) continue;

                    device.PropertyChanged += DeviceOnPropertyChanged;
                    _devices.Add(device);
                }
            }

            UpdateSurfaceRectangle();
        }

        private static void DeviceOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (string.Equals(propertyChangedEventArgs.PropertyName, nameof(IRGBDevice.Location)))
                UpdateSurfaceRectangle();
        }

        #endregion
    }
}
