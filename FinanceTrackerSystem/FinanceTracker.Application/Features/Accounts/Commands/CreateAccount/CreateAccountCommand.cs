using FinanceTracker.Domain.Enums;
using MediatR;


namespace FinanceTracker.Application.Features.Accounts.Commands.CreateAccount
{
    public record CreateAccountCommand
    (
        Guid UserId,
        string Name,
        AccountType Type
    ) : IRequest<Guid>;
}
