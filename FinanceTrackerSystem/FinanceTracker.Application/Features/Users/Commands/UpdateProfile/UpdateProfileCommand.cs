using FinanceTracker.Application.Features.Users.DTOs;
using MediatR;

namespace FinanceTracker.Application.Features.Users.Commands.UpdateProfile
{
    public record UpdateProfileCommand
    (
        string FirstName,
        string LastName,
        string PhoneNumber
    ) : IRequest<UserDto>;

}
