using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using CsharpHueAssignment.HueInterface;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CsharpHueAssignment.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddBridgePage : Page
    {
        public ObservableCollection<Bridge> Bridges;

        public AddBridgePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                Bridges = e.Parameter as ObservableCollection<Bridge>;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.StackTrace);
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private async void CreateBridgeAsync(object sender, RoutedEventArgs e)
        {
            Bridge bridge;

            if (UriTextBox.Text != "" && BridgeNameTextBox.Text != "")
            {
                if (UserNameTextBox.Text == "")
                {
                    Bridges.Add(bridge = new Bridge(UriTextBox.Text, BridgeNameTextBox.Text));
                }
                else
                {
                    Bridges.Add(bridge = new Bridge(UriTextBox.Text, BridgeNameTextBox.Text, UserNameTextBox.Text));
                }
            }
            else
            {
                var dialog = new MessageDialog("Failed to create bridge");
                await dialog.ShowAsync();
                return;
            }

            await DataManager.SaveObjectToXml(bridge, $"bridge_{bridge.Name}_{bridge.Ip}.xml");
            Frame.Navigate(typeof(MainPage), Bridges);
        }
    }
}
