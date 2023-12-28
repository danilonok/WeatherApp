
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Reflection.Metadata;
using WeatherApp.Managers;
using WeatherApp.Models;

namespace WeatherApp
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        private readonly ISettingsService _settingsService;

        public AppDataModel ViewModel { get; set; }
        private void UpdateInfo()
        {
            WeatherManager manager = new WeatherManager(_settingsService);


            ViewModel.CurrentTemperature = Convert.ToInt32(manager.forecast.current.temperature_2m);
            ViewModel.CityName = manager.CityName;
            ViewModel.WeatherDescription = manager.WeatherDescription;
            ViewModel.WeatherInfo = new List<ForecastViewModel>();
            ViewModel.WindSpeed = Convert.ToInt32(manager.forecast.current.wind_speed_10m);
            ViewModel.WeatherIcon = manager.Icon;
            ViewModel.Humidity = manager.forecast.current.relative_humidity_2m;
            ViewModel.Precipitation = manager.Precipitation;
            ViewModel.Sunrise = manager.forecast.daily.sunrise[0].ToString("HH:mm");
            ViewModel.Sunset = manager.forecast.daily.sunset[0].ToString("HH:mm");
            foreach (var forecast in manager.WeatherData)
            {
                ViewModel.WeatherInfo.Add(new ForecastViewModel { Time = forecast.Time.ToString("HH:mm"), Temperature = forecast.Temperature.ToString(), Weathercode = WeatherManager.WeatherIcons[forecast.Weathercode] });
            }
            
            
            //ForecastView.BindingContext = model.WeatherInfo;
            //ForecastView.ItemsSource = ViewModel.WeatherInfo;

            
           
        }
        public MainPage(ISettingsService settingsService)
        {
            InitializeComponent();
            
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";

            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
            //List<Forecast> forecasts = new List<Forecast>()
            //{
            //    new Forecast {Time="10:00", Temperature="19"},
            //    new Forecast {Time="11:00", Temperature="20"},
            _settingsService = settingsService;
            //};
            ViewModel = new AppDataModel(_settingsService);

            ViewModel.RefreshCommand = new Command(
            execute: () =>
            {
                ViewModel.IsRefreshing = false;
                RefreshViewObj2.IsRefreshing = false;
                
                
                BindingContext = null;


                UpdateInfo();
                LabelTemperature.Text = $"{ViewModel.CurrentTemperature.ToString()}{ConvertToUnitF(ViewModel.Fahrenheit)}";
                LabelWind.Text = $"{ViewModel.WindSpeed.ToString()}{ConvertToUnitM(ViewModel.MilesPerHour)}";
                BindingContext = ViewModel;
               
                //ForecastView.BindingContext = model.WeatherInfo;
                ForecastView.ItemsSource = ViewModel.WeatherInfo;

               
            }
        );

            //var loc = GetCurrentLocation().Result;

            WeatherManager manager = new WeatherManager(settingsService);


            ViewModel.CurrentTemperature = Convert.ToInt32(manager.forecast.current.temperature_2m);
            ViewModel.CityName = manager.CityName;
            ViewModel.WeatherDescription = manager.WeatherDescription;
            ViewModel.WeatherInfo = new List<ForecastViewModel>();
            ViewModel.WindSpeed = Convert.ToInt32(manager.forecast.current.wind_speed_10m);
            ViewModel.WeatherIcon = manager.Icon;
            ViewModel.Humidity = manager.forecast.current.relative_humidity_2m;
            ViewModel.Precipitation = manager.Precipitation;
            ViewModel.Sunrise = manager.forecast.daily.sunrise[0].ToLocalTime().ToString("HH:mm");
            ViewModel.Sunset = manager.forecast.daily.sunset[0].ToLocalTime().ToString("HH:mm");
            foreach (var forecast in manager.WeatherData)
            {
                ViewModel.WeatherInfo.Add(new ForecastViewModel { Time=forecast.Time.ToString("HH:mm"), Temperature=forecast.Temperature.ToString(), Weathercode = WeatherManager.WeatherIcons[forecast.Weathercode] });
            }
            BindingContext = ViewModel;
            TopLayout.BindingContext = ViewModel;
            //ForecastView.BindingContext = model.WeatherInfo;
            ForecastView.ItemsSource = ViewModel.WeatherInfo;

            ExtraInfoGrid.BindingContext = ViewModel;



        }
        public string ConvertToUnitF(object value)
        {
            bool isFahrenheit = (bool)value;
            return isFahrenheit ? "°F" : "°C";
        }
        public string ConvertToUnitM(object value)
        {
            bool isMile = (bool)value;
            return isMile ? "M/h" : "Km/h";
        }

    }
}