using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Library.Api.Configurations
{
    public static class ApiAuthentication
    {
        public static IServiceCollection AddAuthenticationConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    IConfigurationSection googleAuthNSection =
                        configuration.GetSection("Authentication:Google");

                    options.Authority = "https://accounts.google.com";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = googleAuthNSection["Issuer"],
                        ValidateAudience = true,
                        ValidAudience = googleAuthNSection["Audience"],
                        ValidateLifetime = true
                    };
                });

            return services;
        }
    }
}
