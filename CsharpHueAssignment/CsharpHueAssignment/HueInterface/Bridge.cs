using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls.Primitives;
using CsharpHueAssignment.Connection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CsharpHueAssignment.HueInterface
{
    public class Bridge
    {
        public ObservableCollection<HueLamp> Lamps { get; set; } // A list containing all currently known Lamps
        public string Username { get; set; }
        public string Ip { get; set; }
        private int _lampIndex; // Used for requesting lamps. Should not be touched by anything else.

        /// <summary>
        /// Constructor for the bridge
        /// </summary>
        public Bridge(string ip)
        {
            Lamps = new ObservableCollection<HueLamp>();
            _lampIndex = 1;
            Ip = ip;
        }


        /// <summary>
        /// Use the currently assigned Ip address in order to request a user name.
        /// After doing this it will also get all the lamps.
        /// </summary>
        public async Task SetupUserNameAsync()
        {
            await Connection.Connection.PostAsync($"{Ip}/api", new {devicetype = $"MyApp#HueController"},GetUserName);

            await Connection.Connection.GetAsync($"{Ip}/api/{Username}/lights/{_lampIndex}", GetLampData);
        }

        public async Task Login(string username)
        {
            Username = username;
            await Connection.Connection.GetAsync($"{Ip}/api/{Username}/lights/{_lampIndex}", GetLampData);
        }

        /// <summary>
        /// Create a lamp from the received message, 
        /// and continue to request the next lamp untill there is no more lamps not requested.
        /// </summary>
        /// <param name="message"></param>
        public async void GetLampData(dynamic message)
        {
            Debug.WriteLine($"Received lamp data: {message}");

            var lamp = HueLamp.ParseLamp(message, this, _lampIndex);

            if (lamp == null)
            {
                _lampIndex = 1;
                return;
            }
            Lamps.Add(lamp);

            _lampIndex++;
            await Connection.Connection.GetAsync($"{Ip}/api/{Username}/lights/{_lampIndex}", GetLampData);
            Debug.WriteLine($"Lamps: {Lamps.Count}");
        }

        /// <summary>
        /// Get the user name from the message.
        /// </summary>
        /// <param name="message"></param>
        private void GetUserName(dynamic message)
        {
            Username = message[0].success.username;
            Debug.WriteLine($"Assigned username: {Username}");
        }
    }
}
