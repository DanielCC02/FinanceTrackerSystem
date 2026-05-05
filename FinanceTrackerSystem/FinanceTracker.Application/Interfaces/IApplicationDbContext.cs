using FinanceTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; }
        DbSet<Account> Accounts { get; }
        DbSet<Category> Categories { get; }
        DbSet<Transaction> Transactions { get; }
        DbSet<PasswordResetToken> PasswordResetTokens { get; }
        DbSet<EmailConfirmationToken> EmailConfirmationTokens { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
