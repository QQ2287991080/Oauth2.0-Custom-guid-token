using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using Oauth.Data;
using OAuth.Provider;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace OAuth
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        static Startup()
        {
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/token"),
                Provider = new OAuthAppProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                AllowInsecureHttp = true,
                AccessTokenProvider = new AuthenticationTokenProvider
                {
                    OnCreate = (context) =>
                    {
                      
                        using (OauthConnection db=new OauthConnection())
                        {
                            var token = context.SerializeTicket();
                            var guid = Guid.NewGuid().ToString();
                            Auth_Token auth_Token = new Auth_Token
                            {
                                Guid = guid,
                                Token = token,
                                StartTime = DateTime.Now,
                                EndTime = DateTime.Now.AddDays(14),
                                UserName = context.Ticket.Identity.Name
                            };
                            db.Auth_Token.Add(auth_Token);
                            db.SaveChanges();
                            context.SetToken(guid);
                        }
                        //HttpRuntime.Cache.Insert(guid, token, null, DateTime.Now.AddDays(14), Cache.NoSlidingExpiration);
                    },
                    OnReceive = (context) =>
                    {
                        using (OauthConnection db = new OauthConnection())
                        {
                            string guid = context.Token;
                            var info= db.Auth_Token.FirstOrDefault(p => p.Guid == guid);
                            if (info==null) return;
                            else
                            {
                                if (info.EndTime<DateTime.Now||info.OutTime!=null) return;
                                else context.DeserializeTicket(info.Token);
                            }
                        }
                    }
                }
            };
        }

        //OAuthBearerAuthenticationOptions bearerOptions = new OAuthBearerAuthenticationOptions()
        //{
        //    AccessTokenProvider = new YourCustomTokenProvider() // YourCustomTokenProvider implements IAuthenticationTokenProvider 
        //};
        private void ConfigureAuth(IAppBuilder app)
        {
            app.UseOAuthBearerTokens(OAuthOptions);
            //跨域问题
            //app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);//Microsoft.Owin.Cors;
        }
    }
}