using CurrencyUpdaterService.Domain.Entities;

namespace CurrencyUpdaterService.Application.Interfaces.Repository;

public interface ICurrencyRepository
{
    Task<IEnumerable<Currency>> GetListAsync(CancellationToken cancellationToken);

    Task<Currency?> GetByNameAsync(string name, CancellationToken cancellationToken);

    Task<bool> UploadAsync(IReadOnlyCollection<Currency> currencyItems, CancellationToken cancellationToken);
}
