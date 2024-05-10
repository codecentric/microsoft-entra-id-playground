using Microsoft.Identity.Web;

namespace CiService.DownstreamApi.CodeRepository;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCodeRepositoryApi(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<CodeRepositoryApi>();

        var downstreamApiConfiguration = configuration
            .GetRequiredSection(DownstreamApiConstants.DownsteamApiConfigurationKey)
            .GetRequiredSection(CodeRepositoryApi.DownstreamApiName);

        return services.AddDownstreamApi(CodeRepositoryApi.DownstreamApiName,
            options => { downstreamApiConfiguration.Bind(options); });
    }
}