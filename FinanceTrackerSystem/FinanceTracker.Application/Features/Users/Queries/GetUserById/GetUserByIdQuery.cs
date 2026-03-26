using FinanceTracker.Application.Features.Users.Queries.GetUsers;
using MediatR;

namespace FinanceTracker.Application.Features.Users.Queries.GetUserById
{
    public record GetUserByIdQuery
    (
        Guid Id
    ) : IRequest<UserDto>;
}
