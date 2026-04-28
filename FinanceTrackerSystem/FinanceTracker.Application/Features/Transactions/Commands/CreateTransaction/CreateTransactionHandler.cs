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
        private readonly ICurrentUserService _currentUser;

        public CreateTransactionHandler(IApplicationDbContext dbContext, IMapper mapper, ICurrentUserService currentUser)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<TransactionDto> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            // 🔥 1. Validar account (y ownership)
            var account = await _dbContext.Accounts
                .FirstOrDefaultAsync(a =>
                    a.Id == request.AccountId &&
                    a.UserId == _currentUser.UserId,
                    cancellationToken)
                ?? throw new KeyNotFoundException("Account not found");


            // 🔥 2. Validar categoría (si viene)
            if (request.CategoryId.HasValue)
            {
                var category = await _dbContext.Categories
                    .FirstOrDefaultAsync(c =>
                        c.Id == request.CategoryId &&
                        (c.UserId == _currentUser.UserId || c.UserId == null),
                        cancellationToken)
                    ?? throw new KeyNotFoundException("Category not found");

                // 🔥 3. Validación PRO (coherencia tipo)
                if (category.SuggestedType.HasValue &&
                    category.SuggestedType != request.Type)
                {
                    throw new InvalidOperationException(
                        $"Category is for {category.SuggestedType}, not {request.Type}");
                }
            }

            // 🔥 4. Crear transacción
            var transaction = new Transaction(
                request.AccountId,
                request.CategoryId,
                request.Amount,
                request.Type,
                request.Description,
                request.Date);

            _dbContext.Transactions.Add(transaction);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<TransactionDto>(transaction);
        }
    }
}
