using FinanceTracker.Application.Features.Users.DTOs;
using MediatR;

namespace FinanceTracker.Application.Features.Users.Commands.UpdateUser
{
    public record UpdateUserCommand
    (
        Guid Id,
        string name,
        string email,
        string? password
    ) : IRequest<UserDto>;
}
