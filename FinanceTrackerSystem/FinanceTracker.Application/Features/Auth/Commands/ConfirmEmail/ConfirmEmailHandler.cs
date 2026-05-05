using FinanceTracker.Application.Interfaces;
using FinanceTracker.Infrastructure.Service.Security.Token;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Features.Auth.Commands.ConfirmEmail
{
    public class ConfirmEmailHandler : IRequestHandler<ConfirmEmailCommand, Unit>
    {
        private readonly IApplicationDbContext _dbContext;

        public ConfirmEmailHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var hashedToken = TokenGenerator.HashToken(request.Token);

            var token = await _dbContext.EmailConfirmationTokens
                .FirstOrDefaultAsync(t => t.Token == hashedToken, cancellationToken);

            if (token == null || token.IsUsed || token.ExpiresAt < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Invalid or expired confirmation token");

            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == token.UserId, cancellationToken)
                ?? throw new KeyNotFoundException("User not found");

            user.ConfirmEmail();
            token.MarkAsUsed();

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}