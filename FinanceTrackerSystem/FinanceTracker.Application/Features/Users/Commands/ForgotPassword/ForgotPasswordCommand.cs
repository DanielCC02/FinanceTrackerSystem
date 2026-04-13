
using MediatR;

namespace FinanceTracker.Application.Features.Users.Commands.ForgotPassword
{
    public record ForgotPasswordCommand
    (
        string Email
    ):IRequest<Unit>;

}
