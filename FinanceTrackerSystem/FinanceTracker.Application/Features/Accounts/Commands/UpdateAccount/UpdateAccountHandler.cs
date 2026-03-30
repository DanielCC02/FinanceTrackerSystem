using AutoMapper;
using FinanceTracker.Application.Features.Accounts.DTOs;
using FinanceTracker.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Features.Accounts.Commands.UpdateAccount
{
    public class UpdateAccountHandler : IRequestHandler<UpdateAccountCommand, AccountDto>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public UpdateAccountHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<AccountDto> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await _dbContext.Accounts
                .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken)
                ?? throw new KeyNotFoundException("Account not found");
            
            account.Update(request.Name, request.Type);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<AccountDto>(account);
        }
    }
}
