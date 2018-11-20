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
        public static ConfigModel config;
        public static void configure()
        {
            string file;
            
            file = Path.Combine(AppContext.BaseDirectory, "_config.json");

            if (!File.Exists(file))
            {
                throw new ApplicationException("Unable to locate the _config.json file.");
            }

            config = JsonConvert.DeserializeObject<ConfigModel>(File.ReadAllText(file));

            using(var db = new DataContext())
            {
                db.Database.EnsureCreated();
            }
        }
    }
}