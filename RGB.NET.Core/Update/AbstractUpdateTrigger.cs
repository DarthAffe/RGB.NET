using System;

namespace RGB.NET.Core
{
    public class AbstractUpdateTrigger : AbstractBindable, IUpdateTrigger
    {
        #region Events

        /// <inheritdoc />
        public event EventHandler<CustomUpdateData> Starting;
        /// <inheritdoc />
        public event EventHandler<CustomUpdateData> Update;

        #endregion

        #region Methods

        protected virtual void OnStartup(CustomUpdateData updateData = null) => Starting?.Invoke(this, updateData);

        protected virtual void OnUpdate(CustomUpdateData updateData = null) => Update?.Invoke(this, updateData);

        /// <inheritdoc />
        public virtual void Dispose()
        { }

        #endregion
    }
}
