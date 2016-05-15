using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace AppClientWebApi
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Read all the companies...");
            CompanyClient companyClient = new CompanyClient("http://localhost:83");
            IEnumerable<Company> companies = companyClient.GetCompanies();
            WriteCompaniesList(companies);

            int nextId = (from c in companies select c.Id).Max() + 1;

            Console.WriteLine("Add a new company...");
            HttpStatusCode result = companyClient.AddCompany(
                new Company
                {
                    Name = string.Format("New Company #{0}", nextId)
                });
            WriteStatusCodeResult(result);

            Console.WriteLine("Updated List after Add:");
            companies = companyClient.GetCompanies();
            WriteCompaniesList(companies);

            Console.WriteLine("Update a company...");
            Company updateMe = companyClient.GetCompany(nextId);
            updateMe.Name = string.Format("Updated company {0}", updateMe.Id);
            result = companyClient.UpdateCompany(updateMe);
            WriteStatusCodeResult(result);

            Console.WriteLine("Updated List after Add:");
            companies = companyClient.GetCompanies();
            WriteCompaniesList(companies);

            Console.WriteLine("Delete a company...");
            result = companyClient.DeleteCompany(nextId - 1);
            WriteStatusCodeResult(result);

            Console.WriteLine("Updated List after Add:");
            companies = companyClient.GetCompanies();
            WriteCompaniesList(companies);

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
