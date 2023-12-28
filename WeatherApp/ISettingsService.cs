using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp
{
    public interface ISettingsService
    {
        void ResetPreferences();
        bool Fahrenheit { get; set; }
        bool MilesPerHour { get; set; }

    }
}
