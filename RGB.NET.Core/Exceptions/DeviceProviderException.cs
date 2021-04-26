using System;

namespace RGB.NET.Core
{
    public class DeviceProviderException : ApplicationException
    {
        #region Properties & Fields

        private bool IsCritical { get; }

        #endregion

        #region Constructors

        public DeviceProviderException(Exception? innerException, bool isCritical)
            : base(innerException?.Message, innerException)
        {
            this.IsCritical = isCritical;
        }

        #endregion
    }
}
