using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Infrastructure.Service.Security.Token;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Features.Auth.Commands.SendEmailConfirmation
{
    public class SendEmailConfirmationHandler : IRequestHandler<SendEmailConfirmationCommand, Unit>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IEmailService _emailService;

        public SendEmailConfirmationHandler(IApplicationDbContext dbContext, IEmailService emailService)
        {
            _dbContext = dbContext;
            _emailService = emailService;
        }

        public async Task<Unit> Handle(SendEmailConfirmationCommand request, CancellationToken cancellationToken)
        {
            // Invalidar tokens anteriores
            var oldTokens = await _dbContext.EmailConfirmationTokens
                .Where(t => t.UserId == request.UserId && !t.IsUsed)
                .ToListAsync(cancellationToken);

            foreach (var t in oldTokens)
                t.MarkAsUsed();

            // Generar nuevo token
            var rawToken = TokenGenerator.GenerateSecureToken();
            var hashedToken = TokenGenerator.HashToken(rawToken);

            var confirmToken = new EmailConfirmationToken(request.UserId, hashedToken);
            _dbContext.EmailConfirmationTokens.Add(confirmToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            // Cargar template y enviar
            var confirmLink = $"https://app.vaultly.com/confirm-email?token={Uri.EscapeDataString(rawToken)}";

            var html = await _emailService.LoadTemplateAsync("EmailConfirmation.html", new Dictionary<string, string>
            {
                { "confirmLink", confirmLink }
            });

            await _emailService.SendAsync(request.Email, "Confirm your email · Vaultly", html);

            return Unit.Value;
        }
    }
}