using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        entity.HasKey(u => u.Id);

        entity.Property(u => u.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        entity.Property(u => u.LastName)
            .HasMaxLength(100)
            .IsRequired();

        entity.Property(u => u.Email)
            .HasMaxLength(150)
            .IsRequired();

        entity.HasIndex(u => u.Email)
            .IsUnique();

        entity.Property(u => u.PasswordHash)
            .IsRequired();

       
        entity.HasMany(u => u.Accounts)
            .WithOne(a => a.User)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasMany(u => u.Categories)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.Property(u => u.IsDeleted)
            .IsRequired();

        entity.HasQueryFilter(u => !u.IsDeleted);
    }
}