using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace KeyboardLedDriver.Tests.Helpers
{
    public class Configuration
    {
        private static Configuration _instance;
        private readonly Dictionary<string, string> _properties;

        private IConfiguration _secretConfig;

        private Configuration()
        {
            var builder = new ConfigurationBuilder().AddUserSecrets<Configuration>();
            _secretConfig = builder.Build();
            var data = ReadConfig(GetConfigFile());
            _properties = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
        }

        public static Configuration Instance => _instance ??= new Configuration();

        public string Organization => _properties["DevOpsOrganization"];
        
        public string Project => _properties["Project"];

        public string Pipeline => _properties["Pipeline"];

        public string AccessToken => _secretConfig["AzDevOps:PAT"];
        
        private string ReadConfig(string file)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"{assembly.GetName().Name}.{file}.json";
            var localResourceName = $"{assembly.GetName().Name}.{file}.local.json";

            if (assembly.GetManifestResourceNames().Contains(localResourceName))
            {
                resourceName = localResourceName;
            }

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private string GetConfigFile()
        {
            return "test.config";
        }
    }
}