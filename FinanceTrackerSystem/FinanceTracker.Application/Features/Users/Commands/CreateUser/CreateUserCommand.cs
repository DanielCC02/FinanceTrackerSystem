using FinanceTracker.Application.Features.Users.DTOs;
using FinanceTracker.Domain.Enums;
using MediatR;

namespace FinanceTracker.Application.Features.Users.Commands.CreateUser
{
    public record CreateUserCommand
    (
        string Name,
        string Email,
        UserRole Role,
        string Password
    ) : IRequest<UserDto>;
}
