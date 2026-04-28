using AutoMapper;
using FinanceTracker.Application.Features.Transactions.DTOs;
using FinanceTracker.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Features.Transactions.Commands.UpdateTransaction
{
    public class UpdateTransactionHandler : IRequestHandler<UpdateTransactionCommand, TransactionDto>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public UpdateTransactionHandler(IApplicationDbContext dbContext, IMapper mapper, ICurrentUserService currentUser)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<TransactionDto> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
        {
            // 🔥 1. Traer transacción con validación de ownership
            var transaction = await _dbContext.Transactions
                .FirstOrDefaultAsync(t =>
                    t.Id == request.Id &&
                    t.Account!.UserId == _currentUser.UserId,
                    cancellationToken)
                ?? throw new KeyNotFoundException("Transaction not found");

            // 🔥 2. Validar categoría (si viene)
            if (request.CategoryId.HasValue)
            {
                var category = await _dbContext.Categories
                    .FirstOrDefaultAsync(c =>
                        c.Id == request.CategoryId &&
                        (c.UserId == _currentUser.UserId || c.UserId == null),
                        cancellationToken)
                    ?? throw new KeyNotFoundException("Category not found");

                // 🔥 3. Validar coherencia (PRO)
                if (category.SuggestedType.HasValue &&
                    category.SuggestedType != request.Type)
                {
                    throw new InvalidOperationException(
                        $"Category is for {category.SuggestedType}, not {request.Type}");
                }
            }

            transaction.Update(request.CategoryId, request.Amount, request.Type, request.Description, request.Date);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<TransactionDto>(transaction);
        }
    }
}
