using FinanceTracker.Application.Features.Auth.Commands.SendEmailConfirmation;
using FinanceTracker.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Features.Auth.Commands.ResendConfirmation
{
    public class ResendConfirmationHandler : IRequestHandler<ResendConfirmationCommand, Unit>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMediator _mediator;

        public ResendConfirmationHandler(IApplicationDbContext dbContext, IMediator mediator)
        {
            _dbContext = dbContext;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(ResendConfirmationCommand request, CancellationToken cancellationToken)
        {
            var email = request.Email.Trim().ToLower();

            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

            if (user == null || user.EmailConfirmed)
                return Unit.Value;

            await _mediator.Send(new SendEmailConfirmationCommand(user.Id, user.Email), cancellationToken);

            return Unit.Value;
        }
    }
}