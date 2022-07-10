using Customers.Domain.Contracts;
using Customers.Infra.Repositories;
using EntityAbstractions.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Customers.Infra.Config;

public static class DependencyInjectionConfig
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddScoped<IErrorContext, ErrorContext>()
            .AddDbContext<DbContext, Context>(options => options.UseSqlServer(configuration.GetConnectionString("Main")))
            .AddScoped<ICustomerRepository, CustomerRepository>();
    }
}