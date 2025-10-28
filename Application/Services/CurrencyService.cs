using CurrencyUpdaterService.Application.HttpClient.Currencies;
using CurrencyUpdaterService.Application.Interfaces.Repository;
using CurrencyUpdaterService.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace CurrencyUpdaterService.Application.Services;

public class CurrencyService(
    ICurrencyRepository currencyRepository,
    ICurrenciesController currenciesController,
    ILogger<CurrencyService> logger) : ICurrencyService
{
    private readonly ICurrencyRepository _currencyRepository = currencyRepository;
    private readonly ICurrenciesController _currenciesController = currenciesController;
    private readonly ILogger<CurrencyService> _logger = logger;

    // SemaphoreSlim for lightweighted job lock
    private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    // Job running fast check flag
    private static volatile int _isRunning = 0;

    public async Task<IEnumerable<Currency>> GetListAsync(CancellationToken cancellationToken)
    {
        return await _currencyRepository.GetListAsync(cancellationToken);
    }

    public async Task<Currency?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await _currencyRepository.GetByNameAsync(name, cancellationToken);
    }

    public async Task<bool> RefreshCurrenciesAsync(CancellationToken cancellationToken)
    {
        bool result = false;

        if (Interlocked.CompareExchange(ref _isRunning, 0, 0) == 1)
        {
            Console.WriteLine("The Currencies Refreshing Job is already running, exit...");
            return false;
        }

        // acquire lightweight lock
        await _semaphore.WaitAsync();

        try
        {
            // Устанавливаем флаг после получения семафора
            Interlocked.Exchange(ref _isRunning, 1);
            result = await RefreshCurrenciesJobAsync(cancellationToken);
        }
        finally
        {
            Interlocked.Exchange(ref _isRunning, 0);

            // release lightweight lock
            _semaphore.Release();
        }

        return result;
    }

    public async Task<bool> RefreshCurrenciesJobAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Start refreshing currencies...");

        var response = await _currenciesController.GetAsync(cancellationToken);

        if (response is null ||
            !response.IsSuccessStatusCode ||
            !response.IsSuccessful ||
            response.Content is null)
        {
            _logger.LogError(response?.Error,
                "Unable to obtain currencies from {endpoint}, error message", response?.Error?.Uri);

            return false;
        }

        var currenciesToUpload = response
            .Content
            .Valutes
            .Select(x => new Currency { Id = x.Id, Name = x.CharCode, Rate = x.VunitRateDecimal })
            .ToList();

        var uploadResult = await _currencyRepository.UploadAsync(currenciesToUpload, cancellationToken);
        
        Console.WriteLine("Finish refreshing currencies...");
        
        return uploadResult;
    }
}