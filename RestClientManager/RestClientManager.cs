﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace RestClientManagerNamespace
{
    public static class RestClientManager
    {
        public static RestClient Client = new RestClient("https://localhost:44388");

        public static string GetToken(string username, string password)
        {
            var request = new RestRequest("/Token", Method.POST);
            request.AddParameter("grant_type", "password");
            request.AddParameter("userName", username);
            request.AddParameter("password", password);
            var responseAsParsedJSON = JObject.Parse(Client.Execute(request).Content);

            return responseAsParsedJSON["access_token"].ToString();
        }
    }
}
