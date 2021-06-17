using System;
using KeyboardLedDriver.Common;

namespace KeyboardLedDriver.StatusProviders
{
    public abstract class PollingProviderBase
    {
        public int PollingInterval { get; set; }
    }
}