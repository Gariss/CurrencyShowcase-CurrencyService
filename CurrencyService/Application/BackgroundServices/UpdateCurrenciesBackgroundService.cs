using CurrencyService.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CurrencyService.Application.BackgroundServices;

public class UpdateCurrenciesBackgroundService : BackgroundService
{
    private readonly ILogger<UpdateCurrenciesBackgroundService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private ICurrencyService _currencyService;
    private readonly int _recurringTimeInMinutes;
    private PeriodicTimer _timer;

    public UpdateCurrenciesBackgroundService(
        ILogger<UpdateCurrenciesBackgroundService> logger,
        IOptions<UpdateJobOptions> options,
        IServiceProvider serviceProvider,
        IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _recurringTimeInMinutes = options.Value?.RecurringTimeInMinutes ?? 120;
        _timer = new PeriodicTimer(TimeSpan.FromMinutes(_recurringTimeInMinutes));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting {jobName} Hosted Service", nameof(UpdateCurrenciesBackgroundService));

        while (await _timer.WaitForNextTickAsync(cancellationToken))
        {
            try
            {
                await DoWork(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {jobName} Hosted Service", nameof(UpdateCurrenciesBackgroundService));
            }
        }

        _logger.LogInformation("{jobName} Hosted Service is stopping", nameof(UpdateCurrenciesBackgroundService));
    }

    private async Task DoWork(CancellationToken cancellationToken)
    {
        bool result;
        try
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                _currencyService = scope.ServiceProvider.GetRequiredService<ICurrencyService>();
                result = await _currencyService.RefreshCurrenciesAsync(cancellationToken);
            }

            if (!result)
            {
                throw new Exception("Something went wrong");
            }
            _logger.LogInformation("Currency rates was successfully refreshed at {moment}", DateTime.Now);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while refreshing currency rates");
        }
    }
}
