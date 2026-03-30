using AutoMapper;
using AutoMapper.QueryableExtensions;
using FinanceTracker.Application.Features.Transactions.DTOs;
using FinanceTracker.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Features.Transactions.Queries.GetTransactionById
{
    public class GetTransactionByIdHandler : IRequestHandler<GetTransactionByIdQuery, TransactionDto>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetTransactionByIdHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<TransactionDto> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
        {
            var transaction = await _dbContext.Transactions
                .Where(t => t.Id == request.Id)
                .ProjectTo<TransactionDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new KeyNotFoundException("Transaction not found.");

            return _mapper.Map<TransactionDto>(transaction);
        }
    }
}
