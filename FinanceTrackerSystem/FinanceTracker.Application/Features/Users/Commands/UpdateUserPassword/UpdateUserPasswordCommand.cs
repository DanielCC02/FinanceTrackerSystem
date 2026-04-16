using MediatR;

namespace FinanceTracker.Application.Features.Users.Commands.UpdateUserPassword
{
    public record UpdateUserPasswordCommand
    (
        string CurrentPassword,
        string NewPassword,
        string ConfirmPassword
    ) : IRequest<Unit>;
}
