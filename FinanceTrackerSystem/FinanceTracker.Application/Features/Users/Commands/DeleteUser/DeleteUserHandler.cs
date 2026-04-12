using FinanceTracker.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Features.Users.Commands.DeleteUser
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, Unit>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUser;

        public DeleteUserHandler(IApplicationDbContext dbContext, ICurrentUserService currentUser)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            if (_currentUser.Role != "Admin")
                throw new UnauthorizedAccessException("Only admins can delete users");

            if (_currentUser.UserId == request.Id)
                throw new InvalidOperationException("Admins cannot delete themselves");

            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken)
                ?? throw new KeyNotFoundException("User not found");

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

            user.Deactivate();

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}