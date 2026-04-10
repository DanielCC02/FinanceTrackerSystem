using FinanceTracker.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Features.Users.Commands.UpdateUserPassword
{
    public class UpdateUserPasswordHandler : IRequestHandler<UpdateUserPasswordCommand, Unit>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IPasswordHasher _passwordHasher;

        public UpdateUserPasswordHandler(IApplicationDbContext dbContext, IPasswordHasher passwordHasher)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }

        public async Task<Unit> Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken)
                ?? throw new KeyNotFoundException("User not found");      
            

            var hashedPassword = _passwordHasher.HashPassword(request.Password);

            user.UpdatePassword(hashedPassword);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
