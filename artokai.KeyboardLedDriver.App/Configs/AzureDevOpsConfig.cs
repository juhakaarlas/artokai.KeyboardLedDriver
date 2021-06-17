using System.Collections.Generic;

namespace KeyboardLedDriver.App.Configs
{
    public class AzureDevOpsConfig
    {
        public string DevOpsOrganization { get; set; }

        public string Project { get; set; }

        public string AccessToken { get; set; }

        public int Interval { get; set; } = 60;

        public List<string> Pipelines { get; set; }
    }
}
