using FinanceTracker.Domain.Common;

namespace FinanceTracker.Domain.Entities
{
    public class PasswordResetToken : BaseEntity
    {
        public Guid UserId { get; private set; }
        public string Token { get; private set; } = string.Empty;
        public DateTime Expiration { get; private set; }
        public bool IsUsed { get; private set; }

        private PasswordResetToken() { }

        public PasswordResetToken(Guid userId, string token)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Token = token;
            Expiration = DateTime.UtcNow.AddMinutes(15);
            IsUsed = false;
            CreatedAt = DateTime.UtcNow;
        }

        public void MarkAsUsed()
        {
            if (IsUsed)
                throw new InvalidOperationException("Token already used");

            IsUsed = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public bool IsValid()
        {
            return !IsUsed && Expiration > DateTime.UtcNow;
        }
    }
}