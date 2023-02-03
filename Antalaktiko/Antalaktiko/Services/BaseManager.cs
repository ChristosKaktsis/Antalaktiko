using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Antalaktiko.Services
{
    public class BaseManager
    {
        public static readonly string BaseAddress = "https://www.antalaktiko.gr/webservice/"; 
        protected HttpClient GetClient()
        {
            HttpClient client = new HttpClient();

            var byteArray = Encoding.ASCII.GetBytes("exelixis:C9xowmTToyOjqKIhiZYe");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            return client;
        }
        protected RestClient GetRestClient()
        {
            var client = new RestClient($"{BaseAddress}scripts/add_functions.php");
            client.AddDefaultHeader("Authorization", "Basic ZXhlbGl4aXM6Qzl4b3dtVFRveU9qcUtJaGlaWWU=");
            return client;
        }
    }
}
