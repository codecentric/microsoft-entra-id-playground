using CiService.Api.Jobs;
using CiService.DownstreamApi.CodeRepository;
using CiService.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options => { options.LowercaseUrls = true; });

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration)
    .EnableTokenAcquisitionToCallDownstreamApi()
    .AddCodeRepositoryApi(builder.Configuration)
    .AddInMemoryTokenCaches();

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