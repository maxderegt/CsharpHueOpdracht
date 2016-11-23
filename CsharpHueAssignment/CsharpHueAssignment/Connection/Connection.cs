using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Web.Http;
using Newtonsoft.Json;

namespace CsharpHueAssignment.Connection
{
    public delegate void HandleMessage(dynamic message);

    public static class Connection
    {
        /// <summary>
        /// Send a post message to the uri and pass the response message to the handle message method.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="body"></param>
        /// <param name="handleMessage"></param>
        public static async void PostAsync(string uri, dynamic body, HandleMessage handleMessage)
        {
            var httpClient = new HttpClient();

            var bodyString = JsonConvert.SerializeObject(body);

            var httpUri = new Uri(uri);
            var httpBody = new HttpStringContent(bodyString);

            var responseMessage = await httpClient.PostAsync(httpUri, httpBody);

            var responseString = await responseMessage.Content.ReadAsStringAsync();
            
            handleMessage(JsonConvert.SerializeObject(responseString));
        }

        /// <summary>
        /// Send a get message to the uri and pass the response message to the handle message method
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="handleMessage"></param>
        public static async void GetAsync(string uri, HandleMessage handleMessage)
        {
            var httpClient = new HttpClient();

            var httpUri = new Uri(uri);

            var responseMessage = await httpClient.GetStringAsync(httpUri);

            handleMessage(JsonConvert.SerializeObject(responseMessage));
        }

        /// <summary>
        /// Send a put message to the uri and pass the response message to the handle message method
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="body"></param>
        /// <param name="handleMessage"></param>
        public static async void PutAsync(string uri, dynamic body, HandleMessage handleMessage)
        {
            var httpClient = new HttpClient();

            var bodyString = JsonConvert.SerializeObject(body);

            var httpUri = new Uri(uri);
            var httpBody = new HttpStringContent(bodyString);

            var responseMessage = await httpClient.PutAsync(httpUri, httpBody);

            var responseString = await responseMessage.Content.ReadAsStringAsync();

            handleMessage(JsonConvert.SerializeObject(responseString));
        }
    }
}
