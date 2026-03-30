using MediatR;

namespace FinanceTracker.Application.Features.Users.Commands.UpdateUserPassword
{
    public record UpdateUserPasswordCommand
    (
        Guid Id,
        string Password
    ) : IRequest<Unit>;
}
