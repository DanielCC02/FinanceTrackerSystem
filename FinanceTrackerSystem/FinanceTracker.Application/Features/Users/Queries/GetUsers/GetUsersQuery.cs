using FinanceTracker.Application.Features.Users.DTOs;
using MediatR;

namespace FinanceTracker.Application.Features.Users.Queries.GetUsers
{
    public record GetUsersQuery
    () : IRequest<List<UserDto>>;
}
