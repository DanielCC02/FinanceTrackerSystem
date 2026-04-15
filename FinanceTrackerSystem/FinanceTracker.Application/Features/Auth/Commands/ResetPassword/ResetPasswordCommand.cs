using MediatR;

namespace FinanceTracker.Application.Features.Auth.Commands.ResetPassword
{
    public record ResetPasswordCommand
    (
        string Token,
        string NewPassword
    ) : IRequest<Unit>;

}
