
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace WeatherApp
{
    public class DataSavingManager
    {
        
        static string fileName = FileSystem.AppDataDirectory + "/savedLocations.json";
        ObservableCollection<Models.LocationModel> Locations = new ObservableCollection<Models.LocationModel>();

        public async void SaveLocations(ObservableCollection<Models.LocationModel> locations)
        {

            var serializedData = JsonSerializer.Serialize(locations);
            await WriteToFileAsync(serializedData);
        }
        public  ObservableCollection<Models.LocationModel> LoadLocations()
        {
            if (File.Exists(fileName))
            {
                var rawData = File.ReadAllText(fileName);
                var locations = JsonSerializer.Deserialize<ObservableCollection<Models.LocationModel>>(rawData);
                return locations;
            }
            return null;
        }
        public async Task WriteToFileAsync(string content)
        {


            using (StreamWriter writer = new StreamWriter(fileName, false))
            {
                await writer.WriteAsync(content);
            }
        }
        public static void DeleteFile()
        {
            File.Delete(fileName);
            var result = File.Exists(fileName);
        }
        public async Task<string> ReadFromFileAsync()
        {

            if (File.Exists(fileName))
            {
                using (StreamReader reader = new StreamReader(fileName))
                {
                    return await reader.ReadToEndAsync();
                }
            }
            else return null;
        }
    }
}
