using MediatR;

namespace FinanceTracker.Application.Features.Users.Commands.DesactivateMyAccount
{
    public record DeactivateMyAccountCommand
    (
        string Password
    ) : IRequest<Unit>;
}
