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

        public event EventHandler<Exception>? Exception;

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
            catch (Exception ex)
            {
                Reset();
                Throw(ex);
                return false;
            }

            return true;
        }

        protected virtual IEnumerable<IRGBDevice> GetLoadedDevices(RGBDeviceType loadFilter)
        {
            foreach (IRGBDevice device in LoadDevices())
            {
                if (loadFilter.HasFlag(device.DeviceInfo.DeviceType))
                    yield return device;
                else
                    device.Dispose();
            }
        }

        protected abstract void InitializeSDK();

        protected abstract IEnumerable<IRGBDevice> LoadDevices();

        protected virtual IDeviceUpdateTrigger GetUpdateTrigger(int id = -1, double? updateRateHardLimit = null)
        {
            if (!UpdateTriggers.TryGetValue(id, out IDeviceUpdateTrigger? updaeTrigger))
                UpdateTriggers[id] = (updaeTrigger = new DeviceUpdateTrigger(updateRateHardLimit ?? _defaultUpdateRateHardLimit));

            return updaeTrigger;
        }

        protected virtual void Reset()
        {
            foreach (IDeviceUpdateTrigger updateTrigger in UpdateTriggers.Values)
                updateTrigger.Dispose();

            Devices = Enumerable.Empty<IRGBDevice>();
            IsInitialized = false;
        }

        protected virtual void Throw(Exception ex)
        {
            try { OnException(ex); } catch { /* we don't want to throw due to bad event handlers */ }

            if (ThrowsExceptions)
                throw ex;
        }

        protected virtual void OnException(Exception ex) => Exception?.Invoke(this, ex);

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
