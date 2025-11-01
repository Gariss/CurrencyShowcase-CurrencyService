using Microsoft.Extensions.Logging;
using FavoritesService.Application.Interfaces.Repository;
using FavoritesService.Domain.Entities;

namespace FavoritesService.Application.Services;

public class FavoritesService(
    IUserCurrencyRepository repository, 
    ILogger<FavoritesService> logger) : IFavoritesService
{
    private readonly IUserCurrencyRepository _repository = repository;
    private readonly ILogger<FavoritesService> _logger = logger;

    public async Task<List<Currency>> GetUserFavoritesAsync(Guid userId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting favorite currencies for user {UserId}", userId);
        var favorites = await _repository.GetByUserIdAsync(userId, cancellationToken);
        _logger.LogInformation("Found {Count} favorite currencies for user {UserId}", favorites.Count, userId);

        return favorites;
    }

    public async Task AddToFavoritesAsync(Guid userId, IReadOnlyCollection<string> currencyIds, 
        CancellationToken cancellationToken)
    {
        if (!currencyIds.Any())
        {
            _logger.LogWarning("Attempt to add empty currency list for user {UserId}", userId);
            return;
        }

        _logger.LogInformation("Adding {Count} currencies to favorites for user {UserId}",
            currencyIds.Count, userId);

        await _repository.AddByUserIdAsync(userId, currencyIds.ToArray(), cancellationToken);

        _logger.LogInformation("Successfully added currencies to favorites for user {UserId}", userId);
    }

    public async Task RemoveFromFavoritesAsync(Guid userId, IReadOnlyCollection<string> currencyIds, 
        CancellationToken cancellationToken)
    {
        if (!currencyIds.Any())
        {
            _logger.LogWarning("Attempt to remove empty currency list for user {UserId}", userId);
            return;
        }

        _logger.LogInformation("Removing {Count} currencies from favorites for user {UserId}",
            currencyIds.Count, userId);

        await _repository.RemoveByUserIdAsync(userId, currencyIds.ToArray(), cancellationToken);

        _logger.LogInformation("Successfully removed currencies from favorites for user {UserId}", userId);
    }
}