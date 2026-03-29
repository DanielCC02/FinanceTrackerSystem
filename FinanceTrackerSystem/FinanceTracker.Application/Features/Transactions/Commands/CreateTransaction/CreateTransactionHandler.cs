

using AutoMapper;
using FinanceTracker.Application.Features.Transactions.DTOs;
using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Features.Transactions.Commands.CreateTransaction
{
    public class CreateTransactionHandler : IRequestHandler<CreateTransactionCommand, TransactionDto> 
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public CreateTransactionHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<TransactionDto> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
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

            if (!Enum.IsDefined(typeof(TransactionType), request.Type))
                throw new Exception("Invalid transaction type");

            var type = (TransactionType)request.Type;

            var transaction = new Transaction(
                request.AccountId,
                request.CategoryId,
                request.Amount,
                type,
                request.Description,
                request.Date);

            _dbContext.Transactions.Add(transaction);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<TransactionDto>(transaction);
        }
    }
}
