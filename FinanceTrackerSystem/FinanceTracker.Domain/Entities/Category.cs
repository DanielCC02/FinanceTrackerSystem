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
        SetUserId(userId);
        SetName(name);
        SetType(type);
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string name, CategoryType type)
    {
        SetName(name);
        SetType(type);
        UpdatedAt = DateTime.UtcNow;
    }

    public void Delete()
    {
        if (IsDeleted)
            throw new InvalidOperationException("Category already deleted");

        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
    }

    private void SetUserId(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("UserId cannot be empty");
        UserId = userId;
    }

    private void SetName (String name) { 
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty");
        Name = name;
    }

    private void SetType(CategoryType type)
    {
        if (!Enum.IsDefined(typeof(CategoryType), type))
            throw new ArgumentException("Invalid category type");
        Type = type;
    }
}
