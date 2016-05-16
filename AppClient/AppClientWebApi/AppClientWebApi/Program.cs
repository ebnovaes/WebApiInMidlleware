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
                            "john@example.com", "assword");
                accessToken = tokenDictionary["access_token"];
            }catch (AggregateException ex)
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

            foreach (KeyValuePair<string, string> kvp in tokenDictionary)
            {
                Console.WriteLine("{0}: {1}", kvp.Key, kvp.Value);
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
