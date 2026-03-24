using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Features.Transactions.Commands.CreateUser
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IApplicationDbContext _dbContext;

        public CreateUserHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var emailExists = await _dbContext.Users
                .AnyAsync(a => a.Email == request.Email, cancellationToken);

            if (emailExists)
            {
                throw new InvalidOperationException("Email already exists.");
            }

            var user = new User(
                request.Name,
                request.Email,
                request.Password);

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return user.Id;
        }
    }
}
