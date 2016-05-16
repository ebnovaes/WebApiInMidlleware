using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.Owin.Security.OAuth;

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
            if (context.Password != "password")
            {
                context.SetError("Invalid grant", "The user name or password is incorrect.");
                context.Rejected();
                return;
            }

            ClaimsIdentity identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("user_name", context.UserName));
           // identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));

            context.Validated(identity);
        }
    }
}
