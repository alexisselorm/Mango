using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Mango.Services.CouponAPI.Extensions
;
public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddAppAuthentication(this WebApplicationBuilder builder)
    {

        var secret = builder.Configuration["ApiSettings:Secret"];
        var issuer = builder.Configuration["ApiSettings:Issuer"];
        var audience = builder.Configuration["ApiSettings:Audience"];

        var key = Encoding.UTF8.GetBytes(secret);

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
         .AddJwtBearer(x =>
         {
             x.TokenValidationParameters = new TokenValidationParameters
             {
                 ValidateIssuerSigningKey = true,
                 IssuerSigningKey = new SymmetricSecurityKey(key),
                 ValidateIssuer = true,
                 ValidateAudience = true,
                 ValidIssuer = issuer,
                 ValidAudience = audience,
             };
         });


        return builder;

    }
}
