using System;
using System.Data.Entity;

namespace WebApiInMiddleware.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("MyDatabase")
        {
        }
        public IDbSet<Company> Companies { get; set; }

        internal Company FirstOrDefault(Func<object, bool> p)
        {
            throw new NotImplementedException();
        }
    }


    public class ApplicationDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            base.Seed(context);
            context.Companies.Add(new Company { Name = "Microsoft" });
            context.Companies.Add(new Company { Name = "Google" });
            context.Companies.Add(new Company { Name = "Apple" });
        }
    }
}
