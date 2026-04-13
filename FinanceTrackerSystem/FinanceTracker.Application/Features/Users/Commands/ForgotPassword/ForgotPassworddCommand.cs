
using MediatR;

namespace FinanceTracker.Application.Features.Users.Commands.ForgotPassword
{
    public record ForgotPassworddCommand
    (
        string Email
    ):IRequest<Unit>;

}
