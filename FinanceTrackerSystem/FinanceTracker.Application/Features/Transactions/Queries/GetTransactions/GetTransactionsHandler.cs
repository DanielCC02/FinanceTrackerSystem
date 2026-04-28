using AutoMapper;
using AutoMapper.QueryableExtensions;
using FinanceTracker.Application.Features.Transactions.DTOs;
using FinanceTracker.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Features.Transactions.Queries.GetTransactions
{
    public class GetTransactionsHandler : IRequestHandler<GetTransactionsQuery, List<TransactionDto>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public GetTransactionsHandler(
            IApplicationDbContext dbContext,
            IMapper mapper,
            ICurrentUserService currentUser)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<List<TransactionDto>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Transactions
                .Where(t => t.Account.UserId == _currentUser.UserId)
                .ProjectTo<TransactionDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}