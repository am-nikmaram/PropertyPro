using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Infrastructure.Persistence;
using PropertyPro.Infrastructure.Persistence.Interceptors;

namespace PropertyPro.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        //services.AddDbContext<ApplicationDbContext>(options =>
        //{
        //    options.UseSqlServer(
        //        configuration.GetConnectionString("DefaultConnection"));

        //});


        services.AddDbContext<ApplicationDbContext>(
    (serviceProvider, options) =>
    {
        //options.UseSqlServer("DefaultConnection");
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

        options.AddInterceptors(
            serviceProvider.GetRequiredService<
                AuditableEntityInterceptor>());
    });



        services.AddScoped<IApplicationDbContext>(
         provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<DatabaseInitializer>();

        return services;
    }
}