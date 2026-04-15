using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Infrastructure.Service.Security.Token;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Features.Auth.Commands.ForgotPassword
{
    public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordCommand, Unit>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IEmailService _emailService;

        public ForgotPasswordHandler(IApplicationDbContext dbContext, IEmailService emailService)
        {
            _dbContext = dbContext;
            _emailService = emailService;
        }

        public async Task<Unit> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var email = request.Email.Trim().ToLower();

            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

            if (user == null)
                return Unit.Value;

            var oldTokens = await _dbContext.PasswordResetTokens
                .Where(t => t.UserId == user.Id && !t.IsUsed)
                .ToListAsync(cancellationToken);

            foreach (var t in oldTokens)
            {
                t.MarkAsUsed();
            }

            var rawToken = TokenGenerator.GenerateSecureToken();
            var hashedToken = TokenGenerator.HashToken(rawToken);

            var resetToken = new PasswordResetToken(user.Id, hashedToken);

            _dbContext.PasswordResetTokens.Add(resetToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            //Email sending logic

            var resetLink = $"https://yourapp.com/reset-password?token={Uri.EscapeDataString(rawToken)}";

            var htmlContent = $@"
                <h2>Password Reset</h2>
                <p>Click the link below to reset your password:</p>
                <a href='{resetLink}'>Reset your password</a>
                <p>This link expires in 15 minutes. If you did not request a password reset, please ignore this email.</p>
            ";

            await _emailService.SendAsync(user.Email, "Reset your password", htmlContent);

            return Unit.Value;
        }
    }
}
