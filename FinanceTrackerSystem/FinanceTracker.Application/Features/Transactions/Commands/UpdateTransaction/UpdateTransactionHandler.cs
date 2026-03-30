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

        public UpdateTransactionHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<TransactionDto> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _dbContext.Transactions.
                FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken)
                ?? throw new KeyNotFoundException("Transaction not found");
            
            transaction.Update(request.CategoryId, request.Amount, request.Type, request.Description, request.Date);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<TransactionDto>(transaction);
        }
    }
}
