namespace RGB.NET.Core
{
    public interface IDeviceUpdateTrigger : IUpdateTrigger
    {
        void TriggerHasData();
    }
}
