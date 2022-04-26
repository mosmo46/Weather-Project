using Project.Weather.Dal;
using Project.Weather.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Weather.Entities
{
    public class Request
    {
        public bool continuToRun = true;

        Dictionary<string, WeatherModel> WeatherTable = new Dictionary<string, WeatherModel>();

        APICityRequest apiCityRequest = new APICityRequest();

        public void Load()
        {
            WeatherTable = apiCityRequest.Load();
        }

        public async Task<Dictionary<string, WeatherModel>> AddOrUpdateFile(string cityName)
        {
            var weatherCityData = await apiCityRequest.GetCityData(cityName);

            if (!WeatherTable.ContainsKey(weatherCityData.location.name))
            {
                WeatherTable.Add(weatherCityData.location.name, weatherCityData);
            }
            else
            {
                WeatherTable[weatherCityData.location.name] = weatherCityData;
            }

            return WeatherTable;
        }

        public void Save()
        {

            apiCityRequest.Save(WeatherTable);
        }

        public void RefreshByUser(string cityName, int numRefreshe)
        {

            Task.Factory.StartNew(async () =>
            {
                continuToRun = true;
                while (continuToRun)
                {
                    APICityRequest api = new APICityRequest();
                    WeatherModel weatherModel;
                    weatherModel = await api.GetCityData(cityName);

                    api.SaveOneCityWeather(weatherModel);

                    System.Threading.Thread.Sleep(numRefreshe * 1000);
                }
            });


        }

        public void StopRefreshUI()
        {
            continuToRun = false;
        }

        public async Task UpdateList()
        {

            var list = apiCityRequest.Load();
            APICityRequest api = new APICityRequest();
            foreach (var item in list)
            {
                var cityName = item.Value.location.name;
                var result = await api.GetCityData(cityName);

                if (!WeatherTable.ContainsKey(cityName))
                {
                    WeatherTable.Add(cityName, result);
                }
            }
            apiCityRequest.Save(WeatherTable);

        }
    }
}
