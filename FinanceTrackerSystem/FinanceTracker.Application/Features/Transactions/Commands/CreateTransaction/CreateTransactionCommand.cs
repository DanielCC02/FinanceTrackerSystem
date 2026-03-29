using FinanceTracker.Application.Features.Transactions.DTOs;
using FinanceTracker.Domain.Enums;
using MediatR;


namespace FinanceTracker.Application.Features.Transactions.Commands.CreateTransaction
{
    public record CreateTransactionCommand
    (
        Guid AccountId,
        Guid? CategoryId,
        decimal Amount,
        TransactionType Type,
        string Description,
        DateTime Date
    ) : IRequest<TransactionDto>;
}
