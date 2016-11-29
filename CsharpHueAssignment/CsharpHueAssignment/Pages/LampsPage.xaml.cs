using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class LampsPage : Page
    {
        public Bridge Bridge;
        public List<HueLamp> selected = new List<HueLamp>();

        public LampsPage()
        {
            this.InitializeComponent();
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
            }
        }

        private void CheckBoxButton(object sender, RoutedEventArgs e)
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
                messageDialog.ShowAsync();
            }
        }

        private async void BlameBartButton_OnClick(object sender, RoutedEventArgs e)
        {
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
    }
}
