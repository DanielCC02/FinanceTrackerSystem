using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Infrastructure.Persistence.Configurations;

public class EmailConfirmationTokenConfiguration : IEntityTypeConfiguration<EmailConfirmationToken>
{
    public void Configure(EntityTypeBuilder<EmailConfirmationToken> entity)
    {
        entity.HasKey(t => t.Id);

        // =========================
        // TOKEN
        // =========================
        entity.Property(t => t.Token)
            .HasMaxLength(128)
            .IsRequired()
            .IsUnicode(false); // 🔥 evita problemas de encoding

        entity.HasIndex(t => t.Token)
            .IsUnique()
            .HasDatabaseName("IX_EmailConfirmationToken_Token");

        // =========================
        // EXPIRATION
        // =========================
        entity.Property(t => t.ExpiresAt)
            .IsRequired();

        entity.HasIndex(t => t.ExpiresAt);

        // =========================
        // STATUS
        // =========================
        entity.Property(t => t.IsUsed)
            .IsRequired();

        // 🔥 índice compuesto PRO (clave para performance real)
        entity.HasIndex(t => new { t.UserId, t.IsUsed })
            .HasDatabaseName("IX_EmailConfirmationToken_User_Active");

        // =========================
        // RELATIONSHIP
        // =========================
        entity.HasOne<User>()
            .WithMany(u => u.EmailConfirmationTokens)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // =========================
        // TABLE NAME (opcional pero pro)
        // =========================
        entity.ToTable("EmailConfirmationTokens");
    }
}