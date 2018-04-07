using System;

namespace RGB.NET.Core
{
    public interface IUpdateTrigger : IDisposable
    {
        event EventHandler<CustomUpdateData> Starting;
        event EventHandler<CustomUpdateData> Update;
    }
}
