using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Infrastructure.Persistence.Configurations;

public class PasswordResetTokenConfiguration : IEntityTypeConfiguration<PasswordResetToken>
{
    public void Configure(EntityTypeBuilder<PasswordResetToken> entity)
    {
        entity.HasKey(t => t.Id);

        entity.Property(t => t.Token)
            .HasMaxLength(64)
            .IsRequired();

        entity.HasIndex(t => t.Token)
            .IsUnique()
            .HasDatabaseName("IX_PasswordResetToken_Token");

        entity.HasIndex(t => t.Expiration);

        entity.Property(t => t.Expiration)
            .IsRequired();

        entity.Property(t => t.IsUsed)
            .IsRequired();

        entity.HasOne<User>()
            .WithMany(u => u.PasswordResetTokens)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}