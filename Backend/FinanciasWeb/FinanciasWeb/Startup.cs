using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Jwt;
using Owin;
using System;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(FinanciasWeb.Startup))]

namespace FinanciasWeb
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "http://192.168.0.121:9011/", //some string, normally web url,  
                        ValidAudience = "http://192.168.0.121:9011/",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["Token"]))
                    }
                });
        }
    }
}
