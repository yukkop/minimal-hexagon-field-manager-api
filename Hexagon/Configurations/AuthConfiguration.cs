using System.Text;
using Hexagon.Database.HexagonDb;
using Hexagon.Database.HexagonDb.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Hexagon.Configurations;

public static class AuthConfiguration
{
    public static void RegisterAuth(this IServiceCollection services, IConfiguration config)
        {

            services.AddIdentity<User, Role>(o =>
            {
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
            })
               .AddEntityFrameworkStores<HexagonContext>()
               .AddDefaultTokenProviders();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(x =>
           {
               x.RequireHttpsMetadata = false;
               x.SaveToken = true;
               x.ClaimsIssuer = config.GetValue<string>("Jwt:Issuer");
               x.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config.GetValue<string>("Jwt:Key"))),
                   ValidateIssuer = false,
                   ValidIssuer = config.GetValue<string>("Jwt:Issuer"),
                   ValidateAudience = false,
                   ValidAudience = config.GetValue<string>("Jwt:Audience"),
                   ValidateLifetime = true,
                   RequireExpirationTime = true,
                   ClockSkew = TimeSpan.Zero
               };
           });
        }

        public static void RegisterAuth(this IApplicationBuilder app)
        {
            app.UseAuthentication();
        }
}