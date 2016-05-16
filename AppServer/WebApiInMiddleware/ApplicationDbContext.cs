using System;
using System.Data.Entity;
using System.Security.Claims;

namespace WebApiInMiddleware.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("MyDatabase")
        {
        }

        static ApplicationDbContext()
        {
            Database.SetInitializer(new ApplicationDbInitializer());
        }

        public IDbSet<Company> Companies { get; set; }
        public IDbSet<MyUser> Users { get; set; }
        public IDbSet<MyUserClaim> Claims { get; set; }

    }


    public class ApplicationDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected async override void Seed(ApplicationDbContext context)
        {
            base.Seed(context);
            context.Companies.Add(new Company { Name = "Microsoft" });
            context.Companies.Add(new Company { Name = "Google" });
            context.Companies.Add(new Company { Name = "Apple" });

            MyUser john = new MyUser { Email = "john@example.com" };
            MyUser jimi = new MyUser { Email = "jimi@Example.com" };

            john.Claims.Add(new MyUserClaim
            {
                ClaimType = ClaimTypes.Name,
                UserId = john.Id,
                ClaimValue = john.Email
            });
            john.Claims.Add(new MyUserClaim
            {
                ClaimType = ClaimTypes.Role,
                UserId = john.Id,
                ClaimValue = "Admin"
            });

            jimi.Claims.Add(new MyUserClaim
            {
                ClaimType = ClaimTypes.Name,
                UserId = jimi.Id,
                ClaimValue = jimi.Email
            });
            john.Claims.Add(new MyUserClaim
            {
                ClaimType = ClaimTypes.Role,
                UserId = jimi.Id,
                ClaimValue = "User"
            });

            var store = new MyUserStore(context);
            await store.AddUserSync(john, "JohnsPassword");
            await store.AddUserSync(jimi, "JimisPassword");
        }
    }
}
