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

        entity.Property(c => c.Type)
            .IsRequired();

        entity.Property(c => c.IsDeleted)
            .IsRequired();

        // 🔥 evita duplicados por usuario/global
        entity.HasIndex(c => new { c.Name, c.UserId })
            .IsUnique();

        entity.HasOne(c => c.User)
            .WithMany(u => u.Categories)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // 🔥 soft delete automático
        entity.HasQueryFilter(c => !c.IsDeleted);
    }
}