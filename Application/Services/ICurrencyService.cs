using CurrencyUpdaterService.Domain.Entities;

namespace CurrencyUpdaterService.Application.Services;

public interface ICurrencyService
{
    Task<IEnumerable<Currency>> GetListAsync(CancellationToken cancellationToken);

    Task<Currency?> GetByNameAsync(string name, CancellationToken cancellationToken);

    Task<bool> RefreshCurrenciesAsync(CancellationToken cancellationToken);
}
