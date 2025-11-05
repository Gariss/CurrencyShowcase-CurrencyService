using UserService.Application.Interfaces.Repository;
using UserService.Application.Services;
using UserService.Database;
using UserService.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Serilog;
using UserService.Application.Infrastructure;
using UserService.Application.Interfaces;

namespace UserService.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("CurrencyShowcaseDb"));
        var dataSource = dataSourceBuilder.Build();

        services.AddDbContext<CurrencyShowcaseContext>(optionsBuilder =>
        {
            optionsBuilder.UseNpgsql(dataSource, x => x.MigrationsAssembly("UserService.Database"));
        });

        return services;
    }

    public static IServiceCollection AddCustomLogging(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSerilog((serviceProvider, loggerConfiguration) =>
        {
            loggerConfiguration
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console();
        });

        return services;
    }

    public static IServiceCollection AddInternalModules(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptionsSection = configuration.GetRequiredSection(nameof(JwtOptions));
        services.Configure<JwtOptions>(jwtOptionsSection);

        services.AddTransient<IJwtProvider, JwtProvider>();
        services.AddTransient<IHashProvider, HashProvider>();

        services.AddScoped<IUserService, Application.Services.UserService>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}