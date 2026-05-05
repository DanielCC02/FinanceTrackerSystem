using AutoMapper;
using AutoMapper.QueryableExtensions;
using FinanceTracker.Application.Features.Transactions.DTOs;
using FinanceTracker.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Features.Transactions.Queries.GetAllTransactions
{
    public class GetAllTransactionsHandler : IRequestHandler<GetAllTransactionsQuery, List<TransactionDto>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public GetAllTransactionsHandler(IApplicationDbContext dbContext, IMapper mapper, ICurrentUserService currentUser)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<List<TransactionDto>> Handle(GetAllTransactionsQuery request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Transactions
                .Where(t => t.Account!.UserId == _currentUser.UserId);

            if (request.Type.HasValue)
                query = query.Where(t => t.Type == request.Type);

            if (request.From.HasValue)
                query = query.Where(t => t.Date >= request.From);

            if (request.To.HasValue)
                query = query.Where(t => t.Date <= request.To);

            if (request.CategoryId.HasValue)
                query = query.Where(t => t.CategoryId == request.CategoryId);

            return await query
                .OrderByDescending(t => t.Date)
                .ProjectTo<TransactionDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
