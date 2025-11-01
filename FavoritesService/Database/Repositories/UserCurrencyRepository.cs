using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FavoritesService.Domain.Entities;
using FavoritesService.Application.Interfaces.Repository;

namespace FavoritesService.Database.Repositories;

public class UserCurrencyRepository(CurrencyShowcaseContext dbContext, ILogger<UserCurrencyRepository> logger) : IUserCurrencyRepository
{
    private readonly CurrencyShowcaseContext _dbContext = dbContext;
    private readonly ILogger<UserCurrencyRepository> _logger = logger;

    public async Task<List<Currency>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _dbContext
            .UserCurrencies
            .Where(x => x.UserId == userId)
            .Include(x => x.Currency)
            .Select(x => x.Currency)
            .ToListAsync(cancellationToken);
    }

    public async Task AddByUserIdAsync(Guid userId, IReadOnlyCollection<string> currencyIds, 
        CancellationToken cancellationToken)
    {
        var existingIds = await _dbContext
            .UserCurrencies
            .Where(
                x => x.UserId == userId && 
                currencyIds.Contains(x.CurrencyId))
            .Select(x => x.CurrencyId)
            .ToListAsync(cancellationToken);

        var newUserCurrencies = currencyIds
            .Except(existingIds)
            .Select(x => new UserCurrency
            {
                UserId = userId,
                CurrencyId = x
            })
            .ToList();

        if (newUserCurrencies.Any())
        {
            await _dbContext.UserCurrencies.AddRangeAsync(newUserCurrencies, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task RemoveByUserIdAsync(Guid userId, IReadOnlyCollection<string> currencyIds, CancellationToken cancellationToken)
    {
        var userCurrenciesToRemove = await _dbContext
            .UserCurrencies
            .Where(uc => uc.UserId == userId && currencyIds.Contains(uc.CurrencyId))
            .ToListAsync(cancellationToken);

        if (userCurrenciesToRemove.Any())
        {
            _dbContext.UserCurrencies.RemoveRange(userCurrenciesToRemove);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
