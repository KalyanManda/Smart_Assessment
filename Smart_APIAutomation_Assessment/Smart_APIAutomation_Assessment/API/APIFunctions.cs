using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Net;
using System.IO;
using System.Configuration;
using RestSharp;

namespace Smart_APIAutomation_Assessment.API
{
    public class APIFunctions
    {
        
        private RestResponse response = null;
        private RestClient client = null;
        private RestRequest request = null;
        private string content = string.Empty;
        public RestResponse APIResponse(string httpMethod, string URI,string resource, string bearerToken , string comment)
        {
            client = new RestClient(URI);
            switch (httpMethod)
            {
                case "GET":
                    var request = new RestRequest(resource, Method.Get);
                    request.AddHeader("Authorization", bearerToken);
                    request.AddHeader("accept", "application/vnd.github.v3+json");                   
                    response = client.ExecuteAsync(request).Result;
                    content = response.get_Content();
                    break;
                case "POST":
                    request = new RestRequest(resource, Method.Post);
                    request.AddHeader("Authorization", bearerToken);
                    request.AddHeader("accept", "application/vnd.github.v3+json");
                    request.AddJsonBody(new { body = comment }, "application/json");
                    response = client.ExecuteAsync(request).Result;
                    content = response.get_Content();
                    break;

            }

            return response;
        }

    }
}
