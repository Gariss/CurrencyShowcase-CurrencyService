using CurrencyService.Domain.Entities;

namespace CurrencyService.Application.Services;

public interface ICurrencyService
{
    Task<IEnumerable<Currency>> GetListAsync(CancellationToken cancellationToken);

    Task<Currency?> GetByCharCodeAsync(string charCode, CancellationToken cancellationToken);

    Task<bool> RefreshCurrenciesAsync(CancellationToken cancellationToken);
}
