using FinanceTracker.Domain.Common;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Domain.Entities;

public class Account : BaseEntity
{
    public Guid UserId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public AccountType Type { get; private set; }

    public User? User { get; private set; }

    public ICollection<Transaction> Transactions { get; private set; } = new List<Transaction>();

    private Account() { }

    public Account(Guid userId, string name, AccountType type)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Name = name;
        Type = type;
        CreatedAt = DateTime.UtcNow;
    }
}