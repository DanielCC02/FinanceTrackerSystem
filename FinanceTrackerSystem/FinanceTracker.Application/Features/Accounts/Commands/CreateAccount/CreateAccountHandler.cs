using AutoMapper;
using FinanceTracker.Application.Features.Accounts.DTOs;
using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Features.Accounts.Commands.CreateAccount
{
    public class CreateAccountHandler : IRequestHandler<CreateAccountCommand, AccountDto>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public CreateAccountHandler(IApplicationDbContext dbContext, IMapper mapper, ICurrentUserService currentUser)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<AccountDto> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var name = request.Name.Trim();

            var account = new Account(
                _currentUser.UserId,
                name,
                request.Type);

            _dbContext.Accounts.Add(account);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<AccountDto>(account);

        }
    }
}
