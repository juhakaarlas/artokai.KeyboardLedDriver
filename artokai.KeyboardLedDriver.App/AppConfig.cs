using System.Collections.Generic;
using System.Configuration;
using System.IO;
using KeyboardLedDriver.App.Configs;
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
}
