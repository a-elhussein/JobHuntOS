using System.Text;
using JobHuntOS.API.Services;
using JobHuntOS.Infrastructure.Data;
using JobHuntOS.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace JobHuntOS.API.Extensions;

public static class AuthExtensions
{
    public static IServiceCollection AddAuthServices(
        this IServiceCollection services, IConfiguration config)
    {
        services.AddIdentityCore<AppUser>(o =>
            {
                o.Password.RequiredLength = 8;
                o.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<AppDbContext>();

        var key = config["Jwt:Key"]
                  ?? throw new InvalidOperationException("Jwt:Key is missing");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config["Jwt:Issuer"],
                    ValidAudience = config["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    ClockSkew = TimeSpan.Zero
                };
            });

        services.AddScoped<ITokenService, TokenService>();
        return services;
    }
}