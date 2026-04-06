using FinanceTracker.Application.Features.Accounts.DTOs;
using FinanceTracker.Domain.Enums;
using MediatR;


namespace FinanceTracker.Application.Features.Accounts.Commands.CreateAccount
{
    public record CreateAccountCommand
    (
        string Name,
        AccountType Type
    ) : IRequest<AccountDto>;
}
