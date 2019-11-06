using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace OAuth.Provider
{

    public class OAuthAppProvider : OAuthAuthorizationServerProvider
    {
        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            return Task.Factory.StartNew(() =>
            {
                var s = context.Request.Headers;
                //if (HttpRuntime.Cache.Get() == null)
                //{
                //    context.SetError("Token已过期");
                //}
                //else
                //{

                //}
                var username = context.UserName;
                var password = context.Password;
                ClaimsIdentity oAutIdentity = new ClaimsIdentity(Startup.OAuthOptions.AuthenticationType);
                oAutIdentity.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, username));
                AuthenticationProperties properties = CreateProperties(username);
                AuthenticationTicket ticket = new AuthenticationTicket(oAutIdentity, properties);
                //var secure = new SecureDataFormat<AuthenticationTicket>();
                //var token= secure.Protect(ticket);
                //var token = Startup.OAuthOptions.AccessTokenFormat.Protect(ticket);
                context.Validated(ticket);
                //return base.GrantResourceOwnerCredentials(context);
            });
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            if (context.ClientId == null)
            {
                context.Validated();
            }
            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }


    }
   

}