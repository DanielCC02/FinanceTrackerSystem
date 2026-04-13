using FinanceTracker.Domain.Common;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;

public class User : BaseEntity
{
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string? PhoneNumber { get; private set; }

    public UserRole Role { get; private set; }
    public string PasswordHash { get; private set; } = string.Empty;

    public bool EmailConfirmed { get; private set; }

    public ICollection<Account> Accounts { get; private set; } = new List<Account>();
    public ICollection<Category> Categories { get; private set; } = new List<Category>();

    private User() { }

    public User(string firstName, string lastName, string email, string passwordHash)
    {
        Id = Guid.NewGuid();
        SetFirstName(firstName);
        SetLastName(lastName);
        SetEmail(email);
        SetRole(UserRole.User);
        SetPasswordHash(passwordHash);
        EmailConfirmed = false;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateProfile(string firstName, string lastName, string phoneNumber)
    {
        SetFirstName(firstName);
        SetLastName(lastName);
        SetPhone(phoneNumber);
        UpdatedAt = DateTime.UtcNow;
    }

    public void ConfirmEmail()
    {
        EmailConfirmed = true;
    }

    public void UpdatePassword(string passwordHash)
    {
        SetPasswordHash(passwordHash);
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
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

        SetRole(newRole);
        UpdatedAt = DateTime.UtcNow;
    }

    // VALIDACIONES

    private void SetFirstName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("First name required");

        FirstName = name;
    }

    private void SetLastName(string lastName)
    {
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name required");

        LastName = lastName;
    }

    private void SetPhone(string? phone)
    {
        PhoneNumber = phone;
    }

    private void SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email required");

        if (!email.Contains('@'))
            throw new ArgumentException("Invalid email");

        Email = email.Trim().ToLower();
    }

    private void SetRole(UserRole role)
    {
        if (!Enum.IsDefined(typeof(UserRole), role))
            throw new ArgumentException("Invalid role");

        Role = role;
    }

    private void SetPasswordHash(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password required");

        PasswordHash = passwordHash;
    }
}