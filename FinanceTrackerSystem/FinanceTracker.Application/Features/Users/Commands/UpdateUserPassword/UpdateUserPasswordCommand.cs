using MediatR;

namespace FinanceTracker.Application.Features.Users.Commands.UpdateUserPassword
{
    public record UpdateUserPasswordCommand
    (
        string Password
    ) : IRequest<Unit>;
}
