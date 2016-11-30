using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ObservableCollection<Bridge> Bridges { get; set; }

        public MainPage()
        {
            Bridges = new ObservableCollection<Bridge>();
            this.InitializeComponent();

            Bridges.Add(new Bridge($"http://localhost:8000", "Local"));
            Bridges.Add(new Bridge($"http://145.48.205.33:80", "Xplora", "iYrmsQq1wu5FxF9CPqpJCnm1GpPVylKBWDUsNDhB"));
        }

        private async void ConnectToBridgeAsync(object sender, RoutedEventArgs e)
        {
            try
            {
                // display the progress ring
                if (progressring.IsActive == true) return;

                progressring.IsActive = true;
                BridgeView.Visibility = Visibility.Collapsed;

                // Connect to the bridge and get the lamps
                var button = sender as Button;
                var bridgeName = button.Content as string;
                Bridge bridge;
                // Check if bridge is in the list and get it
                if (!GetBridge(bridgeName, out bridge))
                {
                    // TODO show message
                    return;
                }

                // Login to the bridge
                if (bridge.Username == null)
                {
                    await bridge.SetupUserNameAsync();
                }
                else
                {
                    await bridge.Login(bridge.Username);
                }
                Frame.Navigate(typeof(LampsPage), bridge);
            }
            catch (Exception exception)
            {
                var messageDialog = new MessageDialog("* Check the bridge ip\n* Check the username of the bridge\n","Failed to connect to the bridge");
                await messageDialog.ShowAsync();
            }
        }

        private bool GetBridge(string name, out Bridge bridge)
        {
            foreach (var bridgeL in Bridges)
            {
                if (bridgeL.Name == name)
                {
                    bridge = bridgeL;
                    return true;
                }
            }
            bridge = null;
            return false;
        }
    }
}
