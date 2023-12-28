using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace WeatherApp.Models
{
    public class Forecast
    {
        public float latitude { get; set; }
        public float longitude { get; set; }
        public Current_Weather current { get; set; }
        public Hourly hourly { get; set; }
        
        public Daily daily { get; set; }
       
    }
    public class Current_Weather
    {
        public float temperature_2m { get; set; }
        public int weather_code { get; set; }
        public float relative_humidity_2m { get; set; }
        public float wind_speed_10m { get; set; }
    }
    public class Hourly
    {
        public List<DateTime> Time { get; set; }
        public List<float> Temperature_2m { get; set; }
        
        public List<int> weather_code { get; set; }
    }
    public class Daily
    {
        public List<float> precipitation_sum { get; set; }
        public List<DateTime> sunrise {  get; set; }
        public List<DateTime> sunset { get; set; }
    }
    public class ForecastPiece
    {
        public DateTime Time { get; set; }
        public string Temperature { get; set; }
        public int Weathercode { get; set; }
    }
    public class ForecastViewModel
    {
        public string Time { get; set; }
        public string Temperature { get; set; }
        public string Weathercode { get; set; }
    }
}
