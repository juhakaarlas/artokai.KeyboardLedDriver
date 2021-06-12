using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using KeyboardLedDriver.Common;

namespace KeyboardLedDriver.StatusProviders
{
    public class AzureDevOpsProvider : IStatusProvider
    {
        public event EventHandler<StatusChangedEventArgs> StatusChanged;

        private bool _monitoring;

        private CancellationTokenSource _cts;

        public AzureDevOpsProvider()
        {

        }

        public bool StartMonitoring()
        {
            if (_monitoring) return true;
            _cts = new CancellationTokenSource();
            Run();
            return true;
        }

        public bool StopMonitoring()
        {
            _cts?.Cancel();
            _monitoring = false;
            return true;
        }

        private async Task Run()
        {
            while (!_cts.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(5), _cts.Token);
                OnTick();
            }
        }

        private bool lasterror = false;
        private void OnTick()
        {
            lasterror = !lasterror;
            StatusChanged?.Invoke(this, new StatusChangedEventArgs(){ IsErrorState = lasterror });
        }
    }
}
