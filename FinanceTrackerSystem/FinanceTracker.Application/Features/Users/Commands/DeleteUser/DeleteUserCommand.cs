using MediatR;

namespace FinanceTracker.Application.Features.Users.Commands.DeleteUser
{
    public record DeleteUserCommand
    (
        Guid Id
    ) : IRequest<Unit>;
}
