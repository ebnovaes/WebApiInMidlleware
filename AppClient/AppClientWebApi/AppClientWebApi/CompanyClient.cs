using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AppClientWebApi
{
    public class CompanyClient
    {
        string _accessToken;
        Uri _baseRequestUri;

        public CompanyClient(Uri baseUri, string accessToken)
        {
            _accessToken = accessToken;
            _baseRequestUri = new Uri(baseUri, "api/companies/");
        }

        void SetClientAuthentication(HttpClient client)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        }

        public async Task<IEnumerable<Company>> GetCompaniesAsync()
        {
            HttpResponseMessage response;
            using (HttpClient client = new HttpClient())
            {
                SetClientAuthentication(client);
                response = await client.GetAsync(_baseRequestUri);
            }
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(string.Format(
                    "API Error: Status Code: {0}", response.StatusCode));
            }

            return await response.Content.ReadAsAsync<IEnumerable<Company>>();        
        }

        public async Task<Company> GetCompanyAsync(int id)
        {
            HttpResponseMessage response;
            using (HttpClient client = new HttpClient())
            {
                SetClientAuthentication(client);
                response = await client.GetAsync(new Uri(_baseRequestUri, id.ToString()));
            }
            return await response.Content.ReadAsAsync<Company>();
        }

        public async Task<HttpStatusCode> AddCompanyAsync(Company company)
        {
            HttpResponseMessage response;
            using (HttpClient client = new HttpClient())
            {
                SetClientAuthentication(client);
                response = await client.PostAsJsonAsync(_baseRequestUri, company);
            }

            return response.StatusCode;
        }

        public async Task<HttpStatusCode> UpdateCompanyAsync(Company company)
        {
            HttpResponseMessage response;
            using (HttpClient client = new HttpClient())
            {
                SetClientAuthentication(client);
                response = await client.PutAsJsonAsync(_baseRequestUri, company);
            }

            return response.StatusCode;
        }

        public async Task<HttpStatusCode> DeleteCompanyAsync(int id)
        {
            HttpResponseMessage response;
            using (HttpClient client = new HttpClient())
            {
                SetClientAuthentication(client);
                response = await client.DeleteAsync(new Uri(_baseRequestUri, id.ToString()));
            }

            return response.StatusCode;
        }
    }
}
