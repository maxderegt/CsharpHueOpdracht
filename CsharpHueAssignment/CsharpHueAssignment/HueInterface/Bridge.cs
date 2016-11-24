using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsharpHueAssignment.Connection;
using Newtonsoft.Json;

namespace CsharpHueAssignment.HueInterface
{
    public class Bridge
    {
        public List<HueLamp> Lamps;
        public string Username { get; set; }
        public string Ip { get; set; }
        private int _lampIndex;

        /// <summary>
        /// Constructor for the bridge, also sets up an acount with the bridge of the lamp
        /// </summary>
        public Bridge(string ip)
        {
            Lamps = new List<HueLamp>();
            _lampIndex = 1;
            Ip = ip;
            SetupUserNameAsync();
        }

        private async void SetupUserNameAsync()
        {
            await Connection.Connection.PostAsync($"{Ip}/api",new {devicetype = $"MyApp#HueController"}, GetUserName);
            
            await Connection.Connection.GetAsync($"{Ip}/api/{Username}/lights/{_lampIndex}", GetLampData);
        }

        private async void GetLampData(dynamic message)
        {
            Debug.WriteLine($"Received lamp data: {message}");

            var lamp = HueLamp.ParseLamp(message);
            if(lamp == null) return;
            Lamps.Add(lamp);

            _lampIndex++;
            await Connection.Connection.GetAsync($"{Ip}/api/{Username}/lights/{_lampIndex}", GetLampData);
            Debug.WriteLine($"Lamps: {Lamps.Count}");
        }

        private void GetUserName(dynamic message)
        {
            Username = message[0].success.username;
            Debug.WriteLine($"Assigned username: {Username}");
        }
    }
}
