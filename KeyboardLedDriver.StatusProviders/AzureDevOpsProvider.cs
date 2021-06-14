using KeyboardLedDriver.Common;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KeyboardLedDriver.StatusProviders
{
    /// <summary>
    /// <para>Provides a service to monitor builds on Azure DevOps.</para>
    /// </summary>
    public class AzureDevOpsProvider : IBuildStatusProvider
    {
        /// <inheritdoc/>
        public event EventHandler<StatusChangedEventArgs> StatusChanged;

        private CancellationTokenSource _cts;

        private AzureDevOpsClient _client;

        /// <summary>
        /// Remember the outcome of the last query.
        /// </summary>
        private bool _lastBuildStatus = true;

        /// <summary>
        /// The Azure DevOps project to monitor.
        /// </summary>
        public string Project { get => _client.Project; set => _client.Project = value; }

        /// <summary>
        /// A list of build names to monitor.
        /// </summary>
        public List<string> BuildNames { get; private set; }

        /// <inheritdoc/>
        public int PollingInterval { get; set; }

        /// <inheritdoc/>
        public bool IsMonitoring { get; private set; }


        /// <summary>
        /// Creates a new instance for monitoring an Azure DevOps project.
        /// The <paramref name="devOpsOrg"/> and <paramref name="project"/> are the values
        /// from the project URL: https://dev.azure.com/devOpsOrg/project
        /// </summary>
        /// <param name="devOpsOrg">The organization which contains the <paramref name="project"/></param>
        /// <param name="project">The project name</param>
        /// <param name="accessToken">Personal access token. This token must provide Read access to Builds.</param>
        public AzureDevOpsProvider(string devOpsOrg, string project, string accessToken)
        {
            _client = new AzureDevOpsClient(devOpsOrg, project, accessToken);
            Project = project;
            BuildNames = new List<string>();
            PollingInterval = 60;
        }

        /// <inheritdoc/>
        public bool StartMonitoring()
        {
            if (IsMonitoring) return true;
            _cts = new CancellationTokenSource();

            Run().ConfigureAwait(false);

            return IsMonitoring = true;
        }

        /// <inheritdoc/>
        public bool StopMonitoring()
        {
            _cts?.Cancel();
            IsMonitoring = false;
            return true;
        }

        private async Task Run()
        {
            if (PollingInterval == 0) return;

            while (!_cts.IsCancellationRequested)
            {
                await OnTick();
                await Task.Delay(TimeSpan.FromSeconds(PollingInterval), _cts.Token);
            }
        }

        private async Task OnTick()
        {
            if (BuildNames.Count == 0) return;

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
