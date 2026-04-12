using FinanceTracker.Application.Features.Users.DTOs;
using MediatR;


namespace FinanceTracker.Application.Features.Users.Queries.GetMyProfile
{
    public record GetMyProfileQuery
    (
    ) : IRequest<UserDto>;
}
