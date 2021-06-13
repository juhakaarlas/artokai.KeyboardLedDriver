using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace KeyboardLedDriver.Corsair.Tests.Helpers
{
    public class Configuration
    {
        private static Configuration _instance;
        private readonly Dictionary<string, string> _properties;
        public string EnvironmentName { get; }

        private Configuration()
        {
            EnvironmentName = Environment.GetEnvironmentVariable("EnvironmentName") ?? "dev";
            var data = ReadConfig(GetConfigFile());
            _properties = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
        }

        public static Configuration Instance => _instance ??= new Configuration();

        public string Organization => _properties["DevOpsOrganization"];
        
        public string Project => _properties["Project"];

        public string AccessToken => _properties["AccessToken"];
        
        private string ReadConfig(string file)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"{assembly.GetName().Name}.{file}";

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
            return "test.config." + (EnvironmentName == "dev" ? "dev" : "env");
        }
    }
}