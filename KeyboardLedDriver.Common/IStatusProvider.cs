using System;

namespace KeyboardLedDriver.Common
{
    public interface IStatusProvider
    {
        event EventHandler<StatusChangedEventArgs> StatusChanged;

        bool StartMonitoring();

        bool StopMonitoring();
    }

    public class StatusChangedEventArgs : EventArgs
    {
        public bool IsErrorState { get; set; }
    }
}