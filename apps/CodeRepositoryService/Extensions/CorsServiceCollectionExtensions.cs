using Microsoft.AspNetCore.Cors.Infrastructure;

namespace CodeRepositoryService.Extensions;

public static class CorsServiceCollectionExtensions
{
    public const string DefaultCorsConfigurationKey = "Cors";

    public static IServiceCollection AddCorsFromConfiguration(this IServiceCollection serviceCollection, string configurationKey = DefaultCorsConfigurationKey)
    {
        return serviceCollection
            .AddCors()
            .Configure<CorsOptions>((services, options) =>
            {
                var configuration = services.GetRequiredService<IConfiguration>();
                var corsConfiguration = configuration.GetRequiredSection(configurationKey);

                foreach (var policyConfiguration in corsConfiguration.GetChildren())
                {
                    var corsPolicy = new CorsPolicy();
                    policyConfiguration.Bind(corsPolicy);
                    options.AddPolicy(policyConfiguration.Key, corsPolicy);
                }
            });
    }
}