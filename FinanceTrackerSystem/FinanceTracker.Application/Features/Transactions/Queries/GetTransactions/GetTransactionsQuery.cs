using FinanceTracker.Application.Features.Transactions.DTOs;
using MediatR;

namespace FinanceTracker.Application.Features.Transactions.Queries.GetTransactions
{
    public record GetTransactionsQuery
    () : IRequest<List<TransactionDto>>;
}
