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
        public string Username { get; set; }
        private Connection.Connection _connection { get; set; }

        /// <summary>
        /// Constructor for the bridge, also sets up an acount with the bridge of the lamp
        /// </summary>
        public Bridge(Connection.Connection connection, string ip)
        {
            _connection = connection;
            _connection.Post($"{ip}/api",new {devicetype = $"MyApp#HueController"});

            Debug.WriteLine("hello world");
        }

        private void ReceiveAnswer()
        {
            while (true)
            {
                foreach (var message in _connection.MessageBuffer)
                {
                    var deserialisedMessage = JsonConvert.DeserializeObject(message);

                    if (IsSuccesMessage(deserialisedMessage))
                    {
                        Username = deserialisedMessage.success.username;
                    }
                }
            }
        }

        private bool IsSuccesMessage(dynamic message)
        {
            try
            {
                var temp = message.success;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
