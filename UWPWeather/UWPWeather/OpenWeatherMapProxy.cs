using Newtonsoft.Json;

using System;

using System.Collections.Generic;

using System.Linq;

using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;

using System.Threading.Tasks;
using Windows.Foundation;

namespace UWPWeather
{
    public class OpenWeatherMapProxy
    {

        public static IAsyncOperation<RootObject> GetWeather(double lat, double lon)

        {
            return AsyncInfo.Run<RootObject>(cancellationToken => Task.Run(() =>
            {
                var http = new HttpClient();
                //WARNING: No APPID included
                //Read README.MD for more details
                var response = http.GetAsync(String.Format("http://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&units=metric&appid=YOUR_KEY_HERE", lat, lon)).Result;

                var result = response.Content.ReadAsStringAsync().Result;



                return JsonConvert.DeserializeObject<RootObject>(result);
            }));


        }

        

    }
    public class Coord

    {

        public double lon { get; set; }

        public double lat { get; set; }

    }



    public class Weather

    {

        public int id { get; set; }

        public string main { get; set; }

        public string description { get; set; }

        public string icon { get; set; }

    }



    public class Main

    {

        public int temp { get; set; }

        public int pressure { get; set; }

        public int humidity { get; set; }

        public int temp_min { get; set; }

        public int temp_max { get; set; }

    }



    public class Wind

    {

        public double speed { get; set; }

        public int deg { get; set; }

    }



    public class Clouds

    {

        public int all { get; set; }

    }



    public class Sys

    {

        public int type { get; set; }

        public int id { get; set; }

        public double message { get; set; }

        public string country { get; set; }

        public int sunrise { get; set; }

        public int sunset { get; set; }

    }



    public class RootObject

    {

        public Coord coord { get; set; }

        public List<Weather> weather { get; set; }

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
