using CodeRepositoryService.Api.Repositories;
using CodeRepositoryService.Api.Repositories.CodeApi;
using CodeRepositoryService.Extensions;
using CodeRepositoryService.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options => { options.LowercaseUrls = true; });

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    // Configure issuer and audience validation from AzureAd configuration key 
    .AddMicrosoftIdentityWebApi(builder.Configuration);

// This enables availablility of RequiredScopeOrAppPermissionAttribute on controller actions
builder.Services.AddRequiredScopeOrAppPermissionAuthorization();

builder.Services.AddSingleton<InMemoryRepositoryRepository>();
builder.Services.AddSingleton<InMemoryCodeRepository>();

builder.Services.AddControllers();

// custom extension to load cors policies from configuration
builder.Services.AddCorsFromConfiguration();

// custom extension to configure swagger in respect to Entra ID authentication
builder.Services.AddSwaggerGenWithBearerAuth();

var app = builder.Build();

// custom extension to log Authorization header as trace log level. Only for demo purposes!
app.UseLogAuthorizationHeader();

// custom extension to use policies, which where configured above
app.UseCorsFromConfiguration();

app.UseSwagger();

app.UseAuthorization();

app.MapControllers();

app.Run();