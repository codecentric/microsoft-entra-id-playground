using Microsoft.Identity.Web;

namespace CiService.DownstreamApi.CodeRepository;

public static class MicrosoftIdentityAppCallsWebApiAuthenticationBuilderExtensions
{
    public static MicrosoftIdentityAppCallsWebApiAuthenticationBuilder AddCodeRepositoryApi(
        this MicrosoftIdentityAppCallsWebApiAuthenticationBuilder builder,
        IConfiguration configuration)
    {
        builder.Services.AddScoped<CodeRepositoryApi>();

        var downstreamApiConfiguration = configuration
            .GetRequiredSection(DownstreamApiConstants.DownsteamApiConfigurationKey)
            .GetRequiredSection(CodeRepositoryApi.DownstreamApiName);

        return builder.AddDownstreamApi(CodeRepositoryApi.DownstreamApiName,
            options => { downstreamApiConfiguration.Bind(options); });
    }
}