using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WeatherApp.Models
{
    public class LocationModel : INotifyPropertyChanged
    {
        private string _name;
        private float _lastTemperature;
        private float _longitude;
        private float _latitude;
        

        public LocationModel()
        {
          
           
        }
        
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public float LastTemperature
        {
            get => _lastTemperature;
            set
            {
                _lastTemperature = value;
                OnPropertyChanged();
            }
        }
        public float Longitude
        {
            get => _longitude;
            set
            {
                _longitude = value;
                OnPropertyChanged();
            }
        }
        public float Latitude
        {
            get => _latitude;
            set
            {
                _latitude = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class LocationPageModel
    {
        public ObservableCollection<LocationModel> Locations { get; set; }
        public bool _isrefreshing = false;
        public ICommand RefreshCommand { get; set; }
        private SettingsService _settingsService;
        public bool IsRefreshing
        {
            get => _isrefreshing;
            set
            {
                _isrefreshing = value;
                OnPropertyChanged();
            }
        }
        

        
        public LocationPageModel(ISettingsService settingsService)
        {
            Locations = new ObservableCollection<LocationModel>();
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
