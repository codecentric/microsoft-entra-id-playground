using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CodeRepositoryService.Extensions;

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
                    throw new ArgumentNullException(
                        $"{nameof(jwtBearerOptions)}.{jwtBearerOptions.ConfigurationManager}");
                }

                var oidcConfiguration = jwtBearerOptions.ConfigurationManager
                    .GetConfigurationAsync(timeoutTokenSrc.Token).Result;

                var identityOptions = services.GetRequiredService<IOptionsMonitor<MicrosoftIdentityOptions>>()
                    .Get(authScheme);

                var ownScope = $"{identityOptions.ClientId}/.default";
                var scopes = new Dictionary<string, string>()
                {
                    { ownScope, "Request permissions for current application" }
                };

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
                    Description = $@"Use this scheme to authenticate using any bearer token against Entra ID

<b>
Microsoft Entra does not support receiving client credentials from a browser!
Instead use curl for example:
</b>

curl https://login.microsoftonline.com/{identityOptions.TenantId}/oauth2/v2.0/token -d 'grant_type=client_credentials' -d 'scope={identityOptions.ClientId}/.default' -d 'client_id={identityOptions.ClientId}' -d 'client_secret=__INSERT_YOUR_CLIENT_SECRET__'

<b>Or use the following script to get a ci-service service principal token for the code-repository application</b>

ci-service.client_credentials.for.code-repository-service.sh

<b>Or use a user token, obtained by an <i>on-behalf-of flow</i>, using the following script:</b>

ci-service.device-code.and.on-behalf-of.flow.for.code-repository-service.sh
",
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
}