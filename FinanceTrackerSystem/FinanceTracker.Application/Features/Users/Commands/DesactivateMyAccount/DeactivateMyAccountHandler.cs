

using FinanceTracker.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Features.Users.Commands.DesactivateMyAccount
{
    public class DeactivateMyAccountHandler : IRequestHandler<DeactivateMyAccountCommand, Unit>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ICurrentUserService _currentUser;

        public DeactivateMyAccountHandler(IApplicationDbContext dbContext, ICurrentUserService currentUser, IPasswordHasher passwordHasher)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
            _passwordHasher = passwordHasher;
        }

        public async Task<Unit> Handle(DeactivateMyAccountCommand request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == _currentUser.UserId, cancellationToken)
            ?? throw new KeyNotFoundException("User not found");

            var isValidPassword = _passwordHasher.VerifyPassword(user.PasswordHash, request.Password);

            if (!isValidPassword)
            {
                throw new UnauthorizedAccessException("Invalid password");
            }

            var accounts = await _dbContext.Accounts
                .Where(a => a.UserId == user.Id)
                .ToListAsync(cancellationToken);

            var accountIds = accounts.Select(a => a.Id).ToList();

            var transactions = await _dbContext.Transactions
                .Where(t => accountIds.Contains(t.AccountId))
                .ToListAsync(cancellationToken);

            foreach (var transaction in transactions)
            {
                transaction.Delete();
            }

            foreach (var account in accounts)
            {
                account.Delete();
            }

            user.Delete(); // o Deactivate()

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
