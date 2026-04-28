using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Infrastructure.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> entity)
    {
        entity.HasKey(c => c.Id);

        entity.Property(c => c.Name)
            .HasMaxLength(100)
            .IsRequired();

        entity.Property(c => c.Icon)
            .HasMaxLength(50)
            .IsRequired();

        // 🔥 NUEVO (correcto)
        entity.Property(c => c.SuggestedType)
            .IsRequired(false);

        entity.Property(c => c.IsDeleted)
            .IsRequired();

        entity.HasOne(c => c.User)
            .WithMany(u => u.Categories)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // 🔥 SOFT DELETE GLOBAL
        entity.HasQueryFilter(c => !c.IsDeleted);

        // 🔥 ÍNDICE (opcional mantener)
        entity.HasIndex(c => new { c.Name, c.UserId });
    }
}