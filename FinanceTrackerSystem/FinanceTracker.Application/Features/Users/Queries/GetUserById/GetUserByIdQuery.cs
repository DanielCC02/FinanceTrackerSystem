using FinanceTracker.Application.Features.Users.DTOs;
using MediatR;

namespace FinanceTracker.Application.Features.Users.Queries.GetUserById
{
    public record GetUserByIdQuery
    (
        Guid Id
    ) : IRequest<UserDetailsDto>;
}
