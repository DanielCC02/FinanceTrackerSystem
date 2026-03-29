using FinanceTracker.Application.Features.Accounts.DTOs;
using MediatR;

namespace FinanceTracker.Application.Features.Accounts.Queries.GetAccounts
{
    public record GetAccountsQuery
    () : IRequest<List<AccountDto>>;
}
