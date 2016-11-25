﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public SingleLampPage()
        {

            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                HueLamp = e.Parameter as HueLamp;

                HueSlider.Value = HueLamp.Hue;
                SaturationSlider.Value = HueLamp.Saturation;
                BrightnessSlider.Value = HueLamp.Brightness;
                ColorTemperatureSlider.Value = HueLamp.Ct;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.StackTrace);   
            }
        }

        private void BackButton(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private async void HueSlider_OnPointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            var slider = sender as Slider;
            HueLamp.Hue = (int)slider.Value;
            var ip = $"{HueLamp.Bridge.Ip}/api/{HueLamp.Bridge.Username}/lights/{HueLamp.Number}/state";
            await Connection.Connection.PutAsync(ip, new { hue = HueLamp.Hue },(message => { }));
        }
    }
}
