using MediatR;

namespace FinanceTracker.Application.Features.Auth.Commands.SendEmailConfirmation
{
    public record SendEmailConfirmationCommand
    (
        Guid UserId,
        string Email
    ) : IRequest<Unit>;
}