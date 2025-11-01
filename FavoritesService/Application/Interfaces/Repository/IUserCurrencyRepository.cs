using FavoritesService.Domain.Entities;

namespace FavoritesService.Application.Interfaces.Repository;

public interface IUserCurrencyRepository
{
    Task<List<Currency>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    Task AddByUserIdAsync(Guid userId, IReadOnlyCollection<string> currencyIds, CancellationToken cancellationToken);
    Task RemoveByUserIdAsync(Guid userId, IReadOnlyCollection<string> currencyIds, CancellationToken cancellationToken);
}
