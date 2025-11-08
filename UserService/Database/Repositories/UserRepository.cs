using UserService.Application.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserService.Domain.Entities;

namespace UserService.Database.Repositories;

public class UserRepository(CurrencyShowcaseContext dbContext, ILogger<UserRepository> logger) : IUserRepository
{
    private readonly CurrencyShowcaseContext _dbContext = dbContext;
    private readonly ILogger<UserRepository> _logger = logger;

    public async Task<bool> AddAsync(User newUser, CancellationToken cancellationToken)
    {
        try
        {
            await _dbContext
                .Users
                .AddAsync(newUser, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to create User");
            return false;
        }
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext
            .Users
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<User?> GetByLoginAsync(string login, CancellationToken cancellationToken)
    {
        return await _dbContext
            .Users
            .SingleOrDefaultAsync(x => x.Login == login, cancellationToken);
    }
}
