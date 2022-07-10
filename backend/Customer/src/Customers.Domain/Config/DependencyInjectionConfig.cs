using Customers.Domain.Commands.Add;
using Customers.Domain.Entities;
using Customers.Domain.Mappers;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Customers.Domain.Config;

public static class DependencyInjectionConfig
{
    public static IServiceCollection AddValidators(this IServiceCollection services, Assembly[] assemblies)
    {
        AssemblyScanner.FindValidatorsInAssemblies(assemblies)
            .ForEach(x => { services.AddScoped(x.InterfaceType, x.ValidatorType); });

        return services;
    }

    public static IServiceCollection AddMappers(this IServiceCollection services)
    {
        return services
             .AddScoped<IMapper<AddCustomerCommand, Customer>, AddCustomerCommandMapper>();
    }
}