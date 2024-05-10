using CiService.Api.Jobs;
using CiService.DownstreamApi.CodeRepository;
using CiService.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options => { options.LowercaseUrls = true; });

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    // Configure issuer and audience validation from AzureAd configuration key
    .AddMicrosoftIdentityWebApi(builder.Configuration)
    // Configure IAuthorizationHeaderProvider with support for Entra ID authentication
    // This is needed to be able to use the DownstreamApi
    .EnableTokenAcquisitionToCallDownstreamApi()
    .AddInMemoryTokenCaches();

// Add CodeRepositoryApi service and configure DownStreamApi from configuration
builder.Services.AddCodeRepositoryApi(builder.Configuration);

// This enables availablility of RequiredScopeOrAppPermissionAttribute on controller actions
builder.Services.AddRequiredScopeOrAppPermissionAuthorization();

builder.Services.AddSingleton<InMemoryJobRepository>();

builder.Services.AddControllers();

// custom extension to load cors policies from configuration
builder.Services.AddCorsFromConfiguration();

// custom extension to configure swagger in respect to Entra ID authentication
builder.Services.AddSwaggerGenWithMicrosoftIdentityAuth();

var app = builder.Build();

// custom extension to use policies, which where configured above
app.UseCorsFromConfiguration();

app.UseSwagger();

app.UseAuthorization();

app.MapControllers();

app.Run();
