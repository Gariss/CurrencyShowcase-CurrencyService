using CurrencyUpdaterService.Application.Interfaces.Repository;
using CurrencyUpdaterService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CurrencyUpdaterService.Database.Repositories;

public class CurrencyRepository(CurrencyUpdaterContext dbContext, ILogger<CurrencyRepository> logger) : ICurrencyRepository
{
    private readonly CurrencyUpdaterContext _dbContext = dbContext;
    private readonly ILogger<CurrencyRepository> _logger = logger;

    public async Task<Currency?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await _dbContext
            .Currencies
            .AsNoTracking()
            .Where (c => c.Name == name)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Currency>> GetListAsync(CancellationToken cancellationToken)
    {
        return await _dbContext
            .Currencies
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> UploadAsync(IReadOnlyCollection<Currency> currencyItems, CancellationToken cancellationToken)
    {
        try
        {
            List<Currency> existingCurrencies = await _dbContext
                .Currencies
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var currenciesToUpdate = currencyItems
                .IntersectBy(existingCurrencies.Select(x => x.Id), u => u.Id)
                .ToList();

            var currenciesToInsert = currencyItems
                .ExceptBy(existingCurrencies.Select(x => x.Id), u => u.Id)
                .ToList();

            await _dbContext.Currencies.AddRangeAsync(currenciesToInsert, cancellationToken);
            _dbContext.Currencies.UpdateRange(currenciesToUpdate);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to upload currencies");
            return false;
        }
    }
}
