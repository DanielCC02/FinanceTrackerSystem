using FinanceTracker.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace FinanceTracker.Application.Features.Accounts.Commands.DeleteAccount
{
    public class AccountDeleteHandler : IRequestHandler<AccountDeleteCommand, Unit>
    {
        private readonly IApplicationDbContext _dbContext;

        public AccountDeleteHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(AccountDeleteCommand request, CancellationToken cancellationToken)
        {
            var account = await _dbContext.Accounts
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken)
                ?? throw new Exception("Account not found");

            account.Delete();

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
