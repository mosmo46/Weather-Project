using Project.Weather.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Project.Weather.Dal
{
    public class APICityRequest
    {
        
        readonly string OneCityWeatherPathFile = @"C:\Users\User\OneDrive\Moshe Yaso Work\C#Projects\Project.Weather\Project.Weather.Ui\bin\Debug\OneCityWeather.txt";
        readonly string TableDictionaryPathFile = @"C:\Users\User\OneDrive\Moshe Yaso Work\C#Projects\Project.Weather\Project.Weather.Ui\bin\Debug\TableDictionary.txt";
        
        public async Task<WeatherModel> GetCityData(string cityName)
        {

            WeatherModel weatherData;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.weatherapi.com/");

                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage pespones = await client.GetAsync($@"v1/current.json?key=12bac74e2e2743d6961184414221304&q={cityName}& aqi=no");

                string lines = await pespones.Content.ReadAsStringAsync();

                weatherData = JsonSerializer.Deserialize<WeatherModel>(lines);

                return weatherData;

            }
        }

        public void Save(Dictionary<string, WeatherModel> data)
        {
            string dataFromDicToString = JsonSerializer.Serialize(data);

            File.WriteAllText(TableDictionaryPathFile, dataFromDicToString);
        }

        public void SaveOneCityWeather(WeatherModel data)
        {
            string res = JsonSerializer.Serialize(data);

            File.WriteAllText(OneCityWeatherPathFile, res);
        }

        public WeatherModel LoadOneCity()
        {
            string res = File.ReadAllText(OneCityWeatherPathFile);
            var data = JsonSerializer.Deserialize<WeatherModel>(res);
            return data;
        }

        public Dictionary<string, WeatherModel> Load()
        {
            string res = File.ReadAllText(TableDictionaryPathFile);
            var data = JsonSerializer.Deserialize<Dictionary<string, WeatherModel>>(res);
            return data;
        }
    }
}
