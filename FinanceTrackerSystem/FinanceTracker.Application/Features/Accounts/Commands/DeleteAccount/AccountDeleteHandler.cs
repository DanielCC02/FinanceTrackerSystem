using FinanceTracker.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace FinanceTracker.Application.Features.Accounts.Commands.DeleteAccount
{
    public class AccountDeleteHandler : IRequestHandler<AccountDeleteCommand, Unit>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUser;

        public AccountDeleteHandler(IApplicationDbContext dbContext, ICurrentUserService currentUser)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
        }

        public async Task<Unit> Handle(AccountDeleteCommand request, CancellationToken cancellationToken)
        {
            var account = await _dbContext.Accounts
                .FirstOrDefaultAsync(a => a.Id == request.Id && a.UserId == _currentUser.UserId, cancellationToken)
                ?? throw new KeyNotFoundException("Account not found");

            var transactions = await _dbContext.Transactions
            .Where(t => t.AccountId == account.Id)
            .ToListAsync(cancellationToken);

            foreach (var transaction in transactions)
            {
                transaction.Delete();
            }

            account.Delete();

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
