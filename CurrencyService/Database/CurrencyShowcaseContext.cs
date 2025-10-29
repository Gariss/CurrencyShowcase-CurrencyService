using Microsoft.EntityFrameworkCore;
using CurrencyService.Domain.Entities;

namespace CurrencyService.Database;

public class CurrencyShowcaseContext : DbContext
{
    public CurrencyShowcaseContext(
        DbContextOptions<CurrencyShowcaseContext> options) : base(options)
    {
    }

    public DbSet<Currency> Currencies => Set<Currency>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}