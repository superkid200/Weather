using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.UI.Notifications;

namespace BackgroundTasks
{
    public sealed class WeatherBackgroundTask : IBackgroundTask
    {
        public static IAsyncOperation<RootObject> GetWeather(double lat, double lon)

        {
            return AsyncInfo.Run<RootObject>(cancellationToken => Task.Run(() =>
            {
                var http = new HttpClient();
                var response = http.GetAsync(String.Format("http://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&units=metric&appid=9e196275fa6b82e82e2c729c31d12305", lat, lon)).Result;

                var result = response.Content.ReadAsStringAsync().Result;



                return JsonConvert.DeserializeObject<RootObject>(result);
            }));


        }
        public void UpdateTile(RootObject weather)
        {
            var document = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare150x150Text01);
            document.GetElementsByTagName("text")[0].InnerText = weather.main.temp.ToString();
            document.GetElementsByTagName("text")[1].InnerText = weather.weather[0].description;
            document.GetElementsByTagName("text")[2].InnerText = weather.name;
            document.GetElementsByTagName("text")[3].InnerText = DateTime.Now.ToString("dd/MM/yyyy");
            TileUpdateManager.CreateTileUpdaterForApplication().Update(new TileNotification(
                document
                ));
        }

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
            Geolocator locator = new Geolocator();
            var position = await locator.GetGeopositionAsync();
            var weather = await GetWeather(position.Coordinate.Latitude, position.Coordinate.Longitude);
            UpdateTile(weather);
            deferral.Complete();
        }
    }
    public sealed class Coord

    {

        public double lon { get; set; }

        public double lat { get; set; }

    }



    public sealed class Weather

    {

        public int id { get; set; }

        public string main { get; set; }

        public string description { get; set; }

        public string icon { get; set; }

    }



    public sealed class Main

    {

        public int temp { get; set; }

        public int pressure { get; set; }

        public int humidity { get; set; }

        public int temp_min { get; set; }

        public int temp_max { get; set; }

    }



    public sealed class Wind

    {

        public double speed { get; set; }

        public int deg { get; set; }

    }



    public sealed class Clouds

    {

        public int all { get; set; }

    }



    public sealed class Sys

    {

        public int type { get; set; }

        public int id { get; set; }

        public double message { get; set; }

        public string country { get; set; }

        public int sunrise { get; set; }

        public int sunset { get; set; }

    }



    public sealed class RootObject

    {

        public Coord coord { get; set; }

        public IList<Weather> weather { get; set; }

        public string @base { get; set; }

        public Main main { get; set; }

        public int visibility { get; set; }

        public Wind wind { get; set; }

        public Clouds clouds { get; set; }

        public int dt { get; set; }

        public Sys sys { get; set; }

        public int id { get; set; }

        public string name { get; set; }

        public int cod { get; set; }

    }
}
