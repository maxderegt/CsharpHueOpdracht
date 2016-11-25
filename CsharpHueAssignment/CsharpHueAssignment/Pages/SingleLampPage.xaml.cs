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
using CsharpHueAssignment.HueInterface;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
// hue 0 - 65535 uint16
// sat 0 - 254 0 = white 254 = coloured uint8
// bri 1 - 254 1 = black 254 = coloured uint8

namespace CsharpHueAssignment.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SingleLampPage : Page
    {
        public HueLamp HueLamp { get; set; }

        public SingleLampPage(HueLamp hueLamp)
        {
            HueLamp = hueLamp;

            HueSlider.Value = HueLamp.Hue;
            SaturationSlider.Value = HueLamp.Saturation;
            BrightnessSlider.Value = HueLamp.Brightness;
            ColorTemperatureSlider.Value = HueLamp.Ct;

            this.InitializeComponent();
        }

        private void BackButton(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
    }
}
