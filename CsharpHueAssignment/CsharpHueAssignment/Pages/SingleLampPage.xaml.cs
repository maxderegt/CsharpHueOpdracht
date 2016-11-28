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
        public List<HueLamp> Lamps { get; set; }
        public HueLamp HueLamp { get; set; }

        public SingleLampPage()
        {

            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                Lamps = e.Parameter as List<HueLamp>;

                HueSlider.Value = Lamps[0].Hue;
                SaturationSlider.Value = Lamps[0].Saturation;
                BrightnessSlider.Value = Lamps[0].Brightness;
                ColorTemperatureSlider.Value = Lamps[0].Ct;
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

        // TODO integrate these methods into one method
        private async void HueSlider_OnPointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            var slider = sender as Slider;
            foreach (var hueLamp in Lamps)
            {
                hueLamp.Hue = (int)slider.Value;
                var ip = $"{hueLamp.Bridge.Ip}/api/{hueLamp.Bridge.Username}/lights/{hueLamp.Number}/state";
                await Connection.Connection.PutAsync(ip, new { hue = hueLamp.Hue }, (message => { }));
                hueLamp.UpdateRgb();
            }
        }
        
        private async void SaturationSlider_OnPointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            var slider = sender as Slider;
            foreach (var hueLamp in Lamps)
            {
                hueLamp.Saturation = (int) slider.Value;
                var ip = $"{hueLamp.Bridge.Ip}/api/{hueLamp.Bridge.Username}/lights/{hueLamp.Number}/state";
                await Connection.Connection.PutAsync(ip, new {sat = hueLamp.Saturation}, (message => { }));
                hueLamp.UpdateRgb();
            }
        }

        private async void BrightnessSlider_OnPointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            var slider = sender as Slider;
            foreach (var hueLamp in Lamps)
            {
                hueLamp.Brightness = (int) slider.Value;
                var ip = $"{hueLamp.Bridge.Ip}/api/{hueLamp.Bridge.Username}/lights/{hueLamp.Number}/state";
                await Connection.Connection.PutAsync(ip, new {bri = hueLamp.Brightness}, (message => { }));
                hueLamp.UpdateRgb();
            }
        }

        private async void ColorTemperatureSlider_OnPointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            var slider = sender as Slider;
            foreach (var hueLamp in Lamps)
            {
                hueLamp.Ct = (int) slider.Value;
                var ip = $"{hueLamp.Bridge.Ip}/api/{hueLamp.Bridge.Username}/lights/{hueLamp.Number}/state";
                await Connection.Connection.PutAsync(ip, new {ct = hueLamp.Brightness}, (message => { }));
                hueLamp.UpdateRgb();
            }
        }

        private void SliderChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (Lamps != null)
            {
                foreach (var HueLamp in Lamps)
                {
                    HueLamp.UpdateRgb();

                }
            }
        }
    }
}
