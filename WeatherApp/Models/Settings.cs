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
    public class SettingsModel : INotifyPropertyChanged
    {
        private readonly ISettingsService _settingsService;
        

        public SettingsModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }
        private bool _autoRefreshData;
        private bool _darkTheme;
        private bool _fahrenheit;


       



        
        public bool Fahrenheit
        {
            get => _settingsService.Fahrenheit;
            set
            {
                _settingsService.Fahrenheit = value;
                OnPropertyChanged();
            }
        }

        public bool MilesPerHour
        {
            get => _settingsService.MilesPerHour;
            set
            {
                _settingsService.MilesPerHour = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    
}
