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
        SetUserId(userId);
        SetName(name);
        SetType(type);
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string name, AccountType type)
    {
        SetName(name);
        SetType(type);
        UpdatedAt = DateTime.UtcNow;
    }

    public void Delete()
    {
        if (IsDeleted)
            throw new InvalidOperationException("Account already deleted");

        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
    }
    private void SetUserId(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("UserId cannot be empty");
        UserId = userId;
    }

    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty");
        Name = name;
    }

    private void SetType(AccountType type)
    {
        if (!Enum.IsDefined(typeof(AccountType), type))
            throw new ArgumentException("Invalid account type");
        Type = type;
    }
}