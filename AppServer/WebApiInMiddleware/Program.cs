using System;
using Microsoft.Owin.Hosting;

namespace WebApiInMiddleware
{
    class Program
    {
        static void Main(string[] args)
        {

            string baseUri = "http://localhost:83";

            Console.WriteLine("Starting web Server...");
            WebApp.Start<Startup>(baseUri);
            Console.WriteLine("Server running at {0} - press Enter to quit. ", baseUri);
            Console.ReadLine();
        }
    }
}
