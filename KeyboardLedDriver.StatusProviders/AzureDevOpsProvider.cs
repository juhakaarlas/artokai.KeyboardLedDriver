using System;
using KeyboardLedDriver.Common;

namespace KeyboardLedDriver.StatusProviders
{
    public class AzureDevOpsProvider : IStatusProvider
    {
        public event EventHandler StatusChanged;

        private bool _monitoring;

        public AzureDevOpsProvider()
        {

        }

        public bool StartMonitoring()
        {
            _monitoring = true;

            return true;
        }

        public bool StopMonitoring()
        {
            _monitoring = false;
            return true;
        }
    }
}
