using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using WeatherApp.Models;
using Microsoft.Maui.Devices.Sensors;
using Newtonsoft.Json.Linq;


namespace WeatherApp.Managers
{
    public class WeatherManager
    {
        public Forecast forecast;
        public string CityName;
        public string WeatherDescription;
        public List<ForecastPiece> WeatherData;
        public float Precipitation;
        public string Icon;
        public bool Fahrenheit;
        public bool Mile;
        private readonly ISettingsService _settingsService;
        public WeatherManager(ISettingsService settingsService)
        {
            _settingsService = settingsService;
            var forecastJson = GetForecast();

            forecast = JsonConvert.DeserializeObject<Forecast>(forecastJson);
            CityName = GetCityName(new Location(forecast.latitude, forecast.longitude));
            WeatherDescription = Weathers[forecast.current.weather_code];
            Icon = WeatherIcons[forecast.current.weather_code];
            Precipitation = forecast.daily.precipitation_sum.Sum();
            WeatherData = GetForecastList();


        }

        public static Dictionary<int, string> Weathers = new Dictionary<int, string>(){
            {0, "Clear sky"},
            {1, "Mainly clear"},
            {2, "Partly cloudy"},
            {3, "Overcast"},
            {45, "Fog"},
            {48, "Depositing rime fog"},
            {51, "Light drizzle"},
            {53, "Moderate drizzle"},
            {55, "Dense drizzle"},
            {56, "Light freezing drizzle"},
            {57, "Dense freezing drizzle"},
            {61, "Slight rain"},
            {63, "Moderate rain"},
            {65, "Heavy rain"},
            {66, "Light freezing rain"},
            {67, "Heavy freezing rain"},
            {71, "Slight snow fall"},
            {73, "Moderate snow fall"},
            {75, "Heavy snow fall"},
            {77, "Snow grains"},
            {80, "Slight rain shower"},
            {81, "Moderate rain shower"},
            {82, "Heavy rain shower"},
            {85, "Slight snow shower"},
            {86, "Heavy snow shower"},
            {95, "Thunderstorm"},
            {96, "Thunderstorm with slight hail"},
            {99, "Thunderstorm with heavy hail"},
};

