using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Background;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPWeather
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Geolocator locator;
        Geoposition position;
        public MainPage()
        {
            this.InitializeComponent();
            
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if(await BackgroundExecutionManager.RequestAccessAsync() == BackgroundAccessStatus.AllowedSubjectToSystemPolicy ||
                await BackgroundExecutionManager.RequestAccessAsync() == BackgroundAccessStatus.AlwaysAllowed)
            {
                foreach(var task in BackgroundTaskRegistration.AllTasks)
                {
                    if(task.Value.Name == "WeatherBackgroundTask")
                    {
                        task.Value.Unregister(true);
                    }
                }
                BackgroundTaskBuilder builder = new BackgroundTaskBuilder();
                builder.Name = "WeatherBackgroundTask";
                builder.TaskEntryPoint = "BackgroundTasks.WeatherBackgroundTask";
                builder.SetTrigger(new TimeTrigger(30, false));
                builder.Register();
            }
            if(await Geolocator.RequestAccessAsync() == GeolocationAccessStatus.Allowed)
            {
                locator = new Geolocator();
                position = await locator.GetGeopositionAsync();
                UpdateWeather(position);
            }
        }

        public async void UpdateWeather(Geoposition position)
        {
            try
            { 
                var myWeather = await OpenWeatherMapProxy.GetWeather(position.Coordinate.Latitude,
    position.Coordinate.Longitude);
                ResultImage.Source = new BitmapImage(new Uri(
                    string.Format("ms-appx:///Assets/Weather/{0}.png", myWeather.weather[0].icon)));
                TempTextBlock.Text = myWeather.main.temp.ToString();
                DescriptionTextBlock.Text = myWeather.weather[0].description;
                LocationTextBlock.Text = myWeather.name;
                ErrorStackPanel.Visibility = Visibility.Collapsed;
                MainStackPanel.Visibility = Visibility.Visible;
            }
            catch
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    MainStackPanel.Visibility = Visibility.Collapsed;
                    ErrorStackPanel.Visibility = Visibility.Visible;
                });

            }
            
        }

    }
}
