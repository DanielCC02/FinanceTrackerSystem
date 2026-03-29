using FinanceTracker.Domain.Common;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Domain.Entities;

public class Transaction : BaseEntity
{
    public Guid AccountId { get; private set; }
    public Guid? CategoryId { get; private set; }
    public decimal Amount { get; private set; }
    public TransactionType Type { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public DateTime Date { get; private set; }

    public Account? Account { get; private set; }
    public Category? Category { get; private set; }

    private Transaction() { }

    public Transaction(Guid accountId, Guid? categoryId, decimal amount, TransactionType type, string description, DateTime date)
    {
        Id = Guid.NewGuid();
        SetAccountId(accountId);
        SetCategoryId(categoryId);
        SetAmount(amount);
        SetType(type);
        SetDescription(description);
        SetDate(date);
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(Guid accountId, Guid? categoryId, decimal amount, TransactionType type, string description, DateTime date)
    {
        SetAccountId(accountId);
        SetCategoryId(categoryId);
        SetAmount(amount);
        SetType(type);
        SetDescription(description);
        SetDate(date);
        UpdatedAt = DateTime.UtcNow;
    }

    public void Delete()
    {
        if (IsDeleted)
            throw new InvalidOperationException("Transaction already deleted");

        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
    }

    // 🔥 MÉTODOS DE DOMINIO (PRO)
    public bool IsIncome() => Type == TransactionType.Income;

    public bool IsExpense() => Type == TransactionType.Expense;

    public decimal GetSignedAmount()
    {
        return Type == TransactionType.Expense ? -Amount : Amount;
    }

    // 🔒 VALIDACIONES INTERNAS

    private void SetAmount(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than zero.");

        Amount = amount;
    }

    private void SetAccountId(Guid accountId)
    {
        if (accountId == Guid.Empty)
            throw new ArgumentException("AccountId cannot be empty.");

        AccountId = accountId;
    }

    private void SetCategoryId(Guid? categoryId)
    {
        if (categoryId.HasValue && categoryId.Value == Guid.Empty)
            throw new ArgumentException("CategoryId cannot be empty.");

        CategoryId = categoryId;
    }

    private void SetDescription(string description)
    {
        Description = description?.Trim() ?? string.Empty;
    }

    private void SetDate(DateTime date)
    {
        if (date > DateTime.UtcNow)
            throw new ArgumentException("Date cannot be in the future.");

        Date = date;
    }

    private void SetType(TransactionType type)
    {
        if (!Enum.IsDefined(typeof(TransactionType), type))
            throw new ArgumentException("Invalid transaction type.");

        Type = type;
    }
}