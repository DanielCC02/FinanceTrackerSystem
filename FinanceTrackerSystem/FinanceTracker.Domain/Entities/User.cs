using FinanceTracker.Domain.Common;

namespace FinanceTracker.Domain.Entities;

public class User : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;

    public ICollection<Account> Accounts { get; private set; } = new List<Account>();
    public ICollection<Category> Categories { get; private set; } = new List<Category>();

    private User() { }

    public User(string name, string email, string passwordHash)
    {
        Id = Guid.NewGuid();
        SetName(name);
        SetEmail(email);
        SetPassword(passwordHash);
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string email)
    {
        SetName(name);
        SetEmail(email);
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdatePassword(string passwordHash)
    {
        SetPassword(passwordHash);
        UpdatedAt = DateTime.UtcNow;
    }

    public void Delete()
    {
        if (IsDeleted)
            throw new InvalidOperationException("User already deleted");

        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
    }

    // VALIDACIONES INTERNAS

    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty");

        Name = name;
    }

    private void SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty");

        Email = email.Trim().ToLower();
    }

    private void SetPassword(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password cannot be empty");

        PasswordHash = passwordHash;
    }
}
