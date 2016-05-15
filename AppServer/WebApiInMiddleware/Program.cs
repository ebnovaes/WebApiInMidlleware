using System;
using Microsoft.Owin.Hosting;

// Add reference to:
using System.Data.Entity;
using WebApiInMiddleware.Models;
using System.Linq;

namespace WebApiInMiddleware
{
    class Program
    {
        static void Main(string[] args)
        {

            // Set up and seed the database:
            Console.WriteLine("Initializing and seeding database...");
            Database.SetInitializer(new ApplicationDbInitializer());
            var db = new ApplicationDbContext();
            int count = db.Companies.Count();
            Console.WriteLine("Database created and seeded with {0} company records...", count);

            string baseUri = "http://localhost:83";

            Console.WriteLine("Starting web Server...");
            WebApp.Start<Startup>(baseUri);
            Console.WriteLine("Server running at {0} - press Enter to quit. ", baseUri);
            Console.ReadLine();
        }
    }
}
