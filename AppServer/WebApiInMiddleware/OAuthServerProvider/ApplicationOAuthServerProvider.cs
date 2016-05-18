using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.Owin.Security.OAuth;
using WebApiInMiddleware.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebApiInMiddleware.OAuthServerProvider
{
    public class ApplicationOAuthServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            await Task.FromResult(context.Validated());
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            ApplicationUserManager manager = context.OwinContext.GetUserManager<ApplicationUserManager>();

            ApplicationUser user = await manager.FindAsync(context.UserName, context.Password);
            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                context.Rejected();
                return;
            }

            ClaimsIdentity identity = new ClaimsIdentity(context.Options.AuthenticationType);
            foreach (IdentityUserClaim userClaim in user.Claims)
            {
                identity.AddClaim(new Claim(userClaim.ClaimType, userClaim.ClaimValue));
            }
           
            context.Validated(identity);
        }
    }
}
