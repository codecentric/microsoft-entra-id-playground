using CodeRepositoryService.Api.Repositories;
using CodeRepositoryService.Api.Repositories.CodeApi;
using CodeRepositoryService.Extensions;
using CodeRepositoryService.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options => { options.LowercaseUrls = true; });

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration);

builder.Services.AddRequiredScopeOrAppPermissionAuthorization();

builder.Services.AddSingleton<InMemoryRepositoryRepository>();
builder.Services.AddSingleton<InMemoryCodeRepository>();

builder.Services.AddControllers();

builder.Services.AddCorsFromConfiguration();

builder.Services.AddSwaggerGenWithBearerAuth();

var app = builder.Build();

app.UseLogAuthorizationHeader();

app.UseCorsFromConfiguration();

app.UseSwagger();

app.UseAuthorization();

app.MapControllers();

app.Run();