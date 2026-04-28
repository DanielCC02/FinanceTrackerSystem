using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Persistence.Seed;

public static class CategorySeeder
{
    public static async Task SeedAsync(FinanceDbContext context)
    {
        // 🔥 1. Traer todas las categorías globales existentes (1 sola query)
        var existingCategories = await context.Categories
            .Where(c => c.UserId == null)
            .Select(c => new { c.Name, c.SuggestedType })
            .ToListAsync();

        // 🔥 2. Definir categorías por defecto
        var defaultCategories = new List<Category>
        {
            new Category(null, "food", TransactionType.Expense, "food"),
            new Category(null, "transport", TransactionType.Expense, "car"),
            new Category(null, "health", TransactionType.Expense, "heart"),
            new Category(null, "entertainment", TransactionType.Expense, "gamepad"),
            new Category(null, "shopping", TransactionType.Expense, "shopping-bag"),
            new Category(null, "bills", TransactionType.Expense, "file-invoice"),

            new Category(null, "salary", TransactionType.Income, "money"),
            new Category(null, "freelance", TransactionType.Income, "laptop"),
            new Category(null, "investments", TransactionType.Income, "chart-line")
        };

        // 🔥 3. Filtrar solo las que NO existen (en memoria)
        var categoriesToInsert = defaultCategories
            .Where(dc => !existingCategories.Any(ec =>
                ec.Name == dc.Name &&
                ec.SuggestedType == dc.SuggestedType))
            .ToList();

        // 🔥 4. Insertar en batch (si hay nuevas)
        if (categoriesToInsert.Any())
        {
            await context.Categories.AddRangeAsync(categoriesToInsert);
            await context.SaveChangesAsync();
        }
    }
}