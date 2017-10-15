using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Jupiter.Utility;
using System.Net;
using System.Net.Http.Headers;

namespace DataExport
{
    class ApiUtility
    {
        private static string token;

        private static string ApiHost = Configuration.GetApp("apiHost");

        private static void getToken()
        {
            if (!string.IsNullOrEmpty(token))
            {
                return;
            }

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ApiHost);

            var parameters = new Dictionary<string, string>();
            parameters.Add("grant_type", "client_credentials");

            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(
                        Encoding.ASCII.GetBytes(Constants.ClientID + ":" + Constants.ClientSecrect)
                        ));

            var response = httpClient.PostAsync("token", new FormUrlEncodedContent(parameters)).Result;
            var responseValue = response.Content.ReadAsStringAsync().Result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                token = JObject.Parse(responseValue)["access_token"].Value<string>();
            }
        }

        public static HttpClient GetClient()
        {
            getToken();
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
            client.BaseAddress = new Uri(ApiHost);
            return client;
        }
    }
}
