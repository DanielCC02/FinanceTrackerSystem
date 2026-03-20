using FinanceTracker.Domain.Common;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Domain.Entities;

public class Transaction : BaseEntity
{
    public Guid AccountId { get; private set; }

    public Guid? CategoryId { get; private set; } // 🔥 IMPORTANT

    public decimal Amount { get; private set; }

    public TransactionType Type { get; private set; }

    public string Description { get; private set; } = string.Empty;

    public DateTime Date { get; private set; }

    public Account? Account { get; private set; }

    public Category? Category { get; private set; }

    private Transaction() { }

    public Transaction(Guid accountId, Guid? categoryId, decimal amount, TransactionType type, string description, DateTime date)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than zero.");

        Id = Guid.NewGuid();
        AccountId = accountId;
        CategoryId = categoryId;
        Amount = amount;
        Type = type;
        Description = description;
        Date = date;
        CreatedAt = DateTime.UtcNow;
    }
}