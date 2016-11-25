using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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

        public async void ConnectToBridgeButton(object sender, RoutedEventArgs args)
        {
            // TODO add cool windows spinning loading incon, and freeze all other application controls.(probably overlay a transparent panel)

            try
            {
                var bridge = new Bridge($"http://localhost:8000");
                await bridge.SetupUserNameAsync();
                Frame.Navigate(typeof(LampsPage));
            }
            catch (Exception exception)
            {
                var messageDialog = new MessageDialog(
                    "The application was unable to connect to the selected bridge...",
                    "Failed to connect to selected bridge");
                await messageDialog.ShowAsync();
            }
        }
    }
}
