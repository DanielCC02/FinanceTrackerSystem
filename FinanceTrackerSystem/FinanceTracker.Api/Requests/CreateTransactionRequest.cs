using FinanceTracker.Domain.Enums;

namespace FinanceTracker.API.Requests
{
    public record CreateTransactionRequest
    (
        Guid? CategoryId,
        decimal Amount,
        TransactionType Type,
        string Description,
        DateTime Date
    );
}