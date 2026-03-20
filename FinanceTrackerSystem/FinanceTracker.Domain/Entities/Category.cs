using FinanceTracker.Domain.Common;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Domain.Entities;

public class Category : BaseEntity
{
    public Guid UserId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public CategoryType Type { get; private set; }

    public User? User { get; private set; }

    public ICollection<Transaction> Transactions { get; private set; } = new List<Transaction>();

    private Category() { }

    public Category(Guid userId, string name, CategoryType type)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Name = name;
        Type = type;
        CreatedAt = DateTime.UtcNow;
    }
}
