using MediatR;

namespace FinanceTracker.Application.Features.Users.Commands.CreateUser
{
    public record CreateUserCommand
    (
        string Name,
        string Email,
        string Password
    ) : IRequest<CreateUserResponseDto>;
}
