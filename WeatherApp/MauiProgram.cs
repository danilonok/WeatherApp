using Microsoft.Extensions.Logging;



namespace WeatherApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .ConfigureEssentials(essentials =>
                 {
                     essentials
                         .UseMapServiceToken("Au6rs1ijh2RwTEFlytaZxKenr1aXb408MgXvDfp4vQPBYaRM8vDwT_Rb6iR4PEJM");
                         
                 });
            builder.Services.AddSingleton<ISettingsService, SettingsService>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<SettingPage>();
            builder.Services.AddSingleton<Locations>();
            builder.Services.AddSingleton<Models.LocationPageModel>();
            builder.Services.AddSingleton<Models.LocationModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}