using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FavoritesService.Domain.Entities;

namespace FavoritesService.Database.Configurations;

internal class UserCurrencyConfiguration : IEntityTypeConfiguration<UserCurrency>
{
    public void Configure(EntityTypeBuilder<UserCurrency> builder)
    {
        builder.HasKey(uc => uc.Id);

        builder.Property(uc => uc.Id)
              .ValueGeneratedOnAdd()
              .HasDefaultValueSql("gen_random_uuid()");

        builder.HasOne(uc => uc.Currency)
              .WithMany()
              .HasForeignKey(uc => uc.CurrencyId)
              .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(uc => new { uc.UserId, uc.CurrencyId })
              .IsUnique();

        builder.HasIndex(uc => uc.UserId);
    }
}