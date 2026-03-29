using AutoMapper;
using AutoMapper.QueryableExtensions;
using FinanceTracker.Application.Features.Accounts.DTOs;
using FinanceTracker.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Features.Accounts.Queries.GetAccounts
{
    public class GetAccountsHandler : IRequestHandler<GetAccountsQuery, List<AccountDto>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetAccountsHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<AccountDto>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Accounts.
                ProjectTo<AccountDto>(_mapper.ConfigurationProvider).
                ToListAsync(cancellationToken);
        }
    }
}
