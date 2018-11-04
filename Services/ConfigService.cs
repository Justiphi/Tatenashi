using System;
using System.IO;
using Newtonsoft.Json;

namespace Justibot.Services
{
    public class ConfigModel
    {
        public string Token { get; set; }
        public string ConnectionString { get; set; } //connection string for future database plans
    }
    public class Configuration
    {
        //configuration class to get configuration information from
        public static ConfigModel config;
        public static void configure()
        {
            string file;
            
            file = Path.Combine(AppContext.BaseDirectory, "_config.json");

            //ensures _config.json exists
            if (!File.Exists(file))
            {
                throw new ApplicationException("Unable to locate the _config.json file.");
            }

            //loads configuration from _config.json file into memory
            config = JsonConvert.DeserializeObject<ConfigModel>(File.ReadAllText(file));
        }
    }
}