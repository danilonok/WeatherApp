using WeatherApp.Models;

namespace WeatherApp;

public partial class SettingPage : ContentPage
{
	public SettingsModel settingsModel { get; set; }
    public LocationPageModel locationPageModel { get; set; }
    private readonly ISettingsService _settingsService;

   

        
    
    public SettingPage(ISettingsService settingsService, LocationPageModel _locationModel)
	{
		InitializeComponent();
        _settingsService = settingsService;
        locationPageModel = _locationModel;
		BindingContext = _settingsService;
	}

    

    private void FahrenheitToggled(object sender, ToggledEventArgs e)
    {
        _settingsService.Fahrenheit = e.Value;
    }
    private void MilesToggled(object sender, ToggledEventArgs e)
    {
        _settingsService.MilesPerHour = e.Value;
    }

    private void Button_Pressed(object sender, EventArgs e)
    {
        _settingsService.ResetPreferences();
        locationPageModel.Locations.Clear();
        locationPageModel.RefreshCommand.Execute(null);
    }
}