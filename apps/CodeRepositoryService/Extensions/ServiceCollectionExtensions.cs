using Microsoft.Extensions.Options;

namespace CodeRepositoryService.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection Configure<TOptions>(
        this IServiceCollection serviceCollection,
        Action<IServiceProvider, TOptions> configure) where TOptions : class
    {
        return serviceCollection.AddSingleton<IConfigureOptions<TOptions>>(services =>
            new ConfigureNamedOptions<TOptions>(Options.DefaultName, options => { configure(services, options); }));
    }
}