using FavoritesService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FavoritesService.Database;

public class CurrencyShowcaseContext : DbContext
{
    public CurrencyShowcaseContext(
        DbContextOptions<CurrencyShowcaseContext> options) : base(options)
    {
    }

    public DbSet<UserCurrency> UserCurrencies => Set<UserCurrency>();

    public DbSet<Currency> Currencies => Set<Currency>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        // Exclude from migrations
        modelBuilder.Entity<Currency>()
            .Metadata.SetIsTableExcludedFromMigrations(true);
    }
}