using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp
{
    class SettingsService : ISettingsService, INotifyPropertyChanged
    {
        public void ResetPreferences()
        {
            Fahrenheit = false;
            MilesPerHour = false;
        }

        public bool Fahrenheit
        {
            get => Preferences.Get("Fahrenheit", false);
            set
            {
                Preferences.Set("Fahrenheit", value);
                OnPropertyChanged();
            }
        }

        public bool MilesPerHour
        {
            get => Preferences.Get("MilesPerHour", false);
            set
            {
                Preferences.Set("MilesPerHour", value);
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