        public static Dictionary<int, string> WeatherIcons = new Dictionary<int, string>()
        {
            {0, "sunny.png"},
            {1, "sunny.png"},
            {2, "sunny.png"},
            {3, "cloudy.png"},
            {45, "cloudy.png"},
            {48, "cloudy.png"},
            {51, "rain.png"},
            {53, "rain.png"},
            {55, "rain.png"},
            {56, "show.png"},
            {57, "show.png"},
            {61, "rain.png"},
            {63, "rain.png"},
            {65, "rain.png"},
            {66, "freezing_rain.png"},
            {67, "freezing_rain.png"},
            {71, "show.png"},
            {73, "show.png"},
            {75, "show.png"},
            {77, "show.png"},
            {80, "rain.png"},
            {81, "rain.png"},
            {82, "rain.png"},
            {85, "rain.png"},
            {86, "rain.png"},
            {95, "storm.png"},
            {96, "storm.png"},
            {99, "storm.png"},
        };
        private List<ForecastPiece> GetForecastList()
        {
            var dict = GetForecastsDictionary();



            List<ForecastPiece> forecastPieces = new List<ForecastPiece>();
            foreach (var x in forecast.hourly.Time)
            {
                forecastPieces.Add(new ForecastPiece { Time = x.ToLocalTime(), Temperature = forecast.hourly.Temperature_2m[forecast.hourly.Time.IndexOf(x)].ToString(), Weathercode = forecast.hourly.weather_code[forecast.hourly.Time.IndexOf(x)] });
            }

            forecastPieces = forecastPieces.Where(x => (DateTime.Compare(x.Time, DateTime.UtcNow.ToLocalTime()) >= 0)).Take(24).ToList();

            //forecastPieces = dict.Select(p => new ForecastPiece { Time = p.Key.ToString("HH:mm"), Temperature = p.Value }).Take(24).ToList();
            return forecastPieces;
        }
        private Dictionary<DateTime, string> GetForecastsDictionary()
        {

            Dictionary<DateTime, string> dict = forecast.hourly.Time.Zip(forecast.hourly.Temperature_2m, (k, v) => new { k, v })
              .ToDictionary(x => x.k.ToLocalTime(), x => x.v.ToString());
            var test = dict.Where(x => (DateTime.Compare(x.Key, DateTime.UtcNow) >= 0));
            dict = test.ToDictionary(x => x.Key, x => x.Value);
            return dict;
        }
        public static void GetForecastForLocation(Models.LocationModel location, bool Mile, bool Fahrehheit)
        {
            using (var client = new HttpClient())
            {
                string uri;
                string mile = "&wind_speed_unit=mph";
                string fahr = "&temperature_unit=fahrenheit";
                if (location != null)
                {
                    uri = $"https://api.open-meteo.com/v1/forecast?latitude={location.Latitude}&longitude={location.Longitude}&current=temperature_2m,relative_humidity_2m,is_day,weather_code,wind_speed_10m&daily=precipitation_sum,sunrise,sunset&hourly=temperature_2m,weather_code&start_date={DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd")}&end_date={DateTime.Now.ToLocalTime().AddDays(1).ToString("yyyy-MM-dd")}";
                    if (Mile && Fahrehheit)
                    {
                        uri = $"https://api.open-meteo.com/v1/forecast?latitude={location.Latitude}&longitude={location.Longitude}&current=temperature_2m,weather_code{fahr}{mile}";
                    }
                    if (Mile && !Fahrehheit)
                    {
                        uri = $"https://api.open-meteo.com/v1/forecast?latitude={location.Latitude}&longitude={location.Longitude}&current=temperature_2m,weather_code{mile}";

                    }
                    if (Fahrehheit && !Mile)
                    {
                        uri = $"https://api.open-meteo.com/v1/forecast?latitude={location.Latitude}&longitude={location.Longitude}&current=temperature_2m,weather_code{fahr}";

                    }
                    var endpoint = new Uri(uri);
                    var result = client.GetAsync(endpoint).Result;
                    var json = result.Content.ReadAsStringAsync().Result;

                    location.LastTemperature = float.Parse(JObject.Parse(json)["current"]["temperature_2m"].ToString());

                }
                else
                {
                    return;
                }

            }
        }
        private string GetForecast()
        {
            using (var client = new HttpClient())
            {
                float defaultLatitude = 50.1006f;
                float defaultLongitide = 8.7665f;

                Location location = GetCachedLocation().Result;
                if (location == null)
                {

                    location = GetCurrentLocation().Result;

                }
                string uri = $"https://api.open-meteo.com/v1/forecast?latitude={defaultLatitude}&longitude={defaultLongitide}&current=temperature_2m,relative_humidity_2m,is_day,weather_code,wind_speed_10m&daily=precipitation_sum,sunrise,sunset&hourly=temperature_2m,weather_code&start_date={DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd")}&end_date={DateTime.Now.ToLocalTime().AddDays(1).ToString("yyyy-MM-dd")}";

                string mile = "&wind_speed_unit=mph";
                string fahr = "&temperature_unit=fahrenheit";
                if (location != null)
                {
                    if (_settingsService.MilesPerHour && _settingsService.Fahrenheit)
                    {
                        uri = $"https://api.open-meteo.com/v1/forecast?latitude={location.Latitude}&longitude={location.Longitude}&current=temperature_2m,relative_humidity_2m,is_day,weather_code,wind_speed_10m&daily=precipitation_sum,sunrise,sunset{fahr}{mile}&hourly=temperature_2m,weather_code&start_date={DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd")}&end_date={DateTime.Now.ToLocalTime().AddDays(1).ToString("yyyy-MM-dd")}";
                    }
                    if (_settingsService.MilesPerHour && !_settingsService.Fahrenheit)
                    {
                        uri = $"https://api.open-meteo.com/v1/forecast?latitude={location.Latitude}&longitude={location.Longitude}&current=temperature_2m,relative_humidity_2m,is_day,weather_code,wind_speed_10m&daily=precipitation_sum,sunrise,sunset{mile}&hourly=temperature_2m,weather_code&start_date={DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd")}&end_date={DateTime.Now.ToLocalTime().AddDays(1).ToString("yyyy-MM-dd")}";

                    }
                    if (_settingsService.Fahrenheit && !_settingsService.MilesPerHour)
                    {
                        uri = $"https://api.open-meteo.com/v1/forecast?latitude={location.Latitude}&longitude={location.Longitude}&current=temperature_2m,relative_humidity_2m,is_day,weather_code,wind_speed_10m&daily=precipitation_sum,sunrise,sunset{fahr}&hourly=temperature_2m,weather_code&start_date={DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd")}&end_date={DateTime.Now.ToLocalTime().AddDays(1).ToString("yyyy-MM-dd")}";

                    }
                    if(!_settingsService.Fahrenheit && !_settingsService.MilesPerHour)
                        uri = $"https://api.open-meteo.com/v1/forecast?latitude={location.Latitude}&longitude={location.Longitude}&current=temperature_2m,relative_humidity_2m,is_day,weather_code,wind_speed_10m&daily=precipitation_sum,sunrise,sunset&hourly=temperature_2m,weather_code&start_date={DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd")}&end_date={DateTime.Now.ToLocalTime().AddDays(1).ToString("yyyy-MM-dd")}";
                }




                var endpoint = new Uri(uri);
                var result = client.GetAsync(endpoint).Result;
                var json = result.Content.ReadAsStringAsync().Result;
                return json;
            }
        }

        public string GetCityName(Location location)
        {
            using (var client = new HttpClient())
            {
                string uri = $"https://maps.googleapis.com/maps/api/geocode/json?latlng={location.Latitude},{location.Longitude}&key=AIzaSyC3kZNCfAqLHzZ6FMbwF2wqGIWARxaGJTw&result_type=locality";
                var endpoint = new Uri(uri);
                var result = client.GetAsync(endpoint).Result;
                var json = result.Content.ReadAsStringAsync().Result;

                string cityName = JObject.Parse(json)["results"][0]["address_components"][0]["short_name"].ToString();
                return cityName;
            }

        }
        public async Task<Location> GetCachedLocation()
        {
            try
            {
                Location location = await Geolocation.Default.GetLastKnownLocationAsync();

                if (location != null)
                    return location;
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                Console.WriteLine(fnsEx.Message);
            }
            catch (FeatureNotEnabledException fneEx)
            {
                Console.WriteLine(fneEx.Message);
            }
            catch (PermissionException pEx)
            {
                Console.WriteLine(pEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }


        private CancellationTokenSource _cancelTokenSource;
        private bool _isCheckingLocation;

        public async Task<Location> GetCurrentLocation()
        {
            try
            {
                _isCheckingLocation = true;
                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                _cancelTokenSource = new CancellationTokenSource();
                Location location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);
                return location;
            }
            catch (Exception ex)
            {
                // Unable to get location 
                Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                _isCheckingLocation = false;
            }
        }
    }
}
