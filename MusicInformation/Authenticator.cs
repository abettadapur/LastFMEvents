using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;

namespace MusicInformation
{
    public class Authenticator
    {
        public static string sessionToken { get; set; }
        public const string tokenUrl = "http://ws.audioscrobbler.com/2.0/?method=auth.gettoken&api_key=" + API_KEY + "&format=json";
        public const string authUrl = "https://www.last.fm/api/auth/?api_key=" + API_KEY;
        private const string ApplicationSecret = "9479db0225e720fa09029d30fe9f6ed9";
        private const string API_KEY = "08dd2c001a8176908e4feb6bd51391a1";


        public static async Task<bool> authenticateUser()
        {
            string token = await getToken();
            Uri StartUri = new Uri(authUrl+"&token="+token, UriKind.RelativeOrAbsolute);
            Uri EndUri = new Uri("127.0.0.1", UriKind.RelativeOrAbsolute);

            WebAuthenticationResult WebAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, StartUri);
            if (WebAuthenticationResult.ResponseStatus == WebAuthenticationStatus.Success)
            {
                StringBuilder parameterBuilder = new StringBuilder();
                parameterBuilder.AppendFormat("{0}={1}&", "api_key", WebUtility.UrlEncode(API_KEY));
                parameterBuilder.AppendFormat("{0}={1}&", "token", WebUtility.UrlEncode(token));
                parameterBuilder.AppendFormat("{0}={1}", "api_sig", WebUtility.UrlEncode(ApplicationSecret));
                var content = new StringContent(parameterBuilder.ToString());
                var client = new HttpClient();
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");
                var responseTask = client.PostAsync("https://www.last.fm/api/auth.getSession", content);
                var response = await responseTask;
                string jsonresponse = await response.Content.ReadAsStringAsync();
                return true;
                


            }
            return false;
          
        }




        public static async Task<string> getToken()
        {
            WebRequest request = WebRequest.Create(tokenUrl);
            WebResponse response = await request.GetResponseAsync();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string json = reader.ReadToEnd();
            string token = JsonConvert.DeserializeObject<TokenWrapper>(json).token;
            return token;

        }
    }
    public class TokenWrapper
    {
        public string token { get; set; }
    }
}
