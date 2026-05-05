using MediatR;

namespace FinanceTracker.Application.Features.Auth.Commands.ResendConfirmation
{
    public record ResendConfirmationCommand
    (
        string Email
    ) : IRequest<Unit>;
}