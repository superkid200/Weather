using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace UWPWeather.Models
{
    public class LocationManager
    {
        public static async Task<BasicGeoposition> GetPosition()
        {
            var accessStatus = await Geolocator.RequestAccessAsync();
            if (accessStatus != GeolocationAccessStatus.Allowed)
                throw new Exception(String.Format("Current location access status: ", accessStatus.ToString()));
            var locator = new Geolocator();
            var position = await locator.GetGeopositionAsync();
            return position.Coordinate.Point.Position;
        }
    }
}
