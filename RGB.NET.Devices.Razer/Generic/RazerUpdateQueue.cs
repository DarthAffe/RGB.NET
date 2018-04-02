using System;
using System.Collections.Generic;
using RGB.NET.Core;
using RGB.NET.Devices.Razer.Native;

namespace RGB.NET.Devices.Razer
{
    public abstract class RazerUpdateQueue : UpdateQueue
    {
        #region Properties & Fields

        private readonly Guid _deviceId;
        private Guid? _lastEffect;

        #endregion

        #region Constructors

        protected RazerUpdateQueue(IUpdateTrigger updateTrigger, Guid deviceId)
            : base(updateTrigger)
        {
            this._deviceId = deviceId;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void Update(Dictionary<object, Color> dataSet)
        {
            IntPtr effectParams = CreateEffectParams(dataSet);
            Guid effectId = Guid.NewGuid();
            CreateEffect(effectParams, ref effectId);

            _RazerSDK.SetEffect(effectId);

            if (_lastEffect.HasValue)
                _RazerSDK.DeleteEffect(_lastEffect.Value);

            _lastEffect = effectId;
        }

        protected virtual void CreateEffect(IntPtr effectParams, ref Guid effectId) => _RazerSDK.CreateEffect(_deviceId, _Defines.EFFECT_ID, effectParams, ref effectId);

        /// <inheritdoc />
        public override void Reset()
        {
            if (_lastEffect.HasValue)
            {
                _RazerSDK.DeleteEffect(_lastEffect.Value);
                _lastEffect = null;
            }
        }

        /// <summary>
        /// Creates the device-specific effect parameters for the led-update.
        /// </summary>
        /// <param name="dataSet">The data to be updated.</param>
        /// <returns>An <see cref="IntPtr"/> pointing to the effect parameter struct.</returns>
        protected abstract IntPtr CreateEffectParams(Dictionary<object, Color> dataSet);

        #endregion
    }
}
