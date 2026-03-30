using FinanceTracker.Application.Features.Transactions.DTOs;
using MediatR;

namespace FinanceTracker.Application.Features.Transactions.Queries.GetTransactionById
{
    public record GetTransactionByIdQuery
    (
        Guid Id
    ) : IRequest<TransactionDto>;
}
