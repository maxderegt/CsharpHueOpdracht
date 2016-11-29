using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using Newtonsoft.Json;
using CsharpHueAssignment.Connection;
using CsharpHueAssignment.HueInterface;
using CsharpHueAssignment.Pages;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CsharpHueAssignment
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public List<Bridge> Bridges { get; set; }

        public MainPage()
        {
            Bridges = new List<Bridge>();
            this.InitializeComponent();
        }

        public async void ConnectToBridgeButtonAsync(object sender, RoutedEventArgs args)
        {
            if(ProgressRing.IsActive == true) return;

            ProgressRing.IsActive = true;
            LaButton.Visibility = Visibility.Collapsed;
            LocalButton.Visibility = Visibility.Collapsed;

            try
            {
                var bridge = new Bridge($"http://localhost:8000");
                await bridge.SetupUserNameAsync();
                Frame.Navigate(typeof(LampsPage),bridge);
            }
            catch (Exception exception)
            {
                var messageDialog = new MessageDialog(
                    /*"The application was unable to connect to the selected bridge..."*/exception.StackTrace,
                    "Failed to connect to selected bridge");
                LocalButton.Visibility = Visibility.Visible;
                LaButton.Visibility = Visibility.Visible;
                ProgressRing.IsActive = false;
                await messageDialog.ShowAsync();
            }
        }

        private async void ConnectToLaBridgeAsync(object sender, RoutedEventArgs e)
        {
            if (ProgressRing.IsActive == true) return;

            ProgressRing.IsActive = true;
            LaButton.Visibility = Visibility.Collapsed;
            LocalButton.Visibility = Visibility.Collapsed;

            try
            {
                var bridge = new Bridge($"http://145.48.205.33:80");
                await bridge.Login("iYrmsQq1wu5FxF9CPqpJCnm1GpPVylKBWDUsNDhB");
                Frame.Navigate(typeof(LampsPage), bridge);
            }
            catch (Exception exception)
            {
                var messageDialog = new MessageDialog(
                    /*"The application was unable to connect to the selected bridge..."*/exception.StackTrace,
                    "Failed to connect to selected bridge");
                LocalButton.Visibility = Visibility.Visible;
                LaButton.Visibility = Visibility.Visible;
                ProgressRing.IsActive = false;
                await messageDialog.ShowAsync();
            }
        }
    }
}
