using MediatR;

namespace FinanceTracker.Application.Features.Auth.Commands.ForgotPassword
{
    public record ForgotPasswordCommand
    (
        string Email
    ):IRequest<Unit>;

}
