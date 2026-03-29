using FinanceTracker.Application.Features.Accounts.DTOs;
using FinanceTracker.Domain.Enums;
using MediatR;


namespace FinanceTracker.Application.Features.Accounts.Commands.UpdateAccount
{
    public record UpdateAccountCommand
    (
        Guid Id,
        string Name,
        AccountType Type
    ) : IRequest<AccountDto>;
}
