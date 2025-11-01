using Microsoft.EntityFrameworkCore;
using Npgsql;
using Serilog;
using FavoritesService.Database;
using FavoritesService.Application.Interfaces.Repository;
using FavoritesService.Application.Services;

namespace FavoritesService.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("CurrencyShowcaseDb"));
        var dataSource = dataSourceBuilder.Build();

        services.AddDbContext<CurrencyShowcaseContext>(optionsBuilder =>
        {
            optionsBuilder.UseNpgsql(dataSource, x => x.MigrationsAssembly("FavoritesService.Database"));
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
        services.AddScoped<IFavoritesService, Application.Services.FavoritesService>();
        services.AddScoped<IUserCurrencyRepository, Database.Repositories.UserCurrencyRepository>();

        return services;
    }
}