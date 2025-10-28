using CurrencyUpdaterService.Application.BackgroundServices;
using CurrencyUpdaterService.Application.HttpClient.Currencies;
using CurrencyUpdaterService.Application.Interfaces.Repository;
using CurrencyUpdaterService.Application.Services;
using CurrencyUpdaterService.Database;
using CurrencyUpdaterService.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Refit;
using Serilog;
using System.Globalization;
using System.Text;

namespace CurrencyUpdaterService.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("FinancesDb"));
        var dataSource = dataSourceBuilder.Build();

        services.AddDbContext<CurrencyUpdaterContext>(optionsBuilder =>
        {
            optionsBuilder.UseNpgsql(dataSource, x => x.MigrationsAssembly("CurrencyUpdaterService.Database"));
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
        services.Configure<UpdateJobOptions>(configuration.GetSection(nameof(UpdateJobOptions)));
        services.AddHostedService<UpdateCurrenciesBackgroundService>();

        services.AddScoped<ICurrencyService, CurrencyService>();
        services.AddScoped<ICurrencyRepository, CurrencyRepository>();

        return services;
    }

    public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        // For proper deserializing RU codepage and culture, easy way
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        services.AddRefitClient<ICurrenciesController>(provider => new RefitSettings
        {
            ContentSerializer = new CbrXmlContentSerializer()
        })
        .ConfigureHttpClient(client =>
        {
            client.BaseAddress = new Uri(configuration.GetConnectionString("CbrfApi")!);
            client.DefaultRequestHeaders.Add("Accept", "application/xml");
            client.DefaultRequestHeaders.Add("Accept-Charset", "windows-1251,utf-8");
        });

        return services;
    }
}