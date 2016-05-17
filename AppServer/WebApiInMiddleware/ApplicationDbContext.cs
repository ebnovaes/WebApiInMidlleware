using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Security.Claims;

namespace WebApiInMiddleware.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUserManager>
    {
        public ApplicationDbContext() : base("MyDatabase")
        {
        }

        static ApplicationDbContext()
        {
            Database.SetInitializer(new ApplicationDbInitializer());
        }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }


        public IDbSet<Company> Companies { get; set; }

    }

    public class ApplicationDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected async override void Seed(ApplicationDbContext context)
        {
            context.Companies.Add(new Company { Name = "Microsoft" });
            context.Companies.Add(new Company { Name = "Google" });
            context.Companies.Add(new Company { Name = "Apple" });
            context.SaveChanges();

            ApplicationUserManager john = new ApplicationUserManager {
                Email = "john@Example.com",
                UserName = "john@Example.com"
            };
            ApplicationUserManager jimi = new ApplicationUserManager
            {
                Email = "jimi@Example.com",
                UserName = "jimi@Example.com"
            };

            UserManager<ApplicationUserManager> manager = new UserManager<ApplicationUserManager>(
                new UserStore<ApplicationUserManager>(context));

            IdentityResult result1 = await manager.CreateAsync(john, "JohnsPassword");
            IdentityResult result2 = await manager.CreateAsync(john, "JimisPassword");

            await manager.AddClaimAsync(john.Id, new Claim(ClaimTypes.Name, "john@example.com"));
            await manager.AddClaimAsync(john.Id, new Claim(ClaimTypes.Role, "Admin"));

            await manager.AddClaimAsync(john.Id, new Claim(ClaimTypes.Name, "jimi@example.com"));
            await manager.AddClaimAsync(john.Id, new Claim(ClaimTypes.Role, "user"));
        }
    }
}
