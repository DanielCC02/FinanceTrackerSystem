using FinanceTracker.Application.Interfaces;
using FinanceTracker.Infrastructure.Service.Security.Token;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Features.Auth.Commands.ResetPassword
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
            var hashedToken = TokenGenerator.HashToken(request.Token);

            var resetToken = await _dbContext.PasswordResetTokens
                .FirstOrDefaultAsync(t => t.Token == hashedToken, cancellationToken);

            if (resetToken == null || !resetToken.IsValid())
                throw new UnauthorizedAccessException("Invalid or expired token");

            if (string.IsNullOrWhiteSpace(request.NewPassword) || request.NewPassword.Length < 6)
                throw new ArgumentException("Password must be at least 6 characters");

            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == resetToken.UserId, cancellationToken) 
                ?? throw new Exception("User not found");

            var newPasswordHash = _passwordHasher.HashPassword(request.NewPassword);

            var allTokens = await _dbContext.PasswordResetTokens
                .Where(t => t.UserId == user.Id && !t.IsUsed)
                .ToListAsync(cancellationToken);

            foreach (var t in allTokens)
            {
                t.MarkAsUsed();
            }

            user.UpdatePassword(newPasswordHash);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
