using FinanceTracker.Domain.Common;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Domain.Entities;

public class Category : BaseEntity
{
    public Guid? UserId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public TransactionType? SuggestedType { get; private set; }

    public string Icon { get; private set; } = string.Empty;

    public User? User { get; private set; }

    public ICollection<Transaction> Transactions { get; private set; } = new List<Transaction>();

    private Category() { }

    public Category(Guid? userId, string name, string? icon = null, TransactionType? suggestedType = null)
    {
        Id = Guid.NewGuid();
        SetUserId(userId);
        SetName(name);
        SetIcon(icon);
        SetSuggestedType(suggestedType);
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string? icon = null, TransactionType? suggestedType = null)
    {
        SetName(name);
        SetIcon(icon);
        SetSuggestedType(suggestedType);
        UpdatedAt = DateTime.UtcNow;
    }

    public void Delete()
    {
        if (IsDeleted)
            throw new InvalidOperationException("Category already deleted");

        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
    }

    private void SetUserId(Guid? userId)
    {
        if (userId.HasValue && userId.Value == Guid.Empty)
            throw new ArgumentException("UserId cannot be empty");

        UserId = userId;
    }

    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty");

        Name = name.Trim();
    }

    private void SetIcon(string? icon)
    {
        if (string.IsNullOrWhiteSpace(icon))
        {
            Icon = "default";
            return;
        }

        Icon = icon.Trim().ToLower();
    }

    private void SetSuggestedType(TransactionType? type)
    {
        if (type.HasValue && !Enum.IsDefined(typeof(TransactionType), type.Value))
            throw new ArgumentException("Invalid suggested type");

        SuggestedType = type;
    }
}