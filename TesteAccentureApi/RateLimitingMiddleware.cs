using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TesteAccentureApi
{
    internal static class RateLimitingMiddleware
    {
        internal static IServiceCollection AddRateLimiting(this IServiceCollection services, IConfiguration configuration)
        {
            // Used to store rate limit counters and ip rules
            services.AddMemoryCache();

            // Load in general configuration from appsettings.json
            services.Configure<IpRateLimitOptions>(options => configuration.GetSection("IpRateLimitingSettings").Bind(options));

            // Inject Counter and Store Rules
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddInMemoryRateLimiting();

            // Return the services
            return services;
        }

        internal static IApplicationBuilder UseRateLimiting(this IApplicationBuilder app)
        {
            app.UseIpRateLimiting();
            return app;
        }
    }
}