using FinanceTracker.Domain.Common;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Domain.Entities;

public class User : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public UserRole Role { get; private set; }
    public string PasswordHash { get; private set; } = string.Empty;

    public ICollection<Account> Accounts { get; private set; } = new List<Account>();
    public ICollection<Category> Categories { get; private set; } = new List<Category>();

    private User() { }

    public User(string name, string email, UserRole role, string passwordHash)
    {
        Id = Guid.NewGuid();
        SetName(name);
        SetEmail(email);
        SetRole(role);
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

    public void ChangeRole(UserRole newRole)
    {
        if (Role == newRole)
            throw new InvalidOperationException("User already has this role");

        Role = newRole;
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

    private void SetRole(UserRole role)
    {
        if (!Enum.IsDefined(typeof(UserRole), role))
        throw new ArgumentException("Invalid user role.");

        Role = role;
    }

    private void SetPassword(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password cannot be empty");

        PasswordHash = passwordHash;
    }
}
