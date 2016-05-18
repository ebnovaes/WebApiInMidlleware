using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AppClientWebApi
{
    class Program
    {
        static void Main(string[] args)
        {

            Run().Wait();

            Console.WriteLine("");
            Console.WriteLine("Done! Press the Enter key to Exit...");
            Console.ReadLine();
            return;
        }

        static async Task Run()
        {   

            string hostUriString = "http://localhost:83";
            ApiClientProvider provider = new ApiClientProvider(hostUriString);
            string accessToken;
            Dictionary<string, string> tokenDictionary;

            try
            {
                tokenDictionary = await provider.GetTokenDictionary(
                            "john@Example.com", "Johnassword");
                accessToken = tokenDictionary["access_token"];

                foreach (KeyValuePair<string, string> kvp in tokenDictionary)
                {
                    Console.WriteLine("{0}: {1}", kvp.Key, kvp.Value);
                }

                Uri baseUri = new Uri(hostUriString);
                CompanyClient companyClient = new CompanyClient(baseUri, accessToken);

                Console.WriteLine("Read all the companies...");
                IEnumerable<Company> companies = await companyClient.GetCompaniesAsync();
                WriteCompaniesList(companies);

                int nextId = (from c in companies select c.Id).Max() + 1;

                Console.WriteLine("Add a new company...");
                HttpStatusCode result = await companyClient.AddCompanyAsync(new Company { Name = string.Format("New company #{0}", nextId) });
                WriteStatusCodeResult(result);

                Console.WriteLine("Updated List after Add:");
                companies = await companyClient.GetCompaniesAsync();
                WriteCompaniesList(companies);

                Console.WriteLine("Update a company...");
                Company updateMe = await companyClient.GetCompanyAsync(nextId);
                updateMe.Name = string.Format("Update company #{0}", updateMe.Id);
                result = await companyClient.UpdateCompanyAsync(updateMe);
                WriteStatusCodeResult(result);

                Console.WriteLine("Updated List after Update:");
                companies = await companyClient.GetCompaniesAsync();
                WriteCompaniesList(companies);

                Console.WriteLine("Delete a company...");
                result = await companyClient.DeleteCompanyAsync(nextId - 1);
                WriteStatusCodeResult(result);

                Console.WriteLine("Updated List after Delete:");
                companies = await companyClient.GetCompaniesAsync();
                WriteCompaniesList(companies);

            }
            catch (AggregateException ex)
            {
                Console.WriteLine(ex.InnerExceptions[0].Message);
                Console.WriteLine("Press the Enter key to Exit...");
                Console.ReadLine();
                return;
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press the Enter key to Exit...");
                Console.ReadLine();
                return;
            }


            Console.Read();
        }

        private static void WriteCompaniesList(IEnumerable<Company> companies)
        {
            foreach (Company company in companies)
            {
                Console.WriteLine("Id: {0} Name: {1}", company.Id, company.Name);
            }
            Console.WriteLine();
        }

        private static void WriteStatusCodeResult(HttpStatusCode statusCode)
        {
            if (statusCode == HttpStatusCode.OK)
            {
                Console.WriteLine("Operation Succeeded - status code {0}", statusCode);
            }
            else
            {
                Console.WriteLine("Operation Failed - status code {0}", statusCode);
            }
            Console.WriteLine("");
        }

    }
}
