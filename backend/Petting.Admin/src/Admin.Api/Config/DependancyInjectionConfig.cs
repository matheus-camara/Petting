using System.Reflection;
using Admin.Domain.Config;
using ApiActions;
using MediatR;
using Notify;

namespace Admin.Api.Configuration;

public static class DependancyInjectionConfig
{
    private static readonly Assembly _assembly = Assembly.GetExecutingAssembly();

    private static Assembly[] GetAssemblies()
    {
        var assemblies = _assembly.GetReferencedAssemblies().Select(x => Assembly.Load(x));
        assemblies = assemblies.Append(_assembly);
        return assemblies.ToArray();
    }

    public static void ConfigureApi(this IServiceCollection services, IConfiguration configuration)
    {
        var assemblies = GetAssemblies();
        services.AddApiActions();
        services.AddScoped<INotificationContext, NotificationContext>();
        services.AddMediatR(assemblies);
        services.AddValidators(assemblies);
        services.AddMessageBroker(configuration);
    }
}