using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using UWPWeather.Models;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Notifications;
using System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPWeather
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ErrorStackPanel.Visibility = Visibility.Collapsed;
                var position = await LocationManager.GetPosition();
                var latitude = position.Latitude;
                var longitude = position.Longitude;
                var myWeather = await OpenWeatherMapProxy.GetWeather(latitude, longitude);
                TempTextBlock.Text = myWeather.main.temp.ToString();
                DescriptionTextBlock.Text = myWeather.weather[0].description;
                LocationTextBlock.Text = myWeather.name;
                ResultImage.Source = new BitmapImage(new Uri(String.Format("ms-appx:///Assets/Weather/{0}.png", myWeather.weather[0].icon)));
                var updater = TileUpdateManager.CreateTileUpdaterForApplication();
		//WARNING: Test code
		//The application URL must be your published URL
                var url = String.Format("http://localhost/Weather/?lat={0}&lon={1}", latitude, longitude);
                var tileContent = new Uri(url);
                updater.StartPeriodicUpdate(tileContent, PeriodicUpdateRecurrence.HalfHour);
            }
            catch
            {
                ErrorStackPanel.Visibility = Visibility.Visible;
            }
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            var uri = new Uri(((AppBarButton)sender).Tag.ToString());
            var success = await Windows.System.Launcher.LaunchUriAsync(uri);
            if(success) { Debug.WriteLine("Navigation completed"); }
            else
            {
                Debug.WriteLine("Navigation failed");
            }
        }
    }
}
