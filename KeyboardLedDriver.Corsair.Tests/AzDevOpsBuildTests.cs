using KeyboardLedDriver.StatusProviders;
using KeyboardLedDriver.Tests.Helpers;
using Xunit;

namespace KeyboardLedDriver.Tests
{
    /// <summary>
    /// Precondition: To execute the tests in this class, you must store the 
    /// personal access token as a user secret with the key <c>AzDevOps:PAT</c>.
    /// To do this, execute (including the double quotes):
    /// <code>dotnet user-secrets set "AzDevOps:PAT" "your_token"</code>
    /// in the test project directory.
    /// </summary>
    public class AzDevOpsBuildTests
    {
        private readonly string _org = Configuration.Instance.Organization;

        private readonly string _project = Configuration.Instance.Project;

        private readonly string _pipeline = Configuration.Instance.Pipeline;

        private readonly string _token = Configuration.Instance.AccessToken;

        [Fact]
        public async void CanGetBuildStatus()
        {
            var testee = new AzureDevOpsClient(_org, _project, _token);

            var result =  await testee.GetBuildStatus(_pipeline);

            Assert.True(result.OperationCompleted);
            Assert.Equal(BuildStatus.Succeeded, result.BuildStatus);
        }
    }
}