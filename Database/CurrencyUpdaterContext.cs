using Microsoft.EntityFrameworkCore;
using CurrencyUpdaterService.Domain.Entities;

namespace CurrencyUpdaterService.Database;

public class CurrencyUpdaterContext : DbContext
{
    public CurrencyUpdaterContext(
        DbContextOptions<CurrencyUpdaterContext> options) : base(options)
    {
    }

    public DbSet<Currency> Currencies => Set<Currency>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}