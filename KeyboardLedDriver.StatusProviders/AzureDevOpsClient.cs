using Microsoft.TeamFoundation.Build.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeyboardLedDriver.StatusProviders
{
    public class AzureDevOpsClient
    {
        private string _accessToken;

        private string _org;

        private string _project;

        private VssConnection _connection;

        /// <summary>
        /// Initializes the Azure DevOps REST API Client
        /// </summary>
        /// <param name="devOpsOrg">The name of AzDevOps Organization. E.g.</param>
        /// <param name="project">The name of the project you want to monitor</param>
        /// <param name="accessToken">Your personal access token which has the permission to READ build information.</param>
        public AzureDevOpsClient(string devOpsOrg, string project, string accessToken)
        {
            _accessToken = accessToken;
            _org = devOpsOrg;
            _project = project;
            var orgUrl = new Uri($"https://dev.azure.com/{_org}");
            _connection = new VssConnection(orgUrl, new VssBasicCredential("pat", _accessToken));

        }

        public async Task<BuildQueryResult> GetBuildStatus(string buildName)
        {
            var operationResult = await GetLatestBuildResult(buildName);
            BuildStatus status = BuildStatus.None;

            if (operationResult.HasValue)
            {
                status = TranslateBuildResult(operationResult);
            }
            
            return new BuildQueryResult()
            {
                OperationCompleted = operationResult != null, BuildStatus = status
            };

            //return (operationResult== TaskStatus.RanToCompletion, operationResult.Result == BuildResult.Succeeded);
        }

        private static BuildStatus TranslateBuildResult(BuildResult? operationResult)
        {
            BuildStatus status;

            switch (operationResult)
            {
                case BuildResult.None:
                    status = BuildStatus.None;
                    break;
                case BuildResult.Succeeded:
                    status = BuildStatus.Succeeded;
                    break;
                case BuildResult.PartiallySucceeded:
                    status = BuildStatus.PartiallySucceeded;
                    break;
                case BuildResult.Failed:
                    status = BuildStatus.Failed;
                    break;
                case BuildResult.Canceled:
                    status = BuildStatus.Canceled;
                    break;
                default:
                    status = BuildStatus.None; 
                    break;
            }

            return status;
        }

        private async Task<BuildResult?> GetLatestBuildResult(string buildName)
        {
            var client = _connection.GetClient<BuildHttpClient>();
            var buildDefinitions = await client.GetDefinitionsAsync(_project, buildName);

            if (buildDefinitions == null || buildDefinitions.Count == 0) return null;

            var builds = await client.GetBuildsAsync(_project, new List<int>(){ buildDefinitions.First().Id });

            if (builds == null || builds.Count == 0) return null;

            var latest = builds.OrderByDescending(b => b.FinishTime).First();
            return latest.Result;
        }
    }

    public class BuildQueryResult
    {
        public bool OperationCompleted { get; set; }

        public BuildStatus BuildStatus { get; set; }
    }

    public enum BuildStatus
    {
        None,
        Succeeded,
        PartiallySucceeded,
        Failed,
        Canceled
    }
}