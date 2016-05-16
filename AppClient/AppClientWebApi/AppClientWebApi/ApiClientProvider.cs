using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AppClientWebApi
{
    public class ApiClientProvider
    {
        string _hostUri;
        public string AccessToken { get; private set; }

        public ApiClientProvider(string hostUri)
        {
            _hostUri = hostUri;
        }

        public async Task<Dictionary<string, string>> GetTokenDictionary(string userName, string password)
        {
            HttpResponseMessage response;
            List<KeyValuePair<string, string>> pairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", userName),
                new KeyValuePair<string, string>("password", password)
            };
            FormUrlEncodedContent content = new FormUrlEncodedContent(pairs);

            using (HttpClient client = new HttpClient())
            {
                Uri tokenEndPoint = new Uri(new Uri(_hostUri), "Token");
                response = await client.PostAsync(tokenEndPoint, content);
            }

            string responseContent = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(string.Format("Error: {0}", responseContent));
            }

            return GetTokenDictionary(responseContent);
        }

        private Dictionary<string, string> GetTokenDictionary(string responseContent)
        {
            Dictionary<string, string> tokenDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);
            return tokenDictionary;
        }
    }
}
