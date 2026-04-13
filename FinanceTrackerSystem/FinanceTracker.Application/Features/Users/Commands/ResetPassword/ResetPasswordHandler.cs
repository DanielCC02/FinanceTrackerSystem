using FinanceTracker.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Features.Users.Commands.ResetPassword
{
    public class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, Unit>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IPasswordHasher _passwordHasher;

        public ResetPasswordHandler(IApplicationDbContext dbContext, IPasswordHasher passwordHasher)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }

        public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var resetToken = await _dbContext.PasswordResetTokens
                .FirstOrDefaultAsync(t => t.Token == request.Token, cancellationToken);

            if (resetToken == null || !resetToken.IsValid())
                throw new Exception("Invalid or expired token");

            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == resetToken.UserId, cancellationToken) 
                ?? throw new Exception("User not found");

            var newPasswordHash = _passwordHasher.HashPassword(request.NewPassword);

            user.UpdatePassword(newPasswordHash);
            resetToken.MarkAsUsed();

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
