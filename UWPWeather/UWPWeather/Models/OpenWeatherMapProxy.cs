using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UWPWeather.Models
{
    public class OpenWeatherMapProxy
    {
        public static async Task<RootObject> GetWeather(double lat, double lon)
        {
            var http = new HttpClient();
	    //WARNING: Unspecified parameter.
	    //Replace the YOUR_KEY_HERE to your API key.
	    //NOTE: For Free and Startup accounts it will take 10 minutes or ...
	    //... 10 to 60 minutes for other accounts for newly-generated API keys to take effect.
            var response = await http.GetAsync(String.Format("http://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&units=metric&appid=YOUR_KEY_HERE", lat, lon));
            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<RootObject>(result);
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
