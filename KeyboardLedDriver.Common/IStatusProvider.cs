using System;

namespace KeyboardLedDriver.Common
{
    public interface IStatusProvider
    {
        event EventHandler StatusChanged;

        bool StartMonitoring();

        bool StopMonitoring();
    }
}