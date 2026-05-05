using FinanceTracker.Domain.Common;

namespace FinanceTracker.Domain.Entities;

public class EmailConfirmationToken : BaseEntity
{
    public Guid UserId { get; private set; }
    public string Token { get; private set; } = string.Empty;
    public DateTime ExpiresAt { get; private set; }
    public bool IsUsed { get; private set; }

    private EmailConfirmationToken() { }

    public EmailConfirmationToken(Guid userId, string token)
    {
        Id = Guid.NewGuid(); 
        UserId = userId;
        Token = token;
        ExpiresAt = DateTime.UtcNow.AddHours(1);
        CreatedAt = DateTime.UtcNow; 
    }

    public void MarkAsUsed()
    {
        IsUsed = true;
    }
}