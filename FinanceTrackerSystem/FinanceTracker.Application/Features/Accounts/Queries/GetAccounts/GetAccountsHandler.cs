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
        private readonly ICurrentUserService _currentUser;

        public GetAccountsHandler(IApplicationDbContext dbContext, IMapper mapper, ICurrentUserService currentUser)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<List<AccountDto>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Accounts
                .Where(a => a.UserId == _currentUser.UserId)
                .ProjectTo<AccountDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
