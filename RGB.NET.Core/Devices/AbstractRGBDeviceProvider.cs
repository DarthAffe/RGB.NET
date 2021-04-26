using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RGB.NET.Core
{
    public abstract class AbstractRGBDeviceProvider : IRGBDeviceProvider
    {
        #region Properties & Fields

        private readonly double _defaultUpdateRateHardLimit;

        public bool IsInitialized { get; protected set; }
        public bool ThrowsExceptions { get; protected set; }

        public virtual IEnumerable<IRGBDevice> Devices { get; protected set; } = Enumerable.Empty<IRGBDevice>();

        protected Dictionary<int, IDeviceUpdateTrigger> UpdateTriggers { get; } = new();

        #endregion

        #region Events

        public event EventHandler<ExceptionEventArgs>? Exception;

        #endregion

        #region Constructors

        protected AbstractRGBDeviceProvider(double defaultUpdateRateHardLimit = 0)
        {
            this._defaultUpdateRateHardLimit = defaultUpdateRateHardLimit;
        }

        #endregion

        #region Methods

        public bool Initialize(RGBDeviceType loadFilter = RGBDeviceType.All, bool throwExceptions = false)
        {
            ThrowsExceptions = throwExceptions;

            try
            {
                Reset();

                InitializeSDK();

                Devices = new ReadOnlyCollection<IRGBDevice>(GetLoadedDevices(loadFilter).ToList());

                foreach (IDeviceUpdateTrigger updateTrigger in UpdateTriggers.Values)
                    updateTrigger.Start();

                IsInitialized = true;
            }
            catch (DeviceProviderException)
            {
                Reset();
                throw;
            }
            catch (Exception ex)
            {
                Reset();
                Throw(ex, true);
                return false;
            }

            return true;
        }

        protected virtual IEnumerable<IRGBDevice> GetLoadedDevices(RGBDeviceType loadFilter)
        {
            List<IRGBDevice> devices = new();
            foreach (IRGBDevice device in LoadDevices())
            {
                try
                {
                    if (loadFilter.HasFlag(device.DeviceInfo.DeviceType))
                        devices.Add(device);
                    else
                        device.Dispose();
                }
                catch (Exception ex)
                {
                    Throw(ex);
                }
            }

            return devices;
        }

        protected abstract void InitializeSDK();

        protected abstract IEnumerable<IRGBDevice> LoadDevices();

        protected virtual IDeviceUpdateTrigger GetUpdateTrigger(int id = -1, double? updateRateHardLimit = null)
        {
            if (!UpdateTriggers.TryGetValue(id, out IDeviceUpdateTrigger? updaeTrigger))
                UpdateTriggers[id] = (updaeTrigger = CreateUpdateTrigger(id, updateRateHardLimit ?? _defaultUpdateRateHardLimit));

            return updaeTrigger;
        }

        protected virtual IDeviceUpdateTrigger CreateUpdateTrigger(int id, double updateRateHardLimit) => new DeviceUpdateTrigger(updateRateHardLimit);

        protected virtual void Reset()
        {
            foreach (IDeviceUpdateTrigger updateTrigger in UpdateTriggers.Values)
                updateTrigger.Dispose();

            Devices = Enumerable.Empty<IRGBDevice>();
            IsInitialized = false;
        }

        protected virtual void Throw(Exception ex, bool isCritical = false)
        {
            ExceptionEventArgs args = new(ex, isCritical, ThrowsExceptions);
            try { OnException(args); } catch { /* we don't want to throw due to bad event handlers */ }

            if (args.Throw)
                throw new DeviceProviderException(ex, isCritical);
        }

        protected virtual void OnException(ExceptionEventArgs args) => Exception?.Invoke(this, args);

        public virtual void Dispose()
        {
            IEnumerable<IRGBDevice> devices = Devices;
            Reset();
            foreach (IRGBDevice device in devices)
                device.Dispose();
        }

        #endregion
    }
}
