using Microsoft.Extensions.DependencyInjection;

namespace Library.Api.Configurations;

public static class ApiCorsConfiguration
{
    /// <summary>
    /// Adding CORS configuration to <see cref="IServiceCollection"/>
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddCorsConfig(this IServiceCollection services)
    {
        services.AddCors(setup =>
        {
            setup.AddPolicy("CorsPolicy", policy =>
            {
                policy
                    .WithOrigins("http://localhost:8080")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .SetIsOriginAllowedToAllowWildcardSubdomains();
            });
        });

        return services;
    }
}
