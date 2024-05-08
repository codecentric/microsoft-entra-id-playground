namespace CiService.Extensions;

public static class CorsApplicationBuilderExtensions
{
    public static IApplicationBuilder UseCorsFromConfiguration(this IApplicationBuilder applicationBuilder, string corsConfigurationKey = CorsServiceCollectionExtensions.DefaultCorsConfigurationKey)
    {
        var configuration = applicationBuilder.ApplicationServices.GetRequiredService<IConfiguration>();
        var corsConfiguration = configuration.GetRequiredSection(corsConfigurationKey);

        foreach (var policyConfiguration in corsConfiguration.GetChildren())
        {
            applicationBuilder.UseCors(policyConfiguration.Key);
        }

        return applicationBuilder;
    }
}