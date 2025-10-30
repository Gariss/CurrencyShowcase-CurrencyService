using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;

namespace UserService.Database;

public class CurrencyShowcaseContext : DbContext
{
    public CurrencyShowcaseContext(
        DbContextOptions<CurrencyShowcaseContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}