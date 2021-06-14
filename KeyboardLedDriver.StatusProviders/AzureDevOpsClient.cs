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

        private VssConnection _connection;

        /// <summary>
        /// The Azure DevOps organization name which was passed during creation.
        /// </summary>
        public string Organization { get; private set; }

        /// <summary>
        /// Gets or sets the project name. The project must be within the given <see cref="Organization"/>
        /// </summary>
        public string Project { get; set; }

        /// <summary>
        /// Initializes the Azure DevOps REST API Client
        /// </summary>
        /// <param name="devOpsOrg">The name of AzDevOps Organization. E.g.</param>
        /// <param name="project">The name of the project you want to monitor</param>
        /// <param name="accessToken">Your personal access token which has the permission to READ build information.</param>
        public AzureDevOpsClient(string devOpsOrg, string project, string accessToken)
        {
            _accessToken = accessToken;
            Organization = devOpsOrg;
            Project = project;
            var orgUrl = new Uri($"https://dev.azure.com/{Organization}");
            _connection = new VssConnection(orgUrl, new VssBasicCredential("pat", _accessToken));
        }

        /// <summary>
        /// Returns the status of the latest build from pipeline <paramref name="buildName"/>.
        /// </summary>
        /// <param name="buildName">The name of the build pipeline in <see cref="Project"/> </param>
        /// <returns>A <see cref="BuildQueryResult"/> containing information about the operation.</returns>
        public async Task<BuildQueryResult> GetBuildStatus(string buildName)
        {
            if (string.IsNullOrEmpty(Project) || string.IsNullOrEmpty(buildName))
            {
                return new BuildQueryResult() { OperationCompleted = false, BuildStatus = BuildStatus.None };
            }

            var operationResult = await GetLatestBuildResult(buildName);
            BuildStatus status = BuildStatus.None;

            if (operationResult.HasValue)
            {
                status = MapBuildResult(operationResult);
            }
            
            return new BuildQueryResult()
            {
                OperationCompleted = operationResult != null, BuildStatus = status
            };
        }

        private async Task<BuildResult?> GetLatestBuildResult(string definitionName)
        {
            var client = _connection.GetClient<BuildHttpClient>();
            var buildDefinitions = await client.GetDefinitionsAsync(project: Project, name: definitionName, includeLatestBuilds: true);

            if (buildDefinitions == null || buildDefinitions.Count == 0) return null;

            return  buildDefinitions.First().LatestCompletedBuild?.Result;
        }

        //TODO: Replace trivial mappings with Automapper
        private static BuildStatus MapBuildResult(BuildResult? operationResult)
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
    }

    /// <summary>
    /// This DTO represents the result of a single build status query.
    /// </summary>
    public class BuildQueryResult
    {
        /// <summary>
        /// <c>true</c> if the query operation finished successfully.
        /// Otherwise <c>false</c>.
        /// </summary>
        public bool OperationCompleted { get; set; }

        /// <summary>
        /// Returns one of the possible build states if <see cref="OperationCompleted"/> is <c>true</c>.
        /// Otherwise defaults to <see cref="BuildStatus.None"/>
        /// </summary>
        public BuildStatus BuildStatus { get; set; }
    }

    /// <summary>
    /// A generic representation of the most common completed build states present in various build systems.
    /// </summary>
    public enum BuildStatus
    {
        None,
        Succeeded,
        PartiallySucceeded,
        Failed,
        Canceled
    }
}