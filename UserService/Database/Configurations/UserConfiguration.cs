using UserService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UserService.Database.Configurations;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
               .ValueGeneratedOnAdd()
               .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(u => u.CreatedAt)
               .ValueGeneratedOnAdd()
               .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

        builder.Property(u => u.Name)
               .HasMaxLength(30);

        builder.Property(u => u.Email)
               .HasMaxLength(100);

        builder.HasIndex(u => u.Login)
               .IsUnique();
    }
}