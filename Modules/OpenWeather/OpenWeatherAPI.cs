using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace justibot_server.Modules.OpenWeather
{
    public class OpenWeatherAPI
    {
        private string openWeatherAPIKey;

        public OpenWeatherAPI(string apiKey)
        {
            openWeatherAPIKey = apiKey;
        }       
        public async Task<double> QueryAsync(string queryStr)
        {
            Uri uri = new Uri(string.Format("http://api.openweathermap.org/data/2.5/weather?appid={0}&q={1}", openWeatherAPIKey, queryStr).ToString());
            var client = new WebClient();
            string data = await client.DownloadStringTaskAsync(uri);            
            JObject jsonData = JObject.Parse(data);
            if (jsonData.SelectToken("cod").ToString() == "200")
            {                 
                var mainData=jsonData.SelectToken("main");
                var currentTemperature=convertToCelsius(double.Parse(mainData.SelectToken("temp").ToString()));
                return currentTemperature;
            }
            else
            {
                return 0;
            }

        }        
        private double convertToCelsius(double kelvin)
        {
            return Math.Round(kelvin - 273.15, 3);
        }
    }
}
