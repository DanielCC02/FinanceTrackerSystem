using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Persistence.Seed;

public static class CategorySeeder
{
    public static async Task SeedAsync(FinanceDbContext context)
    {
        var defaultCategories = new List<Category>
        {
            new Category(null, "food", CategoryType.Expense, "food"),
            new Category(null, "transport", CategoryType.Expense, "car"),
            new Category(null, "health", CategoryType.Expense, "heart"),
            new Category(null, "entertainment", CategoryType.Expense, "gamepad"),
            new Category(null, "shopping", CategoryType.Expense, "shopping-bag"),
            new Category(null, "bills", CategoryType.Expense, "file-invoice"),

            new Category(null, "salary", CategoryType.Income, "money"),
            new Category(null, "freelance", CategoryType.Income, "laptop"),
            new Category(null, "investments", CategoryType.Income, "chart-line")
        };

        foreach (var category in defaultCategories)
        {
            var exists = await context.Categories.AnyAsync(c =>
                c.UserId == null &&
                c.Name == category.Name &&
                c.Type == category.Type);

            if (!exists)
            {
                context.Categories.Add(category);
            }
        }

        await context.SaveChangesAsync();
    }
}