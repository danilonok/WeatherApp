using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using WeatherApp.Managers;
using WeatherApp.Models;

namespace WeatherApp;

public partial class Locations : ContentPage
{
    public Models.LocationPageModel ViewModel { get; set; }

    private DataSavingManager _savingManager = new DataSavingManager();

    private readonly ISettingsService _settingsService;


    public Locations(ISettingsService settingsService, LocationPageModel _model)
	{
        
        InitializeComponent();
        _settingsService = settingsService;
        var locs = _savingManager.LoadLocations();
        ViewModel = _model;
        if (locs != null )
            ViewModel.Locations = locs;

        ViewModel.RefreshCommand = new Command(
            execute: () =>
            {
                ViewModel.IsRefreshing = false;
                RefreshViewObj.IsRefreshing = false;
                var currentContext = this.BindingContext;
                this.BindingContext = null;
                this.BindingContext = currentContext;
                UpdateTemperatures();
                
            }
        );
        
        BindingContext = ViewModel;



    }
    async void OnAddButtonClicked(object sender, EventArgs e)
    {

        string result = await DisplayPromptAsync("Add location", "What is the name of location?");

        
        (string, float, float) coordinates = GetCityInfo(result);
        if(coordinates.Item2 != 500 || coordinates.Item3 != 500)
        {
            Models.LocationModel location = new Models.LocationModel();

            location.Name = coordinates.Item1;

            location.Latitude = coordinates.Item2;
            location.Longitude = coordinates.Item3;

            ViewModel.Locations.Add(location);
            
            UpdateTemperatures();
        }
        else
        {
            await DisplayAlert("Error", "This location does not exist", "OK");
        }


    }
    public void OnDeleteSwipeItemInvoked(object sender, EventArgs e)
    {
        var item = sender as SwipeItem;

        var city = item.BindingContext as Models.LocationModel; if (city == null) { return; }

        ViewModel.Locations.Remove(city);
        _savingManager.SaveLocations(ViewModel.Locations);
        Shell.Current.DisplayAlert("Location deleted", city.Name, "OK");
        UpdateTemperatures();
    }

    public void OnEditButtonClicked(object sender, EventArgs e)
    {
        UpdateTemperatures();
    }

    public (string, float, float) GetCityInfo(string name)
    {
        using (var client = new HttpClient())
        {
            string uri = $"https://maps.googleapis.com/maps/api/geocode/json?address={name}&key=AIzaSyC3kZNCfAqLHzZ6FMbwF2wqGIWARxaGJTw";
            var endpoint = new Uri(uri);
            var result = client.GetAsync(endpoint).Result;
            var json = result.Content.ReadAsStringAsync().Result;
            if (JObject.Parse(json)["results"].Count() != 0)
            {
                float latitude = float.Parse(JObject.Parse(json)["results"][0]["geometry"]["location"]["lat"].ToString());
                float longitude = float.Parse(JObject.Parse(json)["results"][0]["geometry"]["location"]["lng"].ToString());
                string cityName = JObject.Parse(json)["results"][0]["address_components"][0]["long_name"].ToString();
                return (cityName, latitude, longitude);
            }
            else
            {
                return ("",500f, 500f);
            }
        }

    }
    public void RefreshCommand() { 
        UpdateTemperatures();
        ViewModel.IsRefreshing = false;
    }
    public void UpdateTemperatures()
    {
        foreach(var loc in ViewModel.Locations)
        {
            
            WeatherManager.GetForecastForLocation(loc, _settingsService.MilesPerHour, _settingsService.Fahrenheit);
            
        }
        _savingManager.SaveLocations(ViewModel.Locations);
    }
}