using MediatR;

namespace FinanceTracker.Application.Features.Users.Commands.ResetPassword
{
    public record ResetPasswordCommand
    (
        string Token,
        string NewPassword
    ) : IRequest<Unit>;

}
