using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace RestClientManagerNamespace
{
    public class RestClientManager
    {
        private static RestClientManager s_instance;
        private RestClient _client;


        public RestClient RestClient
        {
            get => _client;
        }

        public static RestClientManager GetInstance()
        {
            if (s_instance == null)
            {
                s_instance = new RestClientManager();
            }

            return s_instance;
        }

        private RestClientManager()
        {
            _client = new RestClient("https://localhost:44388/");
            string authToken = _getToken();
            _client.AddDefaultHeader("Authorization", $"Bearer {authToken}");
        }

        private string _getToken()
        {
            var request = new RestRequest("/Token", Method.POST);
            request.AddParameter("grant_type", "password");
            request.AddParameter("userName", "mail@marcuslc.com"); //Note to self: brug Web.config i stedet for!
            request.AddParameter("password", "Password1!"); //Samme her
            var responseAsParsedJSON = JObject.Parse(_client.Execute(request).Content);

            return responseAsParsedJSON["access_token"].ToString();
        }
    }
}
