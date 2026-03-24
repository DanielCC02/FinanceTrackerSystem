using MediatR;

namespace FinanceTracker.Application.Features.Transactions.Commands.CreateUser
{
    public record CreateUserCommand
    (
        string Name,
        string Email,
        string Password
    ) : IRequest<Guid>;
}
