using Paramore.Brighter.Extensions.DependencyInjection;
using System.Reflection;

namespace Customers.Api.Config;

public static class BrighterConfig
{
    public static IServiceCollection AddBrighterConfig(this IServiceCollection services, Assembly[] assemblies)
    {
        services.AddBrighter().AutoFromAssemblies();

        return services;
    }
}