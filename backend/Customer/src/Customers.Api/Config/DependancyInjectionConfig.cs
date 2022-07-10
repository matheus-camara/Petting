using ApiActions;
using Customers.Domain.Config;
using Customers.Infra.Config;
using Customers.Localization.Config;
using EntityAbstractions.Persistence.Filters;
using Notify;
using System.Reflection;

namespace Customers.Api.Config;

public static class DependancyInjectionConfig
{
    private static readonly Assembly _assembly = Assembly.GetExecutingAssembly();

    private static Assembly[] GetAssemblies()
    {
        var assemblies = _assembly.GetReferencedAssemblies()
                            .Where(x => x.FullName.StartsWith("Customers"))
                            .Select(x => Assembly.Load(x));
        
        assemblies = assemblies.Append(_assembly);
        return assemblies.ToArray();
    }

    public static void ConfigureApi(this IServiceCollection services, IConfiguration configuration)
    {
        var assemblies = GetAssemblies();
        services
            .AddLocalizationConfig()
            .AddValidators(assemblies)
            .AddScoped<INotificationContext, NotificationContext>()
            .AddRepositories(configuration)
            .AddMappers()
            .AddBrighterConfig(assemblies)
            .AddMvc(options =>
            {
                options.AddApiActions();
                options.Filters.Add(typeof(TransactionFilter));
            });
    }
}