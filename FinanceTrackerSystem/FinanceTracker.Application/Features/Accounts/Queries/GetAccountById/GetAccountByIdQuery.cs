using FinanceTracker.Application.Features.Accounts.DTOs;
using MediatR;

namespace FinanceTracker.Application.Features.Accounts.Queries.GetAccountById
{
    public record GetAccountByIdQuery
    (
        Guid Id
    ) : IRequest<AccountDto>;
}
