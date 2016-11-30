using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using CsharpHueAssignment.Connection;
using CsharpHueAssignment.HueInterface;
using CsharpHueAssignment.Pages;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CsharpHueAssignment.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LampsPage : Page, INotifyPropertyChanged 
    {
        public Bridge Bridge;
        public List<HueLamp> selected = new List<HueLamp>();
        public bool Disco = false;
        public MediaElement Element;

        public LampsPage()
        {
            this.InitializeComponent();
            SystemNavigationManager.GetForCurrentView().BackRequested += HardwareButton_BackPressed;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                Bridge = e.Parameter as Bridge;
                DataContext = this;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.StackTrace);
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (Element != null)
            {
                Element.Stop();
            }
            base.OnNavigatingFrom(e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var templamp = (Button) sender;
            HueLamp lamp = (HueLamp) templamp.DataContext;
            List<HueLamp> lamps = new List<HueLamp>();
            lamps.Add(lamp);
            Frame.Navigate(typeof(SingleLampPage),lamps);
        }

        private void BackButton(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
                if (Element != null)
                {
                    Element.Stop();
                }
            }
        }

        private async void CheckBoxButtonAsync(object sender, RoutedEventArgs e)
        {
            //multiple lights change
            if (selected.Count > 0)
            {
                Frame.Navigate(typeof(SingleLampPage), selected);
            }
            else
            {
                var messageDialog = new MessageDialog(
                   "No Hue Lights selected",
                   "No Hue Lights selected");
                await messageDialog.ShowAsync();
            }
        }

        private async void BlameBartButton_OnClick(object sender, RoutedEventArgs e)
        {
            DiscoButton.IsEnabled = false;
            try
            {
                int i = 1;
                dynamic messag = new
                {
                    name = "#BlameBart"
                };
                foreach (var bridgeLamp in Bridge.Lamps)
                {
                    await Connection.Connection.PutAsync($"{Bridge.Ip}/api/{Bridge.Username}/lights/{i}", messag,
                        (HandleMessage) (message => { }));
                    await Connection.Connection.PutAsync($"{Bridge.Ip}/api/{Bridge.Username}/lights/{i}/state", new { hue = 65535, sat = 254, bri = 254 },
                        (HandleMessage)(message => { }));
                    bridgeLamp.Hue = 65535;
                    bridgeLamp.Saturation = 254;
                    bridgeLamp.Brightness = 254;
                    await Connection.Connection.PutAsync($"{Bridge.Ip}/api/{Bridge.Username}/lights/{i}/state", new { on = true},
                        (HandleMessage)(message => { }));
                    i++;
                }

                Bridge.Lamps.Clear(); // Clear the list before adding new lamps
                selected.Clear();
                await Connection.Connection.GetAsync($"{Bridge.Ip}/api/{Bridge.Username}/lights/1", Bridge.GetLampData);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.StackTrace);       
            }
            DiscoButton.IsEnabled = true;
        }
        
        private void SelectionClick(object sender, RoutedEventArgs e)
        {
            var templamp = (Button)sender;
            HueLamp lamp = (HueLamp)templamp.DataContext;
            if (selected.Contains(lamp))
            {
                selected.Remove(lamp);
                templamp.Content = "";
            }
            else
            {
                
                selected.Add(lamp);
                templamp.Content = "✓";
            }
        }

        private void DiscoButtonClick(object sender, RoutedEventArgs e)
        {
            Disco = !Disco;
            if (Disco)
            {
                if (Element == null)
                {
                    PlaySound();
                }
                Element.Play();
                DiscoLamp();
            }
            else
            {
                Element.Stop();
            }
        }

        public async void DiscoLamp()
        {
            // hue 0 - 65535 uint16
            // sat 0 - 254 0 = white 254 = coloured uint8
            // bri 1 - 254 1 = black 254 = coloured uint8
            Random random = new Random();
            while (Disco)
            {
                foreach (HueLamp lamp in Bridge.Lamps)
                {
                    var ip = $"{Bridge.Ip}/api/{Bridge.Username}/lights/{lamp.Number}/state";
                    int Hue = random.Next(0, 65535);
                    int saturation = random.Next(100, 254);
                    int bright = random.Next(200, 254);
                    lamp.Hue = Hue;
                    lamp.Saturation = saturation;
                    lamp.Brightness = bright;
                    lamp.UpdateRgb();
                    await Connection.Connection.PutAsync(ip, new { hue = Hue, sat = saturation , bri = bright  }, (message => { }));
                }
                NotifyPropertyChanged("RgbColor");
                await Task.Delay(TimeSpan.FromMilliseconds(500));
            }
        }

        public async void PlaySound()
        {
            Element = new MediaElement();
            var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            var file = await folder.GetFileAsync("rickastley_artists.mp3");
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            Element.SetSource(stream, "");
            Element.IsLooping = true;

        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public void HardwareButton_BackPressed(object sender, BackRequestedEventArgs e)
        {
            if (Element != null)
            {
            Element.Stop();
            }
            e.Handled = true;
            BackButton(sender, new RoutedEventArgs());
        }
    }
}
