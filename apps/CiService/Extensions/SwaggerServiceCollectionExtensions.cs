using CiService.DownstreamApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Abstractions;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CiService.Extensions;

public static class SwaggerServiceCollectionExtensions
{
    public static IServiceCollection AddSwaggerGenWithBearerAuth(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddSwaggerGen()
            .Configure<SwaggerGenOptions>((services, options) =>
            {
                var authScheme = JwtBearerDefaults.AuthenticationScheme;
                using var timeoutTokenSrc = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                var jwtBearerOptions = services.GetRequiredService<IOptionsMonitor<JwtBearerOptions>>()
                    .Get(authScheme);

                if (jwtBearerOptions.ConfigurationManager == null)
                {
                    throw new ArgumentNullException($"{nameof(jwtBearerOptions)}.{jwtBearerOptions.ConfigurationManager}");
                }

                var oidcConfiguration = jwtBearerOptions.ConfigurationManager.GetConfigurationAsync(timeoutTokenSrc.Token).Result;

                var identityOptions = services.GetRequiredService<IOptionsMonitor<MicrosoftIdentityOptions>>().Get(authScheme);

                // var downstreamApiScopes = services.GetAllDownstreamApiScopes();

                var ownScope = $"{identityOptions.ClientId}/.default";
                var scopes = new Dictionary<string, string>()
                {
                    { ownScope, "Request permissions for current application" }
                };

                // foreach (var downstreamApiScope in downstreamApiScopes)
                // {
                //     scopes.Add(downstreamApiScope.Value, "Scope for " + downstreamApiScope.Key);
                // }

                var userAuthSecrityScheme = "UserAuthentication";
                options.AddSecurityDefinition(userAuthSecrityScheme, new OpenApiSecurityScheme()
                {
                    Name = userAuthSecrityScheme,
                    Description = "Use this scheme to authenticate as a user principal against Entra ID",
                    Type = SecuritySchemeType.OAuth2,
                    BearerFormat = "JWT",
                    Flows = new OpenApiOAuthFlows()
                    {
                        AuthorizationCode = new OpenApiOAuthFlow()
                        {
                            AuthorizationUrl = new Uri(oidcConfiguration.AuthorizationEndpoint),
                            TokenUrl = new Uri(oidcConfiguration.TokenEndpoint),
                            Scopes = scopes
                        }
                    }
                });

                var servicePrincipalAuthSecrityScheme = "ServicePrincipalAuthentication";
                options.AddSecurityDefinition(servicePrincipalAuthSecrityScheme, new OpenApiSecurityScheme()
                {
                    Name = servicePrincipalAuthSecrityScheme,
                    Description = $@"Use this scheme to authenticate as a service principal against Entra ID

<b>
Microsoft Entra does not support receiving client credentials from a browser!
Instead use curl for example:
</b>

curl https://login.microsoftonline.com/{identityOptions.TenantId}/oauth2/v2.0/token -d 'grant_type=client_credentials' -d 'scope={identityOptions.ClientId}/.default' -d 'client_id=__INSERT_YOUR_CLIENT_ID__' -d 'client_secret=__INSERT_YOUR_CLIENT_SECRET__'",
                    Type = SecuritySchemeType.Http,
                    Scheme = authScheme,
                    BearerFormat = "JWT"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = userAuthSecrityScheme
                            }
                        },
                        new List<string>()
                        {
                            ownScope
                        }
                    },
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = servicePrincipalAuthSecrityScheme
                            }
                        },
                        new List<string>()
                        {
                            ownScope
                        }
                    }
                });
            });
    }

    // private static IDictionary<string, string> GetAllDownstreamApiScopes(this IServiceProvider services)
    // {
    //     var configuration = services.GetRequiredService<IConfiguration>();
    //     var downstreamApiOptionsMonitor = services.GetRequiredService<IOptionsMonitor<DownstreamApiOptions>>();
    //     var downstreamApiConfigurations = configuration.GetSection(DownstreamApiConstants.DownsteamApiConfigurationKey);
    //
    //     return downstreamApiConfigurations.GetChildren()
    //         .Select(config => config.Key)
    //         .ToDictionary(
    //             downstreamApiName => downstreamApiName,
    //             downstreamApiName => downstreamApiOptionsMonitor.Get(downstreamApiName).Scopes!.First());
    // }
}