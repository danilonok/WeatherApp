using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Models;
using WeatherApp.Managers;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace WeatherApp
{
    public class AppDataModel
    {
        private int _currentTemperature;
        
        public int CurrentTemperature
        {
            get => _currentTemperature;
            set
            {
                _currentTemperature = value;
                OnPropertyChanged(nameof(CurrentTemperature));
            }
        }
        public string CityName { get; set; }
        private SettingsService _settingsService;
        public string WeatherDescription { get; set; }
        public float Humidity { get; set; }
        public string WeatherIcon { get; set; }
        public float Precipitation { get; set; }
        public float WindSpeed { get; set; }
        public string Sunrise { get; set; }
        public string Sunset { get; set; }
        public ICommand RefreshCommand { get; set; }
        public bool _isrefreshing = false;
        public bool IsRefreshing
        {
            get => _isrefreshing;
            set
            {
                _isrefreshing = value;
                OnPropertyChanged();
            }
        }
        public List<ForecastViewModel> WeatherInfo { get; set; }
        public AppDataModel(ISettingsService settingsService)
        {
            _settingsService = new SettingsService(); 
        }
        public bool Fahrenheit
        {
            get => _settingsService.Fahrenheit;
            set
            {
                _settingsService.Fahrenheit = value;
                OnPropertyChanged(nameof(Fahrenheit));
            }
        }
        public bool MilesPerHour
        {
            get => _settingsService.MilesPerHour;
            set
            {
                _settingsService.MilesPerHour = value;
                OnPropertyChanged(nameof(MilesPerHour));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
