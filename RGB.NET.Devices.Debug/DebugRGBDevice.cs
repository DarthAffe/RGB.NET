using System;
using System.Collections.Generic;
using RGB.NET.Core;
using RGB.NET.Core.Layout;

namespace RGB.NET.Devices.Debug
{
    /// <inheritdoc cref="AbstractRGBDevice{TDeviceInfo}" />
    /// <summary>
    /// Represents a debug device.
    /// </summary>
    public class DebugRGBDevice : AbstractRGBDevice<DebugRGBDeviceInfo>, IUnknownDevice
    {
        #region Properties & Fields

        /// <inheritdoc />
        public override DebugRGBDeviceInfo DeviceInfo { get; }

        private Func<Dictionary<LedId, Color>> _syncBackFunc;
        private Action<IEnumerable<Led>> _updateLedsAction;

        #endregion

        #region Constructors
        /// <summary>
        /// Internal constructor of <see cref="DebugRGBDeviceInfo"/>.
        /// </summary>
        internal DebugRGBDevice(string layoutPath, Func<Dictionary<LedId, Color>> syncBackFunc = null, Action<IEnumerable<Led>> updateLedsAction = null)
        {
            this._syncBackFunc = syncBackFunc;
            this._updateLedsAction = updateLedsAction;

            DeviceLayout layout = DeviceLayout.Load(layoutPath);
            DeviceInfo = new DebugRGBDeviceInfo(layout.Type, layout.Vendor, layout.Model, layout.Lighting, syncBackFunc != null);
        }

        #endregion

        #region Methods

        internal void Initialize(string layoutPath, string imageLayout) => ApplyLayoutFromFile(layoutPath, imageLayout, true);

        /// <inheritdoc />
        public override void SyncBack()
        {
            try
            {
                Dictionary<LedId, Color> syncBackValues = _syncBackFunc?.Invoke();
                if (syncBackValues == null) return;

                foreach (KeyValuePair<LedId, Color> value in syncBackValues)
                {
                    Led led = ((IRGBDevice)this)[value.Key];
                    SetLedColorWithoutRequest(led, value.Value);
                }
            }
            catch {/* idc that's not my fault ... */}
        }

        /// <inheritdoc />
        protected override void UpdateLeds(IEnumerable<Led> ledsToUpdate) => _updateLedsAction?.Invoke(ledsToUpdate);

        #endregion
    }
}
