using CurrencyService.Application.BackgroundServices;
using CurrencyService.Application.HttpClient.Currencies;
using CurrencyService.Application.Interfaces.Repository;
using CurrencyService.Application.Services;
using CurrencyService.Database;
using CurrencyService.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Refit;
using Serilog;
using System.Text;

namespace CurrencyService.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("CurrencyShowcaseDb"));
        var dataSource = dataSourceBuilder.Build();

        services.AddDbContext<CurrencyShowcaseContext>(optionsBuilder =>
        {
            optionsBuilder.UseNpgsql(dataSource, x => x.MigrationsAssembly("CurrencyService.Database"));
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

        services.AddScoped<ICurrencyService, Application.Services.CurrencyService>();
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