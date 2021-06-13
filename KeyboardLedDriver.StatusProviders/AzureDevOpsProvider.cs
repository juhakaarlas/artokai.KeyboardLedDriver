using KeyboardLedDriver.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace KeyboardLedDriver.StatusProviders
{
    //TODO: Add secrets manager instructions to README
    public class AzureDevOpsProvider : IStatusProvider
    {
        public event EventHandler<StatusChangedEventArgs> StatusChanged;

        private bool _monitoring;

        private CancellationTokenSource _cts;

        private AzureDevOpsClient _client;

        public List<string> BuildNames { get; private set; }

        private bool _lastBuildStatus;

        public AzureDevOpsProvider(string devOpsOrg, string project, string accessToken)
        {
            BuildNames = new List<string>();
            _client = new AzureDevOpsClient(devOpsOrg, project, accessToken);
        }

        public bool StartMonitoring()
        {
            if (_monitoring) return true;
            _cts = new CancellationTokenSource();
            new Task(async () => await Run()).Start();
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

        private async void OnTick()
        {
            bool totalSuccess = true;

            foreach (var build in BuildNames)
            {
                var result = await _client.GetBuildStatus(build);
                totalSuccess &= result.OperationCompleted && result.BuildStatus == BuildStatus.Succeeded;
            }

            if (_lastBuildStatus != totalSuccess)
            {
                _lastBuildStatus = totalSuccess;
                StatusChanged?.Invoke(this, new StatusChangedEventArgs() { IsErrorState = !_lastBuildStatus });
            }
        }
    }
}
