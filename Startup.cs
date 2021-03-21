using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ASP_API.Common;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Jwt;
using Owin;

[assembly: OwinStartup("ProductionConfiguration", typeof(ArsipAPI.Startup))]

namespace ArsipAPI
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
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateIssuerSigningKey = true,
                       ValidIssuer = "http://mysite.com", //some string, normally web url,
                       ValidAudience = "http://mysite.com",
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(new Config().jwtKey()))
                   }
               });
        }
    }
}
