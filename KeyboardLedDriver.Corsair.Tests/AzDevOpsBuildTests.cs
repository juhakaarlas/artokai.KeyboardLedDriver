using KeyboardLedDriver.StatusProviders;
using KeyboardLedDriver.Tests.Helpers;
using Xunit;

namespace KeyboardLedDriver.Tests
{
    public class AzDevOpsBuildTests
    {
        private readonly string _org = Configuration.Instance.Organization;

        private readonly string _project = Configuration.Instance.Project;

        private readonly string _token = Configuration.Instance.AccessToken;

        [Fact]
        public async void CanGetBuildStatus()
        {
            var testee = new AzureDevOpsClient(_org, _project, _token);

            var result =  await testee.GetBuildStatus("My_Build");

            Assert.True(result.OperationCompleted);
            Assert.Equal(BuildStatus.Succeeded, result.BuildStatus);
        }
    }
}