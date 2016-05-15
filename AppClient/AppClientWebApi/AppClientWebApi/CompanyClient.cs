using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace AppClientWebApi
{
    public class CompanyClient
    {
        string _hostUri;

        public CompanyClient(string hostUri)
        {
            _hostUri = hostUri;
        }

        public HttpClient CreateClient(){
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(new Uri(_hostUri), "api/companies/");
            return client;
        }

        public IEnumerable<Company> GetCompanies()
        {
            HttpResponseMessage response;
            using (HttpClient client = CreateClient())
            {
                response = client.GetAsync(client.BaseAddress).Result;
            }

            IEnumerable<Company> result = response.Content.ReadAsAsync<IEnumerable<Company>>().Result;
            return result;
        }

        public Company GetCompany(int id)
        {
            HttpResponseMessage response;
            using (HttpClient client = CreateClient())
            {
                response = client.GetAsync(new Uri(client.BaseAddress, id.ToString())).Result;
            }
            Company result = response.Content.ReadAsAsync<Company>().Result;
            return result;
        }

        public HttpStatusCode AddCompany(Company company)
        {
            HttpResponseMessage response;
            using (HttpClient client = CreateClient())
            {
                response = client.PostAsJsonAsync(client.BaseAddress, company).Result;
            }

            return response.StatusCode;
        }

        public HttpStatusCode UpdateCompany(Company company)
        {
            HttpResponseMessage response;
            using (HttpClient client = CreateClient())
            {
                response = client.PutAsJsonAsync(client.BaseAddress, company).Result;
            }

            return response.StatusCode;
        }

        public HttpStatusCode DeleteCompany(int id)
        {
            HttpResponseMessage response;
            using (HttpClient client = CreateClient())
            {
                response = client.DeleteAsync(new Uri(client.BaseAddress, id.ToString())).Result;
            }

            return response.StatusCode;
        }
    }
}
