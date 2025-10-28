using CurrencyUpdaterService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurrencyUpdaterService.Database.Configurations;

internal class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(e => e.Name, "IX_Currency_Name")
            .IsUnique();

        builder.Property(x => x.Rate)
            .HasColumnType("numeric(18,4)");
    }
}