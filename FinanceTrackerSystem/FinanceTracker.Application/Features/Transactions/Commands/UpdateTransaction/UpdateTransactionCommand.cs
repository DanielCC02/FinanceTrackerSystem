using FinanceTracker.Application.Features.Transactions.DTOs;
using FinanceTracker.Domain.Enums;
using MediatR;

namespace FinanceTracker.Application.Features.Transactions.Commands.UpdateTransaction
{
    public record UpdateTransactionCommand
    (
        Guid Id,
        Guid? CategoryId,
        decimal Amount,
        TransactionType Type,
        string Description,
        DateTime Date
    ) : IRequest<TransactionDto>;
}
