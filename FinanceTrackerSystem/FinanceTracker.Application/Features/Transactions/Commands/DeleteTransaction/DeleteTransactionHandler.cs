using FinanceTracker.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Features.Transactions.Commands.DeleteTransaction
{
    public class DeleteTransactionHandler : IRequestHandler<DeleteTransactionCommand, Unit>
    {
        private readonly IApplicationDbContext _dbContext;

        public DeleteTransactionHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _dbContext.Transactions
                .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken) 
                ?? throw new KeyNotFoundException("Transaction not found");

            transaction.Delete();
            
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
