using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace WebApiInMiddleware.Models
{ 
    public class ApplicationUserManager : UserManager<ApplicationUserManager>
    {
        public ApplicationUserManager(IUserStore<ApplicationUserManager> store) : base(store) { }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            return new ApplicationUserManager(new UserStore<ApplicationUserManager>(context.Get<ApplicationDbContext>()));
        }
    }

}