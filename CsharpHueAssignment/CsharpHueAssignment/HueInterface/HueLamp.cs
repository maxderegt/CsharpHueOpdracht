using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Microsoft.CSharp.RuntimeBinder;

namespace CsharpHueAssignment.HueInterface
{
    public class HueLamp : INotifyPropertyChanged
    {
        public bool IsOn { get; set; }
        public int Brightness { get; set; }
        public int Hue { get; set; }
        public int Saturation { get; set; }
        public float[] Xy { get; set; } // Don't know what this is for
        public int Ct { get; set; } // Color temperature
        public bool Reachable { get; set; } // wether the light can be reached by the bridge
        public string Name { get; set; }
        public string ModelId { get; set; }
        public string SwVersion { get; set; }
        public string UniqueId { get; set; }
        public int Number { get; set; }
        public Bridge Bridge { get; set; } // The bridge this light belongs to
        public SolidColorBrush RgbColor { get; set; } //rgb value of the color for the button
        
        public HueLamp(bool isOn, int brightness, int hue, int saturation, int ct, bool reachable, string name, string modelId, string swVersion, string uniqueId, int number, Bridge bridge)
        {
            IsOn = isOn;
            Brightness = brightness;
            Hue = hue;
            Saturation = saturation;
            Xy = new[] {0.0f,0.0f};
            Ct = ct;
            Reachable = reachable;
            Name = name;
            ModelId = modelId;
            SwVersion = swVersion;
            UniqueId = uniqueId;
            Bridge = bridge;
            Number = number;
            RgbColor = new SolidColorBrush(ColorUtil.HsvToRgb(hue, saturation, brightness));
        }

        public void UpdateRgb()
        {
            if (IsOn)
            {
                RgbColor = new SolidColorBrush(ColorUtil.HsvToRgb(Hue, Saturation, Brightness));
            }
            else
            {
                RgbColor = new SolidColorBrush(new Color());
            }
        }

        /// <summary>
        /// Create a HueLamp based on the data received from the bridge
        /// Will return null if parsing was unsuccessful.
        /// </summary>
        /// <param name="lampData"></param>
        /// <returns></returns>
        public static HueLamp ParseLamp(dynamic lampData, Bridge bridge, int number)
        {
            try
            {
                return new HueLamp(
                    (bool) lampData.state.on,
                    (int) lampData.state.bri,
                    (int) lampData.state.hue,
                    (int) lampData.state.sat,
                    (int) lampData.state.ct,
                    (bool) lampData.state.reachable,
                    (string) lampData.name,
                    (string) lampData.modelid,
                    (string) lampData.swversion,
                    (string) lampData.uniqueid,
                    number,
                    bridge);
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
