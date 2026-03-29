

using AutoMapper;
using FinanceTracker.Application.Features.Accounts.DTOs;
using FinanceTracker.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Features.Accounts.Queries.GetAccountById
{
    public class GetAccountByIdHandler : IRequestHandler<GetAccountByIdQuery, AccountDto>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetAccountByIdHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<AccountDto> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
        {
            var account = await _dbContext.Accounts
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken)
                ?? throw new Exception("Account not found");

            if (account.IsDeleted)
                throw new Exception("Account is deleted");

            return _mapper.Map<AccountDto>(account);
        }
    }
}
