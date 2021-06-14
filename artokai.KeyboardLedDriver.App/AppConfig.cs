using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Artokai.KeyboardLedDriver
{
    public class AppConfig
    {
        private IConfiguration Configuration { get; set; }

        public static AppConfig Instance => _instance ??= new AppConfig();

        private static AppConfig _instance;

        public AzureDevOpsConfig AzDevOpsConfig
        {
            get
            {
                var config = GetSection<AzureDevOpsConfig>();
                config.AccessToken = Configuration["AzDevOpsPat"];
                return config;
            }
        } 

        private AppConfig()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddUserSecrets<AppConfig>()
                .Build();
            
            //var devOpsConfig = new AzureDevOpsConfig();
            //Configuration.Bind("AzureDevOpsConfig", devOpsConfig);
            //var joo = GetSection<AzureDevOpsConfig>();

            //var builder = new ConfigurationBuilder().AddUserSecrets<Configuration>();
            //_secretConfig = builder.Build();
            ////var data = ReadConfig(GetConfigFile());
            //_properties = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
        }

        private T GetSection<T>() where T : new()
        {
            T result = default(T);
            if (typeof(T) == typeof(ErrorPollingConfig))
                result = Configuration.GetSection("errorPolling").Get<T>();

            if (typeof(T) == typeof(AzureDevOpsConfig))
                result = Configuration.GetSection("AzureDevOpsConfig").Get<T>();

            if (result == null)
                result = new T();

            return result;
        }
    }

    public class AzureDevOpsConfig
    {
        public string DevOpsOrganization { get; set; }
        
        public string  Project { get; set; }

        public string AccessToken { get; set; }
        
        public int Interval { get; set; } = 60;

        public List<string> Pipelines { get; set; }
    }

    public class ErrorPollingConfig
    {
        public bool Enabled { get; set; } = false;
        public int Interval { get; set; } = 15;
        public string Url { get; set; } = "";
    }
}
