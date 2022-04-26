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
        
        readonly string OneCityWeather = @"C:\Users\User\OneDrive\Moshe Yaso Work\C#Projects\Project.Weather\Project.Weather.Ui\bin\Debug\OneCityWeather.txt";
        readonly string TableDictionary = @"C:\Users\User\OneDrive\Moshe Yaso Work\C#Projects\Project.Weather\Project.Weather.Ui\bin\Debug\TableDictionary.txt";
        
        public async Task<WeatherModel> GetCityData(string cityName)
        {

            WeatherModel weather;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.weatherapi.com/");

                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage pespones = await client.GetAsync($@"v1/current.json?key=12bac74e2e2743d6961184414221304&q={cityName}& aqi=no");

                string lines = await pespones.Content.ReadAsStringAsync();

                weather = JsonSerializer.Deserialize<WeatherModel>(lines);

                return weather;

            }
        }

        public void Save(Dictionary<string, WeatherModel> dic)
        {
            string dataDiC = JsonSerializer.Serialize(dic);

            File.WriteAllText(TableDictionary, dataDiC);
        }

        public void SaveOneCityWeather(WeatherModel dic)
        {
            string dataDiC = JsonSerializer.Serialize(dic);

            File.WriteAllText(OneCityWeather, dataDiC);
        }

        public WeatherModel LoadOneCity()
        {
            string res = File.ReadAllText(OneCityWeather);
            var dic = JsonSerializer.Deserialize<WeatherModel>(res);
            return dic;
        }

        public Dictionary<string, WeatherModel> Load()
        {
            string res = File.ReadAllText(TableDictionary);
            var dic = JsonSerializer.Deserialize<Dictionary<string, WeatherModel>>(res);
            return dic;
        }
    }
}
