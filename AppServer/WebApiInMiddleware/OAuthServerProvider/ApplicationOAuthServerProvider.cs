using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.Owin.Security.OAuth;
using WebApiInMiddleware.Models;

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
            MyUserStore store = new MyUserStore(new ApplicationDbContext());
            AuthModel user = await store.FindByEmailAsync(context.UserName);

            if (user == null || !store.PasswordIsValid(user, context.Password))
            {
                context.SetError("Invalid grant", "The user name or password is incorrect.");
                context.Rejected();
                return;
            }

            ClaimsIdentity identity = new ClaimsIdentity(context.Options.AuthenticationType);

            foreach (MyUserClaim claim in user.Claims)
            {
                identity.AddClaim(new Claim(claim.ClaimType, claim.ClaimValue));
            }
            
            context.Validated(identity);
        }
    }
}
