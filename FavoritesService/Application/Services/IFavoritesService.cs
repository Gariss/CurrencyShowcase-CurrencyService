using FavoritesService.Domain.Entities;

namespace FavoritesService.Application.Services;

public interface IFavoritesService
{
    Task<List<Currency>> GetUserFavoritesAsync(Guid userId, CancellationToken cancellationToken);
    Task AddToFavoritesAsync(Guid userId, IReadOnlyCollection<string> currencyIds, CancellationToken cancellationToken);
    Task RemoveFromFavoritesAsync(Guid userId, IReadOnlyCollection<string> currencyIds, CancellationToken cancellationToken);
}
