using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Features.Accounts.DTOs
{
    public class AccountDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }
}
