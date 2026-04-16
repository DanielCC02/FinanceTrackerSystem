using FinanceTracker.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Features.Users.Commands.UpdateUserPassword
{
    public class UpdateUserPasswordHandler : IRequestHandler<UpdateUserPasswordCommand, Unit>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ICurrentUserService _currentUser;

        public UpdateUserPasswordHandler(IApplicationDbContext dbContext, IPasswordHasher passwordHasher, ICurrentUserService currentUser)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _currentUser = currentUser;
        }

        public async Task<Unit> Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
        {
            if (_currentUser.UserId == Guid.Empty)
                throw new UnauthorizedAccessException("User not authenticated");

            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == _currentUser.UserId, cancellationToken)
                ?? throw new KeyNotFoundException("User not found");

            if (user.IsDeleted)
                throw new UnauthorizedAccessException("User is inactive");

            var isValidPassword = _passwordHasher.VerifyPassword(user.PasswordHash, request.CurrentPassword);

            if (!isValidPassword)
                throw new UnauthorizedAccessException("Invalid current password");

            var isSamePassword = _passwordHasher.VerifyPassword(user.PasswordHash, request.NewPassword);

            if (isSamePassword)
                throw new InvalidOperationException("New password must be different from current password");

            var hashedPassword = _passwordHasher.HashPassword(request.NewPassword);

            user.UpdatePassword(hashedPassword);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
