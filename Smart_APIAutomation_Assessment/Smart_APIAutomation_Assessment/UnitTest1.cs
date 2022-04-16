using System;
using NUnit.Framework;
using System.Configuration;
using RestSharp;
using System.Net;

namespace Smart_APIAutomation_Assessment
{
    [TestFixture]
    public class GistComments
    {
       
        /// <summary>
        /// Create Gist comment to an existing Gist
        /// </summary>
        [Test]
        public void CreateGistComment()
        {
            string URI = ConfigurationManager.AppSettings["URI"];
            string resource = ConfigurationManager.AppSettings["commentsResource"];
            API.APIFunctions apiFunctions = new API.APIFunctions();
            RestResponse response = apiFunctions.APIResponse("POST", URI, resource, ConfigurationManager.AppSettings["BearerToken"], DateTime.UtcNow.ToString("MMddyyyhhmmss"));
            Console.WriteLine("response content : " + response.get_Content());
            Console.WriteLine("response Status : " + response.get_StatusCode());
            Assert.True(response.get_StatusCode() == HttpStatusCode.Created, "Failed to create a comment under Gist");
            Newtonsoft.Json.Linq.JObject jObject = Newtonsoft.Json.Linq.JObject.Parse(response.get_Content());
            string commentId = jObject["id"].ToString();
            string comment = jObject["body"].ToString();
            string apiurl_getComment = jObject["url"].ToString();
            RestResponse getresponse = apiFunctions.APIResponse("GET", URI, resource+"/"+commentId, ConfigurationManager.AppSettings["BearerToken"], "");

            Console.WriteLine("response content : " + getresponse.get_Content());
            Console.WriteLine("response Status : " + getresponse.get_StatusCode());
            Assert.True(getresponse.get_StatusCode() == HttpStatusCode.OK, "Failed to get the created comment under Gist");

        }

        /// <summary>
        /// Create Gist comment to an existing Gist With Incorrect AuthenticationType i.e., by using Basic authentication
        /// </summary>
        [Test]
        public void CreateGistCommentWithIncorrectAuthenticationType()
        {
            string URI = ConfigurationManager.AppSettings["URI"];
            string resource = ConfigurationManager.AppSettings["commentsResource"];
            API.APIFunctions apiFunctions = new API.APIFunctions();
            RestResponse response = apiFunctions.APIResponse("POST", URI, resource, "Basic Authentication", DateTime.UtcNow.ToString("MMddyyyhhmmss"));
            Console.WriteLine("response content : " + response.get_Content());
            Console.WriteLine("response Status : " + response.get_StatusCode());
            Assert.True(response.get_StatusCode() == HttpStatusCode.Unauthorized, "Status Code mismatch");
            Assert.True(response.get_Content().Contains("Requires authentication"), "response message Mismatch, Expected: Requires Authentication; Actual : "+ response.get_Content());

        }

        /// <summary>
        /// Create Gist comment to an existing Gist with Incorrect Authentication Token i.e., by using incorrect/invalid bearer token
        /// </summary>
        [Test]
        public void CreateGistCommentWithIncorrectAuthenticationToken()
        {
            string URI = ConfigurationManager.AppSettings["URI"];
            string resource = ConfigurationManager.AppSettings["commentsResource"];
            API.APIFunctions apiFunctions = new API.APIFunctions();
            RestResponse response = apiFunctions.APIResponse("POST", URI, resource, "Bearer Authentication", DateTime.UtcNow.ToString("MMddyyyhhmmss"));
            Console.WriteLine("response content : " + response.get_Content());
            Console.WriteLine("response Status : " + response.get_StatusCode());
            Assert.True(response.get_StatusCode() == HttpStatusCode.Unauthorized, "Status Code mismatch");
            Assert.True(response.get_Content().Contains("Bad credentials"), "response message Mismatch, Expected: Bad credentials; Actual : " + response.get_Content());
        }

        /// <summary>
        /// Create Gist Comment With Less Privilege Authentication Token, i.e., with no write permission only read permissions
        /// </summary>
        [Test]
        public void CreateGistCommentWithLessPrivilegeAuthenticationToken()
        {
            string URI = ConfigurationManager.AppSettings["URI"];
            string resource = ConfigurationManager.AppSettings["commentsResource"];
            API.APIFunctions apiFunctions = new API.APIFunctions();
            RestResponse response = apiFunctions.APIResponse("POST", URI, resource, ConfigurationManager.AppSettings["LessPrivilegeBearerToken"], DateTime.UtcNow.ToString("MMddyyyhhmmss"));
            Console.WriteLine("response content : " + response.get_Content());
            Console.WriteLine("response Status : " + response.get_StatusCode());
            Assert.True(response.get_StatusCode() == HttpStatusCode.NotFound, "Status Code mismatch");
            Assert.True(response.get_Content().Contains("Not Found"), "response message Mismatch, Expected: Not Found; Actual : " + response.get_Content());
        }

        /// <summary>
        /// Create Gist comment to an existing Gist without passing the request body i.e., Mandatory field valdiation error should be displayed in the response.
        /// </summary>
        [Test]
        public void CreateGistCommentWithoutBody()
        {
            string URI = ConfigurationManager.AppSettings["URI"];
            string resource = ConfigurationManager.AppSettings["commentsResource"];
            API.APIFunctions apiFunctions = new API.APIFunctions();
            RestResponse response = apiFunctions.APIResponse("POST", URI, resource, ConfigurationManager.AppSettings["BearerToken"], "");
            Console.WriteLine("response content : " + response.get_Content());
            Console.WriteLine("response Status : " + response.get_StatusCode());
            Assert.True(response.get_StatusCode().ToString() == "422" ,"Status Code mismatch");
            Assert.True(response.get_Content().Contains("Validation Failed"), "response message Mismatch, Expected: Validation Failed; Actual : " + response.get_Content());
        }
    }
}
