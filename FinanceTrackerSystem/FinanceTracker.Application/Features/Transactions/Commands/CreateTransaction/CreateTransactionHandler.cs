

using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Features.Transactions.Commands.CreateTransaction
{
    public class CreateTransactionHandler : IRequestHandler<CreateTransactionCommand, Guid> 
    {
        private readonly IApplicationDbContext _dbContext;

        public CreateTransactionHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            var accountExists = await _dbContext.Accounts
                .AnyAsync(a => a.Id == request.AccountId, cancellationToken);

            if (!accountExists)
                throw new Exception("Account not found");

            if (request.CategoryId.HasValue)
            {
                var categoryExists = await _dbContext.Categories
                    .AnyAsync(b => b.Id == request.CategoryId.Value, cancellationToken);

                if (!categoryExists) 
                    throw new Exception("Category not found");
            }

            var transaction = new Transaction(
                request.AccountId,
                request.CategoryId,
                request.Amount,
                request.Type,
                request.Description,
                request.Date);

            _dbContext.Transactions.Add(transaction);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return transaction.Id;
        }
    }
}
