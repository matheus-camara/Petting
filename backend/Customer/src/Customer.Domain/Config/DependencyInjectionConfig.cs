using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Customer.Domain.Config;

public static class DependencyInjectionConfig
{
    public static void AddValidators(this IServiceCollection services, Assembly[] assemblies)
    {
        AssemblyScanner.FindValidatorsInAssemblies(assemblies)
            .ForEach(x => { services.AddScoped(x.InterfaceType, x.ValidatorType); });
    }
}