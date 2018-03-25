using System;

namespace RGB.NET.Core
{
    public interface IUpdateTrigger
    {
        event EventHandler Starting;
        event EventHandler Update;

        void TriggerHasData();
    }
}
