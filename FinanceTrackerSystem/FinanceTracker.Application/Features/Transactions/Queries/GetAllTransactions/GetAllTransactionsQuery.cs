using FinanceTracker.Application.Features.Transactions.DTOs;
using FinanceTracker.Domain.Enums;
using MediatR;

namespace FinanceTracker.Application.Features.Transactions.Queries.GetAllTransactions
{
    public record GetAllTransactionsQuery
    (
        TransactionType? Type,
        DateTime? From,
        DateTime? To,
        Guid? CategoryId
    ) : IRequest<List<TransactionDto>>;
}
