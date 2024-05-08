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

builder.Services.AddRequiredScopeOrAppPermissionAuthorization();

builder.Services.AddSingleton<InMemoryJobRepository>();

builder.Services.AddControllers();

builder.Services.AddCorsFromConfiguration();

builder.Services.AddSwaggerGenWithBearerAuth();

var app = builder.Build();

app.UseCorsFromConfiguration();

app.UseSwagger();

app.UseAuthorization();

app.MapControllers();

app.Run();