using MediatR;

namespace FinanceTracker.Application.Features.Users.Commands.DesactivateMyAccount
{
    public record DesactiveMyAccountCommand
    (
    ) : IRequest<Unit>;
}
